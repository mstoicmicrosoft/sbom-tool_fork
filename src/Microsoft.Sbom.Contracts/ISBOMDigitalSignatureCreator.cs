// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

namespace Microsoft.Sbom.Contracts;

using System.Security.Cryptography.X509Certificates;
using System.Threading.Tasks;

/// <summary>
/// Provides an interface to create a digital signature for a SBOM file.
/// </summary>
public interface ISBOMDigitalSignatureCreator
{
    /// <summary>
    /// Creates a digital signature for a given SBOM file with a provided certificate and writes it in a file
    /// in the same folder as the SBOM file.
    /// </summary>
    /// <param name="sbomFilePath">Path to the SBOM file which is going to be signed.</param>
    /// <param name="signingCertificate">The certificate which will be used to generate the digital signature.</param>
    /// <returns></returns>
    Task<SBOMDigitalSignatureCreationResult> CreateDigitalSignatureAsync(
        string sbomFilePath,
        X509Certificate2 signingCertificate);
}
