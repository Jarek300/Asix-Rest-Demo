using Asix;

namespace BlazorWebAssemblyApp.Code;

public class AlarmModel
{
    public string Name;
    public AlarmState AlarmState = new AlarmState();

    public AlarmModel(string aName)
    {
        Name = aName;
    }
}
