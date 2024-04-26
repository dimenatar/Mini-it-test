using UnityEngine;

namespace Extensions
{
	public class Vibrations
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

		private long _vibrationsDuration;

		public Vibrations(long vibrationsDuration)
		{
			_vibrationsDuration = vibrationsDuration;

#if !UNITY_EDITOR && UNITY_ANDROID
        _player = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
        _activity = _player.GetStatic<AndroidJavaObject>("currentActivity");
        _version = new AndroidJavaClass("android.os.Build$VERSION");
        _vibrationEffect = new AndroidJavaClass("android.os.VibrationEffect");
        _vibrator = _activity.Call<AndroidJavaObject>("getSystemService", "vibrator");
#endif
		}

		public int SDK_version => _version.GetStatic<int>("SDK_INT");

		public void Vibrate(long millisecond, int amplitude)
		{
#if !UNITY_EDITOR && UNITY_ANDROID

        if (SDK_version >= 26)
        {
            AndroidJavaObject vibrationEffectObj = _vibrationEffect.CallStatic<AndroidJavaObject>("createOneShot", millisecond, amplitude);
            _vibrator.Call("vibrate", vibrationEffectObj);
        }
        else
        {
            _vibrator.Call("vibrate", millisecond);
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

		public void VibrateSoft()
		{
			Vibrate(_vibrationsDuration, SOFT_AMPLITUDE);
		}

		public void VibrateMedium(long milliseconds)
		{
			Vibrate(milliseconds, MEDIUM_AMPLITUDE);
		}

		public void VibrateMedium()
		{
			Vibrate(_vibrationsDuration, MEDIUM_AMPLITUDE);
		}

		public void VibrateHard(long milliseconds)
		{
			Vibrate(milliseconds, HARD_AMPLITUDE);
		}

		public void VibrateHard()
		{
			Vibrate(_vibrationsDuration, HARD_AMPLITUDE);
		}

		public void VibrateMax(long milliseconds)
		{
			Vibrate(milliseconds, MAX_AMPLITUDE);
		}

		public void VibrateMax()
		{
			Vibrate(_vibrationsDuration, MAX_AMPLITUDE);
		}
	}
}