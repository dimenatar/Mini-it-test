using Merge;
using System.Collections.Generic;
using UnityEngine;

namespace Scriptables
{
    [CreateAssetMenu(order = 43)]
	public class FruitEvolutionConfig : ScriptableObject
    {
        [SerializeField] private List<FruitsEvolutionFormula> _formulas;

        public List<FruitsEvolutionFormula> BuildingEvolutionFormulas => new List<FruitsEvolutionFormula>(_formulas);
    }
}