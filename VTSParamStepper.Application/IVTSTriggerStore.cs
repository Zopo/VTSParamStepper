using VTSParamStepper.ValueObjects;

namespace VTSParamStepper.Application;

public interface IVTSTriggerStore
{
    void AddTrigger(ParamTrigger trigger);
    
    ParamTrigger? GetHotkeyTrigger(string hotkeyName);
}