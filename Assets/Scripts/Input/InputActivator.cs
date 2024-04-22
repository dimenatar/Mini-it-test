using System.Collections.Generic;
using System.Linq;

public class InputActivator
{
    private List<IActivatable> _activatables;

    public InputActivator(params IActivatable[] activatables)
    {
        _activatables = activatables.ToList();
    }

    public InputActivator() 
    {
        _activatables = new List<IActivatable>();
    }

    public void AddActivatables(params IActivatable[] activatables)
    {
        _activatables.AddRange(activatables);
    }

    public void AddActivatable(IActivatable activatable)
    {
        _activatables.Add(activatable);
    }

    public void EnableInput()
    {
        for (int i = 0; i < _activatables.Count; i++)
        {
            if (_activatables[i] == null)
            {
                _activatables.RemoveAt(i);
                i--;
            }
            else
            {
                _activatables[i].Enable();
            }
        }
    }

    public void DisableInput()
    {
        for (int i = 0; i < _activatables.Count; i++)
        {
            if (_activatables[i] == null)
            {
                _activatables.RemoveAt(i);
                i--;
            }
            else
            {
                _activatables[i].Disable();
            }
        }
    }
}
