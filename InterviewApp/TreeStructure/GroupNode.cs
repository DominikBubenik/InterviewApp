namespace InterviewApp;

public class GroupNode
{
    public string GroupName { get; private set; }
    
    public List<BaseDeviceNode> DevicesList { get; private set; }
    public Dictionary<string, BaseDeviceNode> DeviceDictionary { get; private set; }
    
    public GroupNode(string groupName)
    {
        GroupName = groupName ?? throw new System.ArgumentNullException(nameof(groupName));
        DevicesList = new List<BaseDeviceNode>();
        DeviceDictionary = new Dictionary<string, BaseDeviceNode>();
    }
    
    public void AddDevice(BaseDeviceNode deviceNode)
    {
        if (deviceNode == null) throw new System.ArgumentNullException(nameof(deviceNode));
        DevicesList.Add(deviceNode);
        DeviceDictionary.Add(deviceNode.Id, deviceNode);
    }

    public bool RemoveDevice(string id)
    {
        if (id == null) throw new System.ArgumentNullException(nameof(id));
        if (DeviceDictionary.TryGetValue(id, out var device))
        {
            DeviceDictionary.Remove(id);
            DevicesList.Remove(device);
            return true;
        }
        return false;
    }
}