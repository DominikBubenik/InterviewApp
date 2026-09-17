using System;
using InterviewApp;

namespace InterviewApp.Devices;

public enum SpeakerSound
{
    None,
    Music,
    Alarm
}

public class Speaker : BaseDeviceNode
{
    private SpeakerSound _sound;
    private double _volume;

    public SpeakerSound Sound
    {
        get => _sound;
        set => SetSound(value);
    }

    public double Volume
    {
        get => _volume;
        set => SetVolume(value);
    }

    public Speaker(string id, string name, SpeakerSound initialSound = SpeakerSound.None, double initialVolume = 0.5) : base("Speaker", id, name)
    {
        SetSound(initialSound);
        SetVolume(initialVolume);
    }

    /// <summary>
    /// Sets the speaker sound and notifies listeners if the value has changed.
    /// </summary>
    public void SetSound(SpeakerSound value)
    {
        if (_sound != value)
        {
            _sound = value;
            OnPropertyChanged(nameof(Sound));
        }
    }

    /// <summary>
    /// Sets the speaker volume and notifies listeners if the value has changed.
    /// </summary>
    public void SetVolume(double value)
    {
        if (System.Math.Abs(_volume - value) > double.Epsilon)
        {
            _volume = value;
            OnPropertyChanged(nameof(Volume));
        }
    }

    public override string GetCurrentState()
    {
        return $"Sound={Sound}, Volume={Volume:F2}";
    }
}