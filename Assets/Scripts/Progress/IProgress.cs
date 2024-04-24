namespace Progress
{
	public interface IProgress
	{
		public bool IsLoaded { get; }

		void ClearProgress();
		void LoadProgress(DictionaryProgressManager dictionaryProgressManager);
		void SaveProgress(DictionaryProgressManager dictionaryProgressManager);
	}
}