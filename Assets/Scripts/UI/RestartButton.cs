using Scenes;
using UnityEngine;
using UnityEngine.UI;
using Zenject;

namespace UI
{
	public class RestartButton : MonoBehaviour
	{
		[SerializeField] private Button _button;

		private Restarter _restarter;
		private bool _isRestarted;

		private void Awake()
		{
			_button.onClick.AddListener(Restart);
		}

		[Inject]
		private void Construct(Restarter restarter)
		{
			_restarter = restarter;
		}

		private void Restart()
		{
			if (!_isRestarted)
			{
				_isRestarted = true;
				_restarter.Restart();
			}
		}
	}
}