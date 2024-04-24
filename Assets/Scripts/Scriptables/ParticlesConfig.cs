using AYellowpaper.SerializedCollections;
using Particles;
using System.Collections.Generic;
using UnityEngine;

namespace Scriptables
{
	[CreateAssetMenu(order = 46)]
	public class ParticlesConfig : ScriptableObject
	{
		[SerializeField] private SerializedDictionary<ParticleType, ParticleSystem> _particles;

		public Dictionary<ParticleType, ParticleSystem> Particles => new Dictionary<ParticleType, ParticleSystem>(_particles);
	}
}