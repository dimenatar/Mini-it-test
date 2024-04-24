using Environment;
using Extensions;
using Progress;

namespace Data
{
	public class LevelDataController
	{
		private LevelEventsProvider _levelEventsProvider;
		private LevelsDatasProvider _levelsDatasProvider;

		private ILevelProgress[] _levelProgresses;
		private LevelData _levelData;

		private bool _isSaved;

		public LevelDataController(LevelEventsProvider levelEventsProvider, LevelsDatasProvider levelsDatasProvider)
		{
			_levelEventsProvider = levelEventsProvider;
			_levelsDatasProvider = levelsDatasProvider;

			levelEventsProvider.Awaken += OnLevelAwaken;
			levelEventsProvider.PauseStateChanged += OnPauseStateChanged;
		}

		private void OnPauseStateChanged(bool isPause)
		{
			if (!isPause)
			{
				_isSaved = false;
			}
		}

		public void SaveLevelData()
		{
			if (!_isSaved)
			{
				foreach (var levelProgress in _levelProgresses)
				{
					if (levelProgress != null)
					{
						levelProgress.SaveProgress(_levelData);
					}
					else
					{
						this.PrintError($"level progress is null: {levelProgress}");
					}
				}
			}
			_isSaved = true;
		}

		private void OnLevelAwaken()
		{
			_isSaved = false;
		}

		public void SetLevelData(LevelData levelData)
		{
			_levelData = levelData;
		}

		public void SetLevelProgresses(params ILevelProgress[] levelProgresses)
		{
			_levelProgresses = levelProgresses;
		}

		public void LoadDataToProgresses()
		{
			foreach (var levelProgress in _levelProgresses)
			{
				levelProgress.LoadProgress(_levelData);
			}
		}
	}
}