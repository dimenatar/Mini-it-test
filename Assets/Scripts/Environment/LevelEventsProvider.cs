using System;
using UnityEngine;

namespace Environment
{
	public class LevelEventsProvider
	{
		public LevelEventsProvider()
		{
			Application.quitting += OnAppQuitting;
		}

		public event Action Awaken;
		public event Action Started;
		public event Action TransitionCompleted;
		public event Action LevelAboutToUnload;
		public event Action Disabled;
		public event Action Destroyed;
		public event Action AppClosing;

		public event Action<bool> PauseStateChanged;

		public void SetLevelEvents(LevelEvents levelEvents)
		{
			levelEvents.Awaken += OnAwaken;
			levelEvents.Started += OnStarted;
			levelEvents.Disabled += OnDisabled;
			levelEvents.Destroyed += OnDestroyed;
			levelEvents.PauseStateChanged += OnPauseStateChanged;
		}

		private void OnPauseStateChanged(bool state)
		{
			PauseStateChanged?.Invoke(state);
		}

		public void InvokeLevelAboutToUnload()
		{
			MonobehaviourExtensions.StopAllTasks();
			LevelAboutToUnload?.Invoke();
		}

		public void InvokeTransitionCompleted()
		{
			TransitionCompleted?.Invoke();
		}

		private void OnDisabled()
		{
			Disabled?.Invoke();
		}

		private void OnStarted()
		{
			Started?.Invoke();
		}

		private void OnDestroyed()
		{
			Destroyed?.Invoke();
		}

		private void OnAwaken()
		{
			Awaken?.Invoke();
		}

		private void OnAppQuitting()
		{
			AppClosing?.Invoke();
		}
	}
}