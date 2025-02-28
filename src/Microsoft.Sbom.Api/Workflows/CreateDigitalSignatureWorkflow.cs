// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Microsoft.Sbom.Api.Workflows;

using System;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using Microsoft.Sbom.Api.DigitalSignatureCreator;
using Microsoft.Sbom.Api.Output;
using Microsoft.Sbom.Api.Output.Telemetry;
using Microsoft.Sbom.Api.Utils;
using Microsoft.Sbom.Common;
using Microsoft.Sbom.Common.Config;
using Microsoft.Sbom.Extensions;
using Serilog;

/// <summary>
/// Creates a digital signature for the given SBOM file with the certificate provided by the caller.
/// </summary>
public class CreateDigitalSignatureWorkflow : IWorkflow<CreateDigitalSignatureWorkflow>
{
    private readonly X509Certificate2 signingCertificate;
    private readonly IRecorder recorder;
    private readonly IDigitalSignatureCreatorProvider digitalSignatureCreatorProvider;
    private readonly ILogger log;
    private readonly IFileSystemUtils fileSystemUtils;
    private readonly ISbomConfigProvider sbomConfigs;
    private readonly IConfiguration configuration;
    private readonly IOutputWriter outputWriter;

    public CreateDigitalSignatureWorkflow(
        X509Certificate2 signingCertificate,
        IRecorder recorder,
        IDigitalSignatureCreatorProvider digitalSignatureCreatorProvider,
        ILogger log,
        IFileSystemUtils fileSystemUtils,
        ISbomConfigProvider sbomConfigs,
        IConfiguration configuration,
        IOutputWriter outputWriter)
    {
        this.signingCertificate = signingCertificate ?? throw new ArgumentNullException(nameof(signingCertificate));
        this.recorder = recorder ?? throw new ArgumentNullException(nameof(recorder));
        this.digitalSignatureCreatorProvider = digitalSignatureCreatorProvider ?? throw new ArgumentNullException(nameof(digitalSignatureCreatorProvider));
        this.log = log ?? throw new ArgumentNullException(nameof(log));
        this.fileSystemUtils = fileSystemUtils ?? throw new ArgumentNullException(nameof(fileSystemUtils));
        this.sbomConfigs = sbomConfigs ?? throw new ArgumentNullException(nameof(sbomConfigs));
        this.configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        this.outputWriter = outputWriter ?? throw new ArgumentNullException(nameof(outputWriter));
    }

    public async Task<bool> RunAsync()
    {
        using (recorder.TraceEvent(Events.SBOMValidationWorkflow))
        {
            try
            {
                var sbomConfig = sbomConfigs.Get(configuration.ManifestInfo.Value.FirstOrDefault());
                using var sbomFileStream = fileSystemUtils.OpenRead(sbomConfig.ManifestJsonFilePath);

                var digitalSignatureCreator = digitalSignatureCreatorProvider.Get();

                if (digitalSignatureCreator == null)
                {
                    log.Error("Could not find a digital signature creator. Cannot sign.");
                    return false;
                }

                var signature = digitalSignatureCreator.CreateDigitalSignature(
                    sbomFileStream,
                    signingCertificate);

                if (string.IsNullOrWhiteSpace(signature))
                {
                    log.Error("Digital signature creation failed.");
                    return false;
                }

                // write the signature to the file
                // TODO: how does this get configured to know where to write the file and what to name it?
                await outputWriter.WriteAsync(signature);

                return true;
            }
            catch (Exception e)
            {
                recorder.RecordException(e);
                log.Error("Encountered an error while creating the digital signature for the SBOM file.");
                log.Error($"Error details: {e.Message}");
                return false;
            }
        }
    }
}
