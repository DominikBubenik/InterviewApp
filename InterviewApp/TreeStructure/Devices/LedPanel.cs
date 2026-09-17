using InterviewApp;

namespace InterviewApp.Devices;

public class LedPanel : BaseDeviceNode
{
    private string _message = string.Empty;

    public string Message
    {
        get => _message;
        set
        {
            if (_message != value)
            {
                _message = value ?? string.Empty;
                OnPropertyChanged(nameof(Message));
            }
        }
    }

    public LedPanel(string id, string name, string message = "") : base("LedPanel", id, name)
    {
        _message = message ?? string.Empty;
    }

    public override string GetCurrentState()
    {
        return $"Message='{Message}'";
    }
}