using Data;
using Progress;
using Scenes;
using Zenject;

namespace States
{
	public class BootstrapState : IState
	{
		private DataController _dataController;
		private LevelsDatasProvider _levelDataProvider;
		private LevelLoader _levelLoader;

		[Inject]
		public BootstrapState(DataController dataController, LevelsDatasProvider levelDataProvider, LevelLoader levelLoader)
		{
			_dataController = dataController;
			_levelDataProvider = levelDataProvider;
			_levelLoader = levelLoader;
		}

		public async void Enter(object data)
		{
			_dataController.SetSavingState(true);
			_dataController.LoadProgresses();

			await _levelLoader.LoadSceneAsync(SceneType.Main);
		}

		public void Exit() { }
	}
}