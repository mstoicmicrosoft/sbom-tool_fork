// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Microsoft.Sbom.Api;

using System;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using Microsoft.Sbom.Api.Output.Telemetry;
using Microsoft.Sbom.Api.Workflows;
using Microsoft.Sbom.Contracts;

public class SBOMDigitalSignatureCreator : ISBOMDigitalSignatureCreator
{
    private readonly IWorkflow<CreateDigitalSignatureWorkflow> createDigitalSignatureWorkflow;
    private readonly IRecorder recorder;

    public SBOMDigitalSignatureCreator(
        IWorkflow<CreateDigitalSignatureWorkflow> createDigitalSignatureWorkflow,
        IRecorder recorder)
    {
        this.createDigitalSignatureWorkflow = createDigitalSignatureWorkflow ?? throw new ArgumentNullException(nameof(createDigitalSignatureWorkflow));
        this.recorder = recorder ?? throw new ArgumentNullException(nameof(recorder));
    }

    /// <inheritdoc>
    public async Task<SBOMDigitalSignatureCreationResult> CreateDigitalSignatureAsync(
        string sbomFilePath,
        X509Certificate2 signingCertificate)
    {
        if (signingCertificate == null)
        {
            throw new ArgumentNullException(nameof(signingCertificate));
        }

        if (!signingCertificate.HasPrivateKey)
        {
            throw new ArgumentException("Provided signing certificate does not have a private key. Cannot create a digital signature using it.");
        }

        if (!File.Exists(sbomFilePath))
        {
            throw new FileNotFoundException($"SBOM file not found in specified location: {sbomFilePath}");
        }

        var isSuccess = await createDigitalSignatureWorkflow.RunAsync();
        await recorder.FinalizeAndLogTelemetryAsync();

        var errors = recorder.Errors.Select(error => error.ToEntityError()).ToList();
        return new SBOMDigitalSignatureCreationResult(!errors.Any(), errors);
    }
}
