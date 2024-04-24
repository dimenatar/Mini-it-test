using Doozy.Runtime.Reactor;
using Fruits;
using UnityEngine;
using Zenject;

namespace UI
{
	public class SpawnerTimerView : MonoBehaviour
	{
		[SerializeField] private Progressor _progressor;

		private InitialFruitSpawner _initialFruitSpawner;

		[Inject]
		private void Construct(InitialFruitSpawner spawner)
		{
			_initialFruitSpawner = spawner;
		}

		private void Awake()
		{
			_initialFruitSpawner.PassedTimeChanged += OnPassedTimeChanged;
		}

		private void Start()
		{
			_progressor.SetProgressAt(1 - _initialFruitSpawner.PassedTime / _initialFruitSpawner.Delay);
		}

		private void OnDestroy()
		{
			_initialFruitSpawner.PassedTimeChanged -= OnPassedTimeChanged;
		}

		private void OnPassedTimeChanged(float time)
		{
			_progressor.SetProgressAt(1 - time / _initialFruitSpawner.Delay);
		}
	}
}