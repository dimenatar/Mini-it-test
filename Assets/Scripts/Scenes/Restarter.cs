using Progress;

namespace Scenes
{
	public class Restarter
	{
		private DataController _dataController;
		private LevelLoader _levelLoader;

		public Restarter(DataController dataController, LevelLoader levelLoader)
		{
			_dataController = dataController;
			_levelLoader = levelLoader;
		}

		public async void Restart()
		{
			_dataController.ClearSaveData();
			await _levelLoader.ReloadSceneAsync();
		}
	}
}