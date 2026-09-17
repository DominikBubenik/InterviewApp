using System;
using InterviewApp;

namespace InterviewApp.Devices;

public class CardReader : BaseDeviceNode
{
    private string _accessCardNumber = "0000000000000000";

    public string AccessCardNumber
    {
        get => _accessCardNumber;
        set => SetAccessCardNumber(value);
    }

    public CardReader(string id, string name, string initialValue = "") : base("CardReader", id, name)
    {
        if (string.IsNullOrEmpty(initialValue))
        {
            _accessCardNumber = "0000000000000000";
        }
        else
        {
            SetAccessCardNumber(initialValue);
        }
    }

    /// <summary>
    /// Validates, transforms, and sets the access card number.
    /// </summary>
    public void SetAccessCardNumber(string value)
    {
        if (value == null)
        {
            throw new ArgumentNullException(nameof(value), "Access card number cannot be null.");
        }

        if (value.Length % 2 != 0)
        {
            throw new ArgumentException("Access card number length must be even.");
        }

        if (value.Length > 16)
        {
            throw new ArgumentException("Access card number length must not exceed 16 characters.");
        }

        foreach (char c in value)
        {
            if (!((c >= '0' && c <= '9') || (c >= 'a' && c <= 'f') || (c >= 'A' && c <= 'F')))
            {
                throw new ArgumentException("Access card number must contain only hexadecimal digits.");
            }
        }

        string transformed = ReverseBytesAndPad(value);
        if (_accessCardNumber != transformed)
        {
            _accessCardNumber = transformed;
            OnPropertyChanged(nameof(AccessCardNumber));
        }
    }

    public static string ReverseBytesAndPad(string value)
    {
        if (value == null) return string.Empty;
        
        int byteCount = value.Length / 2;
        string[] bytes = new string[byteCount];
        for (int i = 0; i < byteCount; i++)
        {
            bytes[i] = value.Substring(i * 2, 2);
        }

        Array.Reverse(bytes);
        string reversed = string.Concat(bytes);

        if (reversed.Length < 16)
        {
            reversed = reversed.PadLeft(16, '0');
        }

        return reversed;
    }

    public override string GetCurrentState()
    {
        return $"AccessCardNumber='{AccessCardNumber}'";
    }
}