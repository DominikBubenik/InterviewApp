namespace InterviewApp;

public abstract class BaseDeviceNode
{
    public string Type { get; }
    public string Id { get; }
    
    private string _name;
    public string Name
    {
        get => _name;
        set
        {
            if (_name != value)
            {
                _name = value;
                OnPropertyChanged(nameof(Name));
            }
        }
    }

    public event System.Action<BaseDeviceNode, string>? PropertyChanged;

    protected BaseDeviceNode(string type, string id, string name)
    {
        Type = type ?? throw new System.ArgumentNullException(nameof(type));
        Id = id ?? throw new System.ArgumentNullException(nameof(id));
        _name = name ?? throw new System.ArgumentNullException(nameof(name));
    }

    protected void OnPropertyChanged(string propertyName)
    {
        PropertyChanged?.Invoke(this, propertyName);
    }

    public abstract string GetCurrentState();
}