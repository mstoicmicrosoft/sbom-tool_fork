// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Sbom.Extensions;

namespace Microsoft.Sbom.Api.DigitalSignatureCreator;

/// <summary>
/// A type that provides a <see cref="IDigitalSignatureCreator"/> implementation.
/// </summary>
public interface IDigitalSignatureCreatorProvider
{
    IDigitalSignatureCreator Get();

    void Init();
}
