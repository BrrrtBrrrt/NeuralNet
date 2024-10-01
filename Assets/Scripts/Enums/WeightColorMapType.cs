using UnityEngine;

namespace Assets.Scripts.Enums
{
    public enum WeightColorMapType
    {
        [InspectorName("Weights strength")]
        WEIGHTS_STRENGTH,
        [InspectorName("Momentum of weights change")]
        WEIGHTS_CHANGE_MOMENTUM,
        [InspectorName("Weights value based gradient")]
        WEIGHTS_VALUE_GRADIENT,
        [InspectorName("Activations value based gradient")]
        ACTIVATIONS_VALUE_GRADIENT,
        [InspectorName("White")]
        WHITE,
        [InspectorName("None")]
        NONE,
    }
}
