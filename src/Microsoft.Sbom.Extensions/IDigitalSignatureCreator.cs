// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Microsoft.Sbom.Extensions;

using System.IO;
using System.Security.Cryptography.X509Certificates;

/// <summary>
/// Creates a digital signature for the given SBOM file with the certificate provided by the caller.
/// </summary>
public interface IDigitalSignatureCreator
{
    /// <summary>
    /// Creates a digital signature for the given SBOM file with the certificate provided by the caller.
    /// </summary>
    /// <param name="sbomFileStream">A read stream to the content of the SBOM file.</param>
    /// <param name="signingCertificate">The certificate used for creating the digital signature.</param>
    /// <returns>The digital signature in string format.</returns>
    string CreateDigitalSignature(
        Stream sbomFileStream,
        X509Certificate2 signingCertificate);
}
