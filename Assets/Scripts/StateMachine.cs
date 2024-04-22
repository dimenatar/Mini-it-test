using System;
using System.Collections.Generic;
using System.Linq;
using Zenject;

public class StateMachine
{
    private Dictionary<Type, IState> _states;

    private IState _currentState;

    [Inject]
    public StateMachine()
    {
        _states = new Dictionary<Type, IState>();
    }

    public void SetState<T>(object args = null) where T : IState
    {
        var destination = _states.First(state => state.Key.Equals(typeof(T))).Value;
        SwitchState(_currentState, destination, args);
    }

    public void AddState<T>(T state) where T : IState
    {
        if (!_states.ContainsValue(state))
        {
            _states.Add(typeof(T), state);
        }
    }

    public void ReplaceState<T>(T state) where T : IState
    {
        _states[typeof(T)] = state;
    }

    private void SwitchState(IState current, IState to, object args)
    {
        if (current != null) current.Exit();
        _currentState = to;
        _currentState.Enter(args);
    }
}
