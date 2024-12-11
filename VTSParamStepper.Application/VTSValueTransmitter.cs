using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using VTS.Core;
using VTSParamStepper.Web;

namespace VTSParamStepper.Application;

public class VTSValueTransmitter(IConfiguration config, IVTSPlugin vtsPlugin, IVTSParamStore vtsParamStore) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        var lastTransmittedTime = DateTimeOffset.UtcNow;
        while (!stoppingToken.IsCancellationRequested)
        {
            if (vtsPlugin.IsAuthenticated)
            {
                var now = DateTimeOffset.UtcNow;
                var parametersToUpdate = vtsParamStore.GetParameters().Where(x =>
                    new TimeSpan(now.Ticks - x.LastUpdated.Ticks) > TimeSpan.FromMilliseconds(500) ||
                    lastTransmittedTime < x.LastUpdated);
                var injectionValues = parametersToUpdate.Select(x => new VTSParameterInjectionValue
                    { id = x.VTSParameter.parameterName, value = x.CurrentValue, weight = 1f });
                var vtsParameterInjectionValues = injectionValues as VTSParameterInjectionValue[] ?? injectionValues.ToArray();
                if (vtsParameterInjectionValues.Any())
                {
                    lastTransmittedTime = now;
                    await vtsPlugin.InjectParameterValues(vtsParameterInjectionValues);
                }
            }

            await Task.Delay(TimeSpan.FromMilliseconds(20), stoppingToken);
        }
    }
}