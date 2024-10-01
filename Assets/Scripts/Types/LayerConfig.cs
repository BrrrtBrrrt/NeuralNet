using Assets.Scripts.Enums;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Types
{
    /// <summary>
    /// Configuration settings for a neural network layer.
    /// </summary>
    [Serializable]
    public class LayerConfig
    {
        /// <summary>
        /// The number of neurons in the layer.
        /// Must be at least 1.
        /// </summary>
        [Min(1)]
        public int NeuronCount = 1;

        /// <summary>
        /// The activation function to be used by the layer.
        /// </summary>
        public ActivationFunctionType ActivationFunction = ActivationFunctionType.TAN_H;

        /// <summary>
        /// The strategy for initializing the weights of the layer.
        /// </summary>
        public WeightsInitializationStrategyType WeightsInitializationStrategy = WeightsInitializationStrategyType.XAVIER_UNIFORM;

        /// <summary>
        /// The strategy for initializing the biases of the layer.
        /// </summary>
        public BiasesInitializationStrategyType BiasesInitializationStrategy = BiasesInitializationStrategyType.VALUE0;

        /// <summary>
        /// Arguments for the activation function, if required.
        /// Default value is a list containing a single string "0.01".
        /// </summary>
        public List<string> ActivationFunctionArgs = new()
        {
            "0.01"
        };
    }
}
