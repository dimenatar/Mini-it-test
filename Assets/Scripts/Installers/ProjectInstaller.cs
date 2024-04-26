using Data;
using Environment;
using Extensions;
using Fruits;
using Merge;
using Particles;
using Progress;
using Scenes;
using Scriptables;
using States;
using Tiles;
using UnityEngine;
using UserInput;
using Zenject;

namespace Installers
{
	public class ProjectInstaller : MonoInstaller
	{
		[SerializeField] private FruitName _initialFruitName;

		#region Configs
		[Header("Configs")]
		[SerializeField] private FruitEvolutionConfig _fruitEvolutionConfig;
		[SerializeField] private ScenesConfig _scenesConfig;
		[SerializeField] private TilesColorsConfig _tileColorsConfig;
		[SerializeField] private FruitModels _fruitModels;
		[SerializeField] private FruitDataBundle _fruitDataBundle;
		[SerializeField] private ParticlesConfig _particlesConfig;
		#endregion

		private StatesController _states;

		[SerializeField] private float _delayToMerdge = 1f;
		[SerializeField] private long _vibrationsDurationMS = 100;

		public override void InstallBindings()
		{
			DictionaryProgressManager dictionaryProgressManager = new DictionaryProgressManager();
			DataController dataController = new DataController(dictionaryProgressManager);
			LevelEventsProvider levelEventsProvider = new LevelEventsProvider();
			Vibrations vibrations = new Vibrations(_vibrationsDurationMS);

			FruitSpawner fruitSpawner = new FruitSpawner(_fruitModels, _fruitDataBundle);

			LevelProvider levelProvider = new LevelProvider();
			InputActivator inputActivator = new InputActivator();
			Merger merger = new Merger(_fruitEvolutionConfig, _delayToMerdge, fruitSpawner, inputActivator, levelProvider);
			ParticlePlayer particlePlayer = new ParticlePlayer(merger, _particlesConfig);

			bool isEditor = Application.isEditor;
			IInput input;
			if (isEditor)
			{
				input = new MouseInput();
			}
			else
			{
				input = new MobileInput();
			}

			InputEvents inputEvents = new InputEvents(input);

			TileHighlighter tileHighlighter = new TileHighlighter(_tileColorsConfig, merger);

			//var initialState = new InitialState(_island, buildingCreator, new List<ITransition>(), levelProgress, _buildingCenter, dataController, bonusBoxOpener, bonusBoxFactory, modifierHolder, buildingPassiveIncome, bonusBoxSpawnerController, _shopPanel, shopBoxController);
			LevelsDatasProvider levelsDatasProvider = new LevelsDatasProvider(dataController, _initialFruitName);
			LevelDataController levelDataController = new LevelDataController(levelEventsProvider, levelsDatasProvider);
			LevelLoader levelLoader = new LevelLoader(_scenesConfig);
			Restarter restarter = new Restarter(dataController, levelLoader);

			inputActivator.AddActivatables(input, inputEvents);

			StateMachine stateMachine = new StateMachine();
			var saveState = new SaveDataState(dataController, levelDataController);
			BootstrapState bootstrapState = new BootstrapState(dataController, levelsDatasProvider, levelLoader);
			InitialState initialState = new InitialState(levelsDatasProvider, dataController, levelDataController, fruitSpawner);

			_states = FindObjectOfType<StatesController>();

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

			Container.Bind<Merger>().FromInstance(merger).AsSingle();
			Container.Bind<LevelsDatasProvider>().FromInstance(levelsDatasProvider).AsSingle();
			Container.Bind<LevelLoader>().FromInstance(levelLoader).AsSingle();
			Container.Bind<InitialState>().FromInstance(initialState).AsSingle();
			Container.Bind<LevelProvider>().FromInstance(levelProvider).AsSingle();

			Container.Bind<LevelEventsProvider>().FromInstance(levelEventsProvider).AsSingle();

			Container.Bind<LevelDataController>().FromInstance(levelDataController).AsSingle();
			Container.Bind<StatesController>().FromInstance(_states).AsSingle();
			Container.Bind<FruitName>().FromInstance(_initialFruitName);
			Container.Bind<ParticlePlayer>().FromInstance(particlePlayer);
			Container.Bind<Restarter>().FromInstance(restarter);
			Container.Bind<Vibrations>().FromInstance(vibrations);
		}
	}
}