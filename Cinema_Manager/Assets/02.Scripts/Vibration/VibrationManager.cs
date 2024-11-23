using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VibrationManager : MonoSingleton<VibrationManager>
{
    private bool _isVibrationOn = true;

    public void Vibrate(long milliseconds)
    {
        if (false == _isVibrationOn) return;

        Vibration.Vibrate(milliseconds);
    }

    public void Vibrate(long milliseconds, int amplitude)
    {
        if (false == _isVibrationOn) return;

        Vibration.Vibrate(milliseconds, amplitude);
    }

    private void Vibrate(long[] pattern, int repeat, int[] amplitudes)
    {
        if (false == _isVibrationOn) return;

        Vibration.Vibrate(pattern, repeat, amplitudes);
    }

    public void VibrationSet(bool isVibrationOn)
    {
        _isVibrationOn = isVibrationOn;
    }
}
