namespace Updaters
{
	public interface IUpdater
	{
		public void InvokeTicks();
		public void AddTickable(ITickable tickable);
		public void RemoveTickable(ITickable tickable);
	}
}