// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using PowerArgs;

namespace Microsoft.Sbom.Api.Config.Args;

public class CreateDigitalSignatureArgs : CommonArgs
{
    /// <summary>
    /// Gets or sets the path to the manifest.json file which needs to be signed.
    /// </summary>
    [ArgShortcut("f")]
    [ArgDescription("Specifies the path to the manifest.json file which needs to be signed.")]
    public string SBOMFilePath { get; set; }

    /// <summary>
    /// Gets or sets the path to the certificate file which will be used to sign.
    /// </summary>
    [ArgShortcut("sc")]
    [ArgDescription("Specifies the path to the certificate file which will be used to sign.")]
    public string SigningCertificateFilePath { get; set; }
}
