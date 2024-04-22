using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Environment
{
	public class LevelProvider
	{
		public Level Level { get; private set; }

		public void SetLevel(Level level)
		{
			Level = level;
		}
	}
}