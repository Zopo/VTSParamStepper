namespace VTSParamStepper.ValueObjects;

public class HotkeyTrigger(string hotkeyName, string paramName, float valueStep, TimeSpan? timeSpan = null)
    : ParamTrigger(paramName, valueStep, timeSpan)
{
    public string HotkeyName { get; } = hotkeyName;
}