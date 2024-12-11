namespace VTSParamStepper.ValueObjects;

public abstract class ParamTrigger(string paramName, float valueStep, TimeSpan? timeSpan = null)
{
    public string ParamName { get; } = paramName;
    public float ValueStep { get; } = valueStep;
    public TimeSpan? TimeSpan { get; } = timeSpan;
}