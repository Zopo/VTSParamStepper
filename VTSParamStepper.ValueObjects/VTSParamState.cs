using VTS.Core;

namespace VTSParamStepper.ValueObjects;

public class VTSParamState(VTSCustomParameter parameter)
{
    public VTSCustomParameter VTSParameter { get; } = parameter;
    public float CurrentValue { get; set; } = parameter.defaultValue;
    public DateTimeOffset LastUpdated { get; private set; } = DateTimeOffset.UtcNow;
    
    public float StepValue(float step)
    {
        this.CurrentValue += step;
        this.CurrentValue = step switch
        {
            > 0 => Math.Min(this.CurrentValue, this.VTSParameter.max),
            < 0 => Math.Max(this.CurrentValue, this.VTSParameter.min),
            _ => this.CurrentValue
        };

        this.LastUpdated = DateTimeOffset.UtcNow;
        return this.CurrentValue;
    }
}