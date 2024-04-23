using Fruits;
using System;
using UnityEngine;
using Zenject;

public class InitialFruitSpawner : MonoBehaviour, IProgress, ITickable
{
	[SerializeField] private float _reducingStep = 1f;
	[SerializeField] private float _delay = 5f;

	private float _passedTime;
	private FruitName _initialFruitName;
	private Level _level;
	private FruitSpawner _fruitSpawner;

	public float Delay => _delay;
	public float PassedTime
	{
		get => _passedTime;
		private set
		{
			if (value < 0)
			{
				value = 0;
			}

			_passedTime = value;
			PassedTimeChanged?.Invoke(value);
		}
	}
	public bool IsLoaded { get; private set; }

	public event Action<float> PassedTimeChanged;

	[Inject]
	private void Construct(DataController dataController, FruitName initialFruitName, Level level, FruitSpawner fruitSpawner)
	{
		dataController.AddProgress(this);
		_initialFruitName = initialFruitName;
		_level = level;
		_fruitSpawner = fruitSpawner;
	}

	public void ClearProgress()
	{
		IsLoaded = false;
		PassedTime = 0f;
	}

	public void LoadProgress(DictionaryProgressManager dictionaryProgressManager)
	{
		PassedTime = dictionaryProgressManager.GetFloat(Tags.SPAWNER_TIMER);
	}

	public void SaveProgress(DictionaryProgressManager dictionaryProgressManager)
	{
		dictionaryProgressManager.SaveValue(Tags.SPAWNER_TIMER, PassedTime);
	}

	public void Tick()
	{
		if (_level.IsSpace())
		{
			PassedTime -= _reducingStep;

			if (PassedTime == 0)
			{
				SpawnFruit();
			}
			PassedTime = Delay;
		}
	}

	private void SpawnFruit()
	{
		var freeTile = _level.GetFreeTile();
		_fruitSpawner.SpawnFruit(freeTile, _initialFruitName);
	}
}
