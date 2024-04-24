using System.Collections;
using UnityEngine;

namespace Updaters
{
	public class SecondsStepUpdater : Updater
	{
		private YieldInstruction _yildInstruction;

		private void Start()
		{
			_yildInstruction = new WaitForSeconds(1);
			StartCoroutine(RunCycle());
		}

		private void OnDestroy()
		{
			StopAllCoroutines();
		}

		private IEnumerator RunCycle()
		{
			while (true)
			{
				yield return _yildInstruction;
				InvokeTicks();
			}
		}
	}
}