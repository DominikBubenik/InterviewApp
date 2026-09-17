using System;
using InterviewApp;

namespace InterviewApp.Devices;

[Flags]
public enum DoorState
{
    None = 0,
    Locked = 1 << 0,
    Open = 1 << 1,
    OpenForTooLong = 1 << 2,
    OpenedForcibly = 1 << 3
}

public class Door : BaseDeviceNode
{
    private DoorState _state;

    public DoorState State
    {
        get => _state;
        set => SetState(value);
    }

    public Door(string id, string name, DoorState initialState = DoorState.None) : base("Door", id, name)
    {
        _state = initialState;
    }

    /// <summary>
    /// Sets the door state and notifies listeners if the value has changed.
    /// </summary>
    public void SetState(DoorState value)
    {
        if (_state != value)
        {
            _state = value;
            OnPropertyChanged(nameof(State));
        }
    }

    private bool GetBit(DoorState flag)
    {
        return (_state & flag) == flag;
    }

    private void SetBit(DoorState flag, bool value)
    {
        DoorState newState = value ? (_state | flag) : (_state & ~flag);
        SetState(newState);
    }

    public bool Locked
    {
        get => GetBit(DoorState.Locked);
        set => SetBit(DoorState.Locked, value);
    }

    public bool Open
    {
        get => GetBit(DoorState.Open);
        set => SetBit(DoorState.Open, value);
    }

    public bool OpenForTooLong
    {
        get => GetBit(DoorState.OpenForTooLong);
        set => SetBit(DoorState.OpenForTooLong, value);
    }

    public bool OpenedForcibly
    {
        get => GetBit(DoorState.OpenedForcibly);
        set => SetBit(DoorState.OpenedForcibly, value);
    }

    public override string GetCurrentState()
    {
        return $"State={_state} (Locked={Locked}, Open={Open}, OpenForTooLong={OpenForTooLong}, OpenedForcibly={OpenedForcibly})";
    }
}