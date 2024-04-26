using UnityEngine;

public class VibrationsController : MonoBehaviour
{
    private const byte SOFT_AMPLITUDE = 50;
    private const byte MEDIUM_AMPLITUDE = 120;
    private const byte HARD_AMPLITUDE = 200;
    private const byte MAX_AMPLITUDE = 255;

    private AndroidJavaObject _activity;
    private AndroidJavaObject _vibrator;
    private AndroidJavaClass _player;
    private AndroidJavaClass _version;
    private AndroidJavaClass _vibrationEffect;

    public int SDK_version => _version.GetStatic<int>("SDK_INT");

    public static VibrationsController Instance 
    {   
        get
        {
            if (_instance == null)
            {
                SetupInstance();
            }
            if (!_isInitialised)
            {
                Init();
            }
            return _instance;
        }
        private set => _instance = value;
    }

    private static VibrationsController _instance;
    private static bool _isInitialised;

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
        }
        DontDestroyOnLoad(gameObject);

#if !UNITY_EDITOR && UNITY_ANDROID

        if (!_isInitialised)
        Init();
#endif
    }

    public void Vibrate(long millisecond, int amplitude)
    {
#if !UNITY_EDITOR && UNITY_ANDROID

        if (_instance.SDK_version >= 26)
        {
            AndroidJavaObject vibrationEffectObj = _instance._vibrationEffect.CallStatic<AndroidJavaObject>("createOneShot", millisecond, amplitude);
            _instance._vibrator.Call("vibrate", vibrationEffectObj);
        }
        else
        {
            _instance._vibrator.Call("vibrate", millisecond);
        }
#endif
    }

    public void VibrateWithPattern(long[] pattern, int repeat)
    {
#if !UNITY_EDITOR && UNITY_ANDROID
        if (_vibrator.Call<bool>("hasVibrator"))
        {
            if (SDK_version >= 26)
            {
                AndroidJavaObject vibrationEffectObj = _vibrationEffect.CallStatic<AndroidJavaObject>("createWaveform", pattern, repeat);
                _vibrator.Call("vibrate", vibrationEffectObj);
            }
            else
            {
                _vibrator.Call("vibrate", pattern, repeat);
            }
        }
#endif
    }

    public void VibrateSoft(long milliseconds)
    {
        Vibrate(milliseconds, SOFT_AMPLITUDE);
    }

    public void VibrateMedium(long milliseconds)
    {
        Vibrate(milliseconds, MEDIUM_AMPLITUDE);
    }

    public void VibrateHard(long milliseconds)
    {
        Vibrate(milliseconds, HARD_AMPLITUDE);
    }

    public void VibrateMax(long milliseconds)
    {
        Vibrate(milliseconds, MAX_AMPLITUDE);
    }

    private static void SetupInstance()
    {
        var found = Object.FindObjectOfType<VibrationsController>();
        if (found == null)
        {
            var empty = new GameObject("VibrationsController");
            var vibrationsController = empty.AddComponent<VibrationsController>();
            _instance = vibrationsController;
            DontDestroyOnLoad(empty);
        }
        else
        {
            _instance = found;
        }
    }

    private static void Init()
    {
#if !UNITY_EDITOR && UNITY_ANDROID
        _instance._player = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
        _instance._activity = _instance._player.GetStatic<AndroidJavaObject>("currentActivity");
        _instance._version = new AndroidJavaClass("android.os.Build$VERSION");
        _instance._vibrationEffect = new AndroidJavaClass("android.os.VibrationEffect");
        _instance._vibrator = _instance._activity.Call<AndroidJavaObject>("getSystemService", "vibrator");
#endif
        _isInitialised = true;
    }
}
