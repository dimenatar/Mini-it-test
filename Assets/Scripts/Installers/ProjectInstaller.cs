using Environment;
using Fruits;
using Merdge;
using Scriptables;
using Tiles;
using UnityEngine;
using Zenject;

public class ProjectInstaller : MonoInstaller
{
    [SerializeField] private FruitName _initialFruitName;

    #region Configs
    [Header("Configs")]
    [SerializeField] private FruitEvolutionConfig _fruitEvolutionConfig;
    [SerializeField] private ScenesConfig _scenesConfig;
    [SerializeField] private TilesColorsConfig _tileColorsConfig;
    #endregion

    private States _states;

    [SerializeField] private float _mouseScrollModifier = 2f;
    [SerializeField] private int _finalBuildingIncomeDelay = 5;
    [SerializeField] private float _delayToMerdge = 1f;
    [SerializeField] private ulong _startMoneyAmount;
    [SerializeField] private ulong _startGemsAmount;
    [SerializeField] private float _finalBuildingCameraSpeed = 10f;
    [SerializeField] private float _buildingTransitionToUIEndScale = 0.2f;
    [SerializeField] private int _tutorialIndexToEnableBonusBoxSpawner;

    public override void InstallBindings()
    {
        DictionaryProgressManager dictionaryProgressManager = new DictionaryProgressManager();
        DataController dataController = new DataController(dictionaryProgressManager);
        LevelEventsProvider levelEventsProvider = new LevelEventsProvider();
  
        FruitSpawner fruitSpawner = new FruitSpawner();

        LevelProvider levelProvider = new LevelProvider();
        InputActivator inputActivator = new InputActivator();
        Merdger merdger = new Merdger(_fruitEvolutionConfig, _delayToMerdge, fruitSpawner, inputActivator, levelProvider);

        bool isEditor = Application.isEditor;
        IInput input;
        if (isEditor)
        {
            input = new MouseInput(_mouseScrollModifier);
        }
        else
        {
            input = new MobileInput();
        }

        InputEvents inputEvents = new InputEvents(input);
       
        TileHighlighter tileHighlighter = new TileHighlighter(_tileColorsConfig, merdger);

        //var initialState = new InitialState(_island, buildingCreator, new List<ITransition>(), levelProgress, _buildingCenter, dataController, bonusBoxOpener, bonusBoxFactory, modifierHolder, buildingPassiveIncome, bonusBoxSpawnerController, _shopPanel, shopBoxController);
        LevelsDatasProvider levelsDatasProvider = new LevelsDatasProvider(dataController, _initialFruitName);
        LevelDataController levelDataController = new LevelDataController(levelEventsProvider, levelsDatasProvider);
        LevelLoader levelLoader = new LevelLoader(_scenesConfig);

        inputActivator.AddActivatables(input, inputEvents);

        StateMachine stateMachine = new StateMachine();
        var saveState = new SaveDataState(dataController, levelDataController);
        BootstrapState bootstrapState = new BootstrapState(dataController, levelsDatasProvider, levelLoader);
        InitialState initialState = new InitialState(levelsDatasProvider, dataController, levelDataController, fruitSpawner);

        _states = FindObjectOfType<States>();

        stateMachine.AddState(saveState);
        stateMachine.AddState(bootstrapState);
        stateMachine.AddState(initialState);

        Container.Bind<ScenesConfig>().FromInstance(_scenesConfig).AsSingle();
        Container.BindInterfacesAndSelfTo<TickInput>().FromInstance(input).AsSingle().NonLazy();
        Container.Bind<DataController>().FromInstance(dataController).AsSingle();
        Container.Bind<TileHighlighter>().FromInstance(tileHighlighter).AsSingle();
       
        Container.Bind<DictionaryProgressManager>().FromInstance(dictionaryProgressManager).AsSingle();
        Container.Bind<InputEvents>().FromInstance(inputEvents);
        Container.Bind<StateMachine>().FromInstance(stateMachine).AsSingle();
        Container.Bind<InputActivator>().FromInstance(inputActivator).AsSingle();
        Container.Bind<FruitSpawner>().FromInstance(fruitSpawner).AsSingle();
      
        Container.Bind<Merdger>().FromInstance(merdger).AsSingle();
        Container.Bind<LevelsDatasProvider>().FromInstance(levelsDatasProvider).AsSingle();
        Container.Bind<LevelLoader>().FromInstance(levelLoader).AsSingle();
        Container.Bind<InitialState>().FromInstance(initialState).AsSingle();
        Container.Bind<LevelProvider>().FromInstance(levelProvider).AsSingle();

        Container.Bind<LevelEventsProvider>().FromInstance(levelEventsProvider).AsSingle();

        Container.Bind<LevelDataController>().FromInstance(levelDataController).AsSingle();
        Container.Bind<States>().FromInstance(_states).AsSingle();
    }
}