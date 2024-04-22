using Merdge;
using System.Collections.Generic;
using UnityEngine;

namespace Scriptables
{
	public class FruitEvolutionConfig : MonoBehaviour
    {
        [SerializeField] private List<FruitsEvolutionFormula> _formulas;

        public List<FruitsEvolutionFormula> BuildingEvolutionFormulas => new List<FruitsEvolutionFormula>(_formulas);
    }
}