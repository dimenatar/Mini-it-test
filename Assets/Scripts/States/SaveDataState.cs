using Data;
using Progress;
using Zenject;

namespace States
{
	public class SaveDataState : IState
	{
		private DataController _dataController;
		private LevelDataController _levelDataController;

		[Inject]
		public SaveDataState(DataController dataController, LevelDataController levelDataController)
		{
			_dataController = dataController;
			_levelDataController = levelDataController;
		}

		public void Enter(object data)
		{
			_levelDataController.SaveLevelData();
			_dataController.SaveProgresses();
		}

		public void Exit() { }
	}
}