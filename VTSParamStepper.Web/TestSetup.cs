using VTS.Core;
using VTSParamStepper.Application;
using VTSParamStepper.ValueObjects;

namespace VTSParamStepper.Web;

public class TestSetup(IVTSParamStore paramStore, IVTSTriggerStore triggerStore, IVTSPlugin plugin) : BackgroundService
{ 
    private async Task Setup()
    {
        var controllableParam = new VTSCustomParameter();
        controllableParam.parameterName = "CustomParamTest";
        controllableParam.min = 0;
        controllableParam.max = 1;
        controllableParam.defaultValue = 0f;
        paramStore.RegisterParameter(controllableParam);
        await plugin.AddCustomParameter(controllableParam);
        
        triggerStore.AddTrigger(new HotkeyTrigger("TestPlus", controllableParam.parameterName, 0.05f, TimeSpan.FromSeconds(10)));
        triggerStore.AddTrigger(new HotkeyTrigger("TestMinus", controllableParam.parameterName, -0.05f, TimeSpan.FromSeconds(10)));
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        await this.Setup();
    }
}