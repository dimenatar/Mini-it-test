using System;

public interface IState
{
    event Action<IState> StateFinished;

    void Enter(object data);
    void Exit();
}