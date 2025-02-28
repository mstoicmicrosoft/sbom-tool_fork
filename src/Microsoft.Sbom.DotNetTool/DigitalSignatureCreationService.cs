// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.

using System;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Microsoft.Sbom.Api;
using Microsoft.Sbom.Api.Output.Telemetry;
using Microsoft.Sbom.Api.Workflows;

namespace Microsoft.Sbom;

public class DigitalSignatureCreationService : IHostedService
{
    private readonly IWorkflow<CreateDigitalSignatureWorkflow> createDigitalSignatureWorkflow;

    private readonly IRecorder recorder;

    private readonly IHostApplicationLifetime hostApplicationLifetime;

    public DigitalSignatureCreationService(
        IWorkflow<CreateDigitalSignatureWorkflow> createDigitalSignatureWorkflow,
        IRecorder recorder,
        IHostApplicationLifetime hostApplicationLifetime)
    {
        this.createDigitalSignatureWorkflow = createDigitalSignatureWorkflow;
        this.recorder = recorder;
        this.hostApplicationLifetime = hostApplicationLifetime;
    }

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        bool result;
        try
        {
            result = await createDigitalSignatureWorkflow.RunAsync();

            await recorder.FinalizeAndLogTelemetryAsync();
            Environment.ExitCode = result ? (int)ExitCode.Success : (int)ExitCode.GeneralError;
        }
        catch (Exception e)
        {
            var message = e.InnerException != null ? e.InnerException.Message : e.Message;
            Console.WriteLine($"Encountered error while running create digital signature workflow. Error: {message}");
            Environment.ExitCode = (int)ExitCode.GeneralError;
        }

        hostApplicationLifetime.StopApplication();
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
}
