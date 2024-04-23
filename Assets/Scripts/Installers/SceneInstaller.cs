using Tiles;
using UnityEngine;
using Zenject;

public class SceneInstaller : MonoInstaller
{
    [Header("Monobehs")]
    [SerializeField] private Camera _camera;
    [SerializeField] private Dragger _dragger;
    [SerializeField] private FrameUpdater _updater;
    [SerializeField] private SecondsStepUpdater _secondsStepUpdater;
    [SerializeField] private Level _level;

    [SerializeField] private float _finalBuildingsAnimationDuration = 2f;
    [SerializeField] private float _delayToShowPanel = 1f;

    [Inject]
    private void Construct(
        InitialState initialState,
        TickInput input,
        InputEvents inputEvents,
        TileHighlighter tileHighlighter,
        InputActivator inputActivator,
        LevelDataController levelDataController)
    {
        initialState.SetSceneData(_level);
        inputEvents.SetCamera(_camera);
        tileHighlighter.SetDragger(_dragger);
        _updater.AddTickable(input);
        inputEvents.Enable();
        inputActivator.AddActivatables(_dragger);
        levelDataController.SetLevelProgresses(_level);
    }

    public override void InstallBindings()
    {
        Container.Bind<Level>().FromInstance(_level);
        Container.Bind<Dragger>().FromInstance(_dragger).AsTransient();
        Container.Bind<FrameUpdater>().FromInstance(_updater).AsTransient();
        Container.Bind<InitialState>().AsTransient();
    }
}
