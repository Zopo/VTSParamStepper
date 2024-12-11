using VTS.Core;
using VTSParamStepper.ValueObjects;

namespace VTSParamStepper.Web;

public interface IVTSParamStore
{
    void RegisterParameter(VTSCustomParameter customParameter);
    VTSParamState? GetParameter(string key);
    IEnumerable<VTSParamState> GetParameters();
}