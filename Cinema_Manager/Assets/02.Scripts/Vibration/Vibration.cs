using UnityEngine;

public static class Vibration
{
#if UNITY_ANDROID && !UNITY_EDITOR
    private static AndroidJavaClass unityPlayer = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
    private static AndroidJavaObject currentActivity = unityPlayer.GetStatic<AndroidJavaObject>("currentActivity");
    private static AndroidJavaObject vibrator = currentActivity.Call<AndroidJavaObject>("getSystemService", "vibrator");
    private static AndroidJavaClass vibrationEffectClass = new AndroidJavaClass("android.os.VibrationEffect");
#else
    private static AndroidJavaClass unityPlayer;
    private static AndroidJavaObject currentActivity;
    private static AndroidJavaObject vibrator;
    private static AndroidJavaClass vibrationEffectClass;
#endif

    public static void Vibrate()
    {
        if (isAndroid())
            vibrator.Call("vibrate");
        else
            Handheld.Vibrate();
    }

    public static void Vibrate(long milliseconds)
    {
        if (isAndroid())
        {
#if UNITY_ANDROID && !UNITY_EDITOR
            // Android 8.0 이상에서는 VibrationEffect 사용
            if (IsOreoOrAbove())
            {
                AndroidJavaObject vibrationEffect = vibrationEffectClass.CallStatic<AndroidJavaObject>(
                    "createOneShot",
                    milliseconds,
                    VibrationEffectDefaultAmplitude()
                );
                vibrator.Call("vibrate", vibrationEffect);
            }
            else
            {
                vibrator.Call("vibrate", milliseconds);
            }
#else
            Handheld.Vibrate();
#endif
        }
    }

    public static void Vibrate(long milliseconds, int amplitude)
    {
        if (isAndroid())
        {
#if UNITY_ANDROID && !UNITY_EDITOR
            if (IsOreoOrAbove())
            {
                AndroidJavaObject vibrationEffect = vibrationEffectClass.CallStatic<AndroidJavaObject>(
                    "createOneShot",
                    milliseconds,
                    Mathf.Clamp(amplitude, 1, 255) // 진동 세기 범위는 1~255
                );
                vibrator.Call("vibrate", vibrationEffect);
            }
            else
            {
                // Android 8.0 미만에서는 진동 세기를 조절할 수 없으므로 기본 진동 실행
                vibrator.Call("vibrate", milliseconds);
            }
#else
            Handheld.Vibrate();
#endif
        }
    }

    public static void Vibrate(long[] pattern, int repeat, int[] amplitudes)
    {
        if (isAndroid())
        {
#if UNITY_ANDROID && !UNITY_EDITOR
            if (IsOreoOrAbove())
            {
                AndroidJavaObject vibrationEffect = vibrationEffectClass.CallStatic<AndroidJavaObject>(
                    "createWaveform",
                    pattern,
                    amplitudes,
                    repeat
                );
                vibrator.Call("vibrate", vibrationEffect);
            }
            else
            {
                vibrator.Call("vibrate", pattern, repeat);
            }
#else
            Handheld.Vibrate();
#endif
        }
    }

    public static void Cancel()
    {
        if (isAndroid())
            vibrator.Call("cancel");
    }

    private static bool isAndroid()
    {
#if UNITY_ANDROID && !UNITY_EDITOR
        return true;
#else
        return false;
#endif
    }

#if UNITY_ANDROID && !UNITY_EDITOR
    private static bool IsOreoOrAbove()
    {
        AndroidJavaClass buildVersion = new AndroidJavaClass("android.os.Build$VERSION");
        int sdkInt = buildVersion.GetStatic<int>("SDK_INT");
        return sdkInt >= 26; // Android 8.0 (API 26)
    }

    private static int VibrationEffectDefaultAmplitude()
    {
        // 기본 진동 세기
        return vibrationEffectClass.GetStatic<int>("DEFAULT_AMPLITUDE");
    }
#endif
}
