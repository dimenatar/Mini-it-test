using Environment;
using Scriptables;
using UnityEngine;
using UnityEngine.SceneManagement;
using Zenject;

namespace States
{
	public class StatesController : MonoBehaviour
	{
		[SerializeField] private ScenesConfig _scenesConfig;

		private StateMachine _stateMachine;
		private LevelEventsProvider _levelEventsProvider;

		[Inject]
		private void Construct(StateMachine stateMachine, LevelEventsProvider levelEventsProvider)
		{
			_stateMachine = stateMachine;
			_levelEventsProvider = levelEventsProvider;

			SceneManager.sceneLoaded += OnSceneLoaded;
			Application.quitting += OnApplicationQuitting;
			_levelEventsProvider.PauseStateChanged += OnPauseStateChanged;
			_levelEventsProvider.LevelAboutToUnload += OnLevelAboutToUnload;
		}

		private void Awake()
		{
			DontDestroyOnLoad(this);
		}

		private void Start()
		{
			_stateMachine.SetState<BootstrapState>();
		}

		private void OnDisable()
		{
			SceneManager.sceneLoaded -= OnSceneLoaded;
			Application.quitting -= OnApplicationQuitting;
			_levelEventsProvider.PauseStateChanged -= OnPauseStateChanged;
			_levelEventsProvider.LevelAboutToUnload -= OnLevelAboutToUnload;
		}

		private void OnLevelAboutToUnload()
		{
			_stateMachine.SetState<SaveDataState>();
		}

		private void OnApplicationQuitting()
		{
			_stateMachine.SetState<SaveDataState>();
		}

		private void OnPauseStateChanged(bool state)
		{
			if (state)
			{
				_stateMachine.SetState<SaveDataState>();
			}
		}

		private void OnSceneLoaded(Scene scene, LoadSceneMode loadMode)
		{
			if (_scenesConfig.Scenes.ContainsValue(scene.name))
			{
				_stateMachine.SetState<InitialState>();
			}
		}
	}
}