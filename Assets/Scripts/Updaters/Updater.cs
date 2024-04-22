using System.Collections.Generic;
using UnityEngine;

public abstract class Updater : MonoBehaviour, IUpdater
{
    protected List<ITickable> _tickables = new List<ITickable>();

    public void AddTickable(ITickable tickable)
    {
        _tickables.Add(tickable);
    }

    public void InvokeTicks()
    {
        for (int i = 0; i < _tickables.Count; i++)
        {
            _tickables[i].Tick();
        }
    }

    public void RemoveTickable(ITickable tickable)
    {
        _tickables.Remove(tickable);
    }
}
