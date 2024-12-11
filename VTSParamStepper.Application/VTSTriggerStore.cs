using VTSParamStepper.ValueObjects;

namespace VTSParamStepper.Application;

public class VTSTriggerStore : IVTSTriggerStore
{
    private readonly IList<ParamTrigger> _triggers = new List<ParamTrigger>();

    public void AddTrigger(ParamTrigger trigger)
    {
        _triggers.Add(trigger);
    }

    public ParamTrigger? GetHotkeyTrigger(string hotkeyName)
    {
        var paramTrigger = this._triggers.FirstOrDefault(x =>
            x is HotkeyTrigger trigger && trigger.HotkeyName.Equals(hotkeyName, StringComparison.CurrentCultureIgnoreCase));
        return paramTrigger;
    }
}