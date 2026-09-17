using System;
using System.Collections.Generic;
using System.Text;

namespace InterviewApp;

public class BuildingSystemRoot
{
    private readonly HashSet<string> _allDeviceIds = new();
    public Dictionary<string, GroupNode> groupNodes = new();

    public event Action<BaseDeviceNode, string>? DevicePropertyChanged;
    public event Action? StructureChanged;

    public void AddGroupNode(GroupNode groupNode)
    {
        if (groupNode == null) throw new ArgumentNullException(nameof(groupNode));
        if (groupNode.DevicesList.Count != 0) throw new Exception("Cannot add group with devices");
        if (groupNodes.ContainsKey(groupNode.GroupName)) throw new Exception($"Group '{groupNode.GroupName}' already exists.");
        
        groupNodes.Add(groupNode.GroupName, groupNode);
        OnStructureChanged();
    }

    public void RemoveGroupNode(string groupName)
    {
        if (groupName == null) throw new ArgumentNullException(nameof(groupName));
        if (!groupNodes.TryGetValue(groupName, out var groupNode))
        {
            throw new Exception($"Group '{groupName}' does not exist.");
        }

        foreach (var device in groupNode.DevicesList)
        {
            _allDeviceIds.Remove(device.Id);
            device.PropertyChanged -= Device_PropertyChanged;
        }

        groupNodes.Remove(groupName);
        OnStructureChanged();
    }

    public void AddDeviceToGroup(string groupName, BaseDeviceNode device)
    {
        if (groupName == null) throw new ArgumentNullException(nameof(groupName));
        if (device == null) throw new ArgumentNullException(nameof(device));

        if (!groupNodes.TryGetValue(groupName, out var groupNode))
        {
            throw new Exception($"Group '{groupName}' does not exist.");
        }

        if (!_allDeviceIds.Add(device.Id))
        {
            throw new Exception($"Device ID '{device.Id}' is not unique across the system.");
        }

        groupNode.AddDevice(device);
        device.PropertyChanged += Device_PropertyChanged;
        OnStructureChanged();
    }

    public void RemoveDeviceFromGroup(string groupName, string deviceId)
    {
        if (groupName == null) throw new ArgumentNullException(nameof(groupName));
        if (deviceId == null) throw new ArgumentNullException(nameof(deviceId));

        if (!groupNodes.TryGetValue(groupName, out var groupNode))
        {
            throw new Exception($"Group '{groupName}' does not exist.");
        }

        if (!groupNode.DeviceDictionary.TryGetValue(deviceId, out var device))
        {
            throw new Exception($"Device '{deviceId}' does not exist in group '{groupName}'.");
        }

        groupNode.RemoveDevice(deviceId);
        _allDeviceIds.Remove(deviceId);
        device.PropertyChanged -= Device_PropertyChanged;
        OnStructureChanged();
    }

    public void MoveDevice(string fromGroupName, string toGroupName, string deviceId)
    {
        if (fromGroupName == null) throw new ArgumentNullException(nameof(fromGroupName));
        if (toGroupName == null) throw new ArgumentNullException(nameof(toGroupName));
        if (deviceId == null) throw new ArgumentNullException(nameof(deviceId));

        if (!groupNodes.TryGetValue(fromGroupName, out var srcGroup))
        {
            throw new Exception($"Source group '{fromGroupName}' does not exist.");
        }

        if (!groupNodes.TryGetValue(toGroupName, out var destGroup))
        {
            throw new Exception($"Destination group '{toGroupName}' does not exist.");
        }

        if (!srcGroup.DeviceDictionary.TryGetValue(deviceId, out var device))
        {
            throw new Exception($"Device '{deviceId}' does not exist in group '{fromGroupName}'.");
        }

        srcGroup.RemoveDevice(deviceId);
        destGroup.AddDevice(device);
        OnStructureChanged();
    }

    public string GetTreeRepresentation()
    {
        var sb = new StringBuilder();
        sb.AppendLine("=== BUILDING SYSTEM TREE ===");
        if (groupNodes.Count == 0)
        {
            sb.AppendLine("  (No groups defined)");
        }
        else
        {
            foreach (var groupKeyValue in groupNodes)
            {
                var group = groupKeyValue.Value;
                sb.AppendLine($"Group: {group.GroupName}");
                if (group.DevicesList.Count == 0)
                {
                    sb.AppendLine("  (No devices)");
                }
                else
                {
                    foreach (var device in group.DevicesList)
                    {
                        sb.AppendLine($"  - [{device.Type}] ID: {device.Id}, Name: '{device.Name}' | State: {device.GetCurrentState()}");
                    }
                }
            }
        }
        sb.AppendLine("============================");
        return sb.ToString();
    }

    public BaseDeviceNode? FindDeviceById(string deviceId)
    {
        if (deviceId == null) return null;
        foreach (var groupKeyValue in groupNodes)
        {
            if (groupKeyValue.Value.DeviceDictionary.TryGetValue(deviceId, out var device))
            {
                return device;
            }
        }
        return null;
    }

    private void Device_PropertyChanged(BaseDeviceNode device, string propertyName)
    {
        DevicePropertyChanged?.Invoke(device, propertyName);
    }

    protected void OnStructureChanged()
    {
        StructureChanged?.Invoke();
    }
}