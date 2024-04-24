using Extensions;
using Merge;
using Scriptables;
using System.Collections.Generic;
using UnityEngine;

namespace Particles
{
	public class ParticlePlayer
	{
		private Dictionary<ParticleType, ParticleSystem> _particlesConfig;

		public ParticlePlayer(Merger merger, ParticlesConfig particlesConfig)
		{
			_particlesConfig = particlesConfig.Particles;

			merger.Merged += OnMerged;
		}

		private void OnMerged(Fruits.Fruit fruit)
		{
			PlayParticles(fruit.transform.position, ParticleType.Merged);
		}

		private void PlayParticles(Vector3 position, ParticleType particleType)
		{
			var original = _particlesConfig[particleType];
			var copy = Object.Instantiate(original, position, original.transform.rotation);
			copy.PlayWithDestroy();
		}
	}
}