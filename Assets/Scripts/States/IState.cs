namespace States
{
	public interface IState
	{
		void Enter(object data);
		void Exit();
	}
}