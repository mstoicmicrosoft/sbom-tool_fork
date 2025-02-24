// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using Microsoft.Sbom.Extensions;

namespace Microsoft.Sbom.Api.DigitalSignatureCreator;

/// <summary>
/// Factory class that provides a <see cref="IDigitalSignatureCreator"/> implementation.
/// </summary>
public class DigitalSignatureCreatorProvider : IDigitalSignatureCreatorProvider
{
    private readonly IDigitalSignatureCreator digitalSignatureCreator;

    public DigitalSignatureCreatorProvider(IDigitalSignatureCreator digitalSignatureCreator)
    {
        this.digitalSignatureCreator = digitalSignatureCreator;
        this.Init();
    }

    public IDigitalSignatureCreator Get()
    {
        return this.digitalSignatureCreator;
    }

    public void Init()
    {
    }
}
