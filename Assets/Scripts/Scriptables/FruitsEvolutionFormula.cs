using Fruits;
using UnityEngine;

namespace Merge
{
	[System.Serializable]
    public struct FruitsEvolutionFormula
    {
        [SerializeField] private FruitName _component;
        [SerializeField] private FruitName _result;

        public FruitsEvolutionFormula(FruitName component, FruitName result)
        {
            _component = component;
            _result = result;
        }

        public FruitName Component => _component;
        public FruitName Result => _result;
    }
}