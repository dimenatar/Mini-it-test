using Data;

namespace Progress
{
	public interface ILevelProgress
	{
		public void LoadProgress(LevelData levelData);
		public void SaveProgress(LevelData levelData);
	}
}