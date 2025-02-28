// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;
using Microsoft.Sbom.Api.Convertors;
using Microsoft.Sbom.Api.DigitalSignatureCreator;
using Microsoft.Sbom.Api.Entities.Output;
using Microsoft.Sbom.Api.Executors;
using Microsoft.Sbom.Api.Filters;
using Microsoft.Sbom.Api.Hashing;
using Microsoft.Sbom.Api.Manifest;
using Microsoft.Sbom.Api.Manifest.Configuration;
using Microsoft.Sbom.Api.Output;
using Microsoft.Sbom.Api.Output.Telemetry;
using Microsoft.Sbom.Api.Recorder;
using Microsoft.Sbom.Api.SignValidator;
using Microsoft.Sbom.Api.Tests;
using Microsoft.Sbom.Api.Utils;
using Microsoft.Sbom.Api.Workflows;
using Microsoft.Sbom.Api.Workflows.Helpers;
using Microsoft.Sbom.Common;
using Microsoft.Sbom.Common.Config;
using Microsoft.Sbom.Contracts;
using Microsoft.Sbom.Contracts.Enums;
using Microsoft.Sbom.Extensions;
using Microsoft.Sbom.Extensions.Entities;
using Microsoft.Sbom.JsonAsynchronousNodeKit;
using Microsoft.Sbom.Parser;
using Microsoft.Sbom.Utils;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Serilog;
using Constants = Microsoft.Sbom.Api.Utils.Constants;
using ErrorType = Microsoft.Sbom.Api.Entities.ErrorType;
using SpdxChecksum = Microsoft.Sbom.Parsers.Spdx22SbomParser.Entities.Checksum;

namespace Microsoft.Sbom.Api.Tests.Workflows;

/// <summary>
/// Tests for the CreateDigitalSignatureWorkflow.
/// </summary>
[TestClass]
public class CreateDigitalSignatureWorkflowTests : ValidationWorkflowTestsBase
{
    private readonly Mock<ILogger> mockLogger = new();
    private readonly Mock<IDigitalSignatureCreator> digitalSignatureCreatorMock = new();
    private readonly Mock<IDigitalSignatureCreatorProvider> digitalSignatureCreatorProviderMock = new();

    [TestInitialize]
    public void Init()
    {
        var signature = "signatureString";
        digitalSignatureCreatorMock.Setup(s => s.CreateDigitalSignature(
            It.IsAny<Stream>(),
            It.IsAny<X509Certificate2>()))
            .Returns(signature);
        digitalSignatureCreatorProviderMock.Setup(s => s.Get()).Returns(digitalSignatureCreatorMock.Object);
    }

    [TestMethod]
    public async Task CreateDigitalSignatureWorkflowTests_Succeeds()
    {
        var createDigitalSignatureWorkflow = new CreateDigitalSignatureWorkflow(
            new X509Certificate2(),
            new Recorder(),
            digitalSignatureCreatorProviderMock.Object,
            mockLogger.Object,
            new FileSystemUtils(),
            new SbomConfigProvider(),
            new Configuration(),
            new OutputWriter());

        var cc = new ConsoleCapture();

        try
        {
            var result = await createDigitalSignatureWorkflow.RunAsync();
            Assert.IsTrue(result);
        }
        finally
        {
            cc.Restore();
        }

        // Asserts go in here on the output captured
        // Test that the file was written with the signature if that is supported / enabled (add a param for that)
    }
}
