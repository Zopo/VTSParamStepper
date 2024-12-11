using Microsoft.Extensions.Hosting;
using VTS.Core;
using VTSParamStepper.ValueObjects;
using VTSParamStepper.Web;

namespace VTSParamStepper.Application;

public class TriggerExecutionService(IVTSTriggerStore triggerStore, IVTSParamStore paramStore, IVTSPlugin plugin) : BackgroundService
{
    public static TimeSpan TickRate { get; } = TimeSpan.FromMilliseconds(20);
    
    private async Task Setup()
    {
        await plugin.SubscribeToHotkeyTriggeredEvent(new VTSHotkeyTriggeredEventConfigOptions(false), (triggeredEvent) =>
        {
            var paramTrigger = triggerStore.GetHotkeyTrigger(triggeredEvent.data.hotkeyName);
            if (paramTrigger != null)
            {
                this.ExecuteTrigger(paramTrigger);
            }
        });
    }

    private async Task ExecuteTrigger(ParamTrigger trigger)
    {
        
        if (trigger.TimeSpan == null)
        {
            // Simple parameter stepping
            var paramToUpdate = paramStore.GetParameter(trigger.ParamName);
            paramToUpdate?.StepValue(trigger.ValueStep);
        }
        else
        {
            var paramToUpdate = paramStore.GetParameter(trigger.ParamName);

            var numberOfExecutionSteps = Math.Ceiling(trigger.TimeSpan.Value.Divide(TickRate));
            var stepRange = trigger.ValueStep / numberOfExecutionSteps;
            var stepped = 0f;
            var executionStep = 1;
            while (executionStep < numberOfExecutionSteps)
            {
                paramToUpdate?.StepValue((float)stepRange);
                stepped += (float)stepRange;
                await Task.Delay(TickRate);
                executionStep++;
            }
            
            var remainder = trigger.ValueStep - stepped;
            paramToUpdate?.StepValue((float)remainder);
        }
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await this.Setup();
    }
}