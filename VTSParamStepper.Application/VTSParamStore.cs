using VTS.Core;
using VTSParamStepper.ValueObjects;
using VTSParamStepper.Web;

namespace VTSParamStepper.Application;

public class VTSParamStore : IVTSParamStore
{
    private readonly IDictionary<string, VTSParamState> _parameters = new Dictionary<string, VTSParamState>();

    public VTSParamState? GetParameter(string key)
    {
        this._parameters.TryGetValue(key, out VTSParamState? customParameter);
        return customParameter;
    }

    public void RegisterParameter(VTSCustomParameter customParameter)
    {
        var paramState = new VTSParamState(customParameter);
        this._parameters.Add(customParameter.parameterName, paramState);
    }

    public IEnumerable<VTSParamState> GetParameters()
    {
        return this._parameters.Values;
    }
}