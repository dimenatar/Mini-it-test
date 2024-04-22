using System;
using Zenject;

public class BootstrapState : IState
{
    private DataController _dataController;
    private LevelsDatasProvider _levelDataProvider;
    private LevelLoader _levelLoader;

    [Inject]
    public BootstrapState(DataController dataController, LevelsDatasProvider levelDataProvider, LevelLoader levelLoader)
    {
        _dataController = dataController;
        _levelDataProvider = levelDataProvider;
        _levelLoader = levelLoader;
    }

    public event Action<IState> StateFinished;

    public async void Enter(object data)
    {
        _dataController.SetSavingState(true);
        _dataController.LoadProgresses();

        await _levelLoader.LoadSceneAsync(SceneType.Main);
    }

    public void Exit() { }
}