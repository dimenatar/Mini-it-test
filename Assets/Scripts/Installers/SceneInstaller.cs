using Data;
using Environment;
using Fruits;
using States;
using Tiles;
using UnityEngine;
using Updaters;
using UserInput;
using Zenject;

namespace Installers
{
	public class SceneInstaller : MonoInstaller
	{
		[Header("Monobehs")]
		[SerializeField] private Camera _camera;
		[SerializeField] private Dragger _dragger;
		[SerializeField] private FrameUpdater _updater;
		[SerializeField] private SecondsStepUpdater _secondsStepUpdater;
		[SerializeField] private Level _level;
		[SerializeField] private InitialFruitSpawner _initialFruitSpawner;

		[Inject]
		private void Construct(
			InitialState initialState,
			TickInput input,
			InputEvents inputEvents,
			TileHighlighter tileHighlighter,
			InputActivator inputActivator,
			LevelDataController levelDataController)
		{
			initialState.SetSceneData(_level);
			inputEvents.SetCamera(_camera);
			tileHighlighter.SetDragger(_dragger);
			_updater.AddTickable(input);
			_updater.AddTickable(_initialFruitSpawner);
			_updater.AddTickable(_dragger);
			inputEvents.Enable();
			inputActivator.AddActivatables(_dragger);
			levelDataController.SetLevelProgresses(_level);
		}

		public override void InstallBindings()
		{
			Container.Bind<Level>().FromInstance(_level);
			Container.Bind<Dragger>().FromInstance(_dragger).AsTransient();
			Container.Bind<FrameUpdater>().FromInstance(_updater).AsTransient();
			Container.Bind<InitialFruitSpawner>().FromInstance(_initialFruitSpawner);
			Container.Bind<InitialState>().AsTransient();
		}
	}
}