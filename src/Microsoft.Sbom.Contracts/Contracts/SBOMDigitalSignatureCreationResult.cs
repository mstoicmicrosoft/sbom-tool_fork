// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System.Collections.Generic;

namespace Microsoft.Sbom.Contracts;

/// <summary>
/// Represents the result of a SBOM generation action.
/// </summary>
public class SBOMDigitalSignatureCreationResult
{
    /// <summary>
    /// Gets or sets a value indicating whether is set to true if the SBOM digital signature creation was successful,
    /// that is when the <see cref="Errors"/> list is empty.
    /// </summary>
    public bool IsSuccessful { get; set; }

    /// <summary>
    /// Gets a list of errors that were encountered during the SBOM digital signature creation.
    /// </summary>
    public IList<EntityError> Errors { get; private set; }

    public SBOMDigitalSignatureCreationResult(bool isSuccessful, IList<EntityError> errors)
    {
        IsSuccessful = isSuccessful;
        Errors = errors ?? new List<EntityError>();
    }
}
