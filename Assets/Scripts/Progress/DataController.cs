using Extensions;
using System.Collections.Generic;
using System.Linq;

namespace Progress
{
	public class DataController
	{
		private List<IProgress> _progresses;
		private DictionaryProgressManager _progressManager;

		private bool _isSavingData = true;

		public DataController(DictionaryProgressManager dictionaryProgressManager)
		{
			_progresses = new List<IProgress>();
			_progressManager = dictionaryProgressManager;
		}

		public void SetSavingState(bool isSavingData)
		{
			_isSavingData = isSavingData;
		}

		public void ClearSaveData()
		{
			SetSavingState(false);
			_progressManager.WipeData();
			_progressManager.UpdateUserData();
			for (int i = 0; i < _progresses.Count; i++)
			{
				if (_progresses[i] == null)
				{
					_progresses.Remove(_progresses[i]);
					i--;
				}
				else
				{
					_progresses[i].ClearProgress();
				}
			}
		}

		public void AddProgress(IProgress progress)
		{
			_progresses.Add(progress);
		}

		public void LoadProgresses()
		{
			var progresses = new List<IProgress>(_progresses.Where(progress => !progress.IsUnityNull()));
			_progresses = progresses;
			for (int i = 0; i < _progresses.Count; i++)
			{
				if (_progresses[i].IsUnityNull())
				{
					_progresses.RemoveAt(i);
					i--;
				}
				else
				{
					if (!_progresses[i].IsLoaded)
					{
						_progresses[i].LoadProgress(_progressManager);
					}
				}
			}
		}

		public void SaveProgresses()
		{
			if (_isSavingData)
			{
				for (int i = 0; i < _progresses.Count; i++)
				{
					if (_progresses[i] == null)
					{
						_progresses.Remove(_progresses[i]);
						i--;
					}
					else
					{
						_progresses[i].SaveProgress(_progressManager);
					}
				}
				_progressManager.UpdateUserData();
			}
		}
	}
}