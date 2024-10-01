using Assets.Scripts.Enums;
using Assets.Scripts.Services;
using Assets.Scripts.Types;
using Assets.Scripts.Utils;
using System.Collections.Generic;

namespace Assets.Scripts.Entities
{
    internal static class NeuralGenerator
    {
        /// <summary>
        /// Generates a new neural network based on the provided layer configurations and weight scaling parameters.
        /// </summary>
        /// <param name="layerConfigs">Array of layer configurations describing the network structure and initialization strategies.</param>
        /// <param name="weightsScaleMin">The minimum value to which weights will be scaled.</param>
        /// <param name="weightsScaleMax">The maximum value to which weights will be scaled.</param>
        /// <returns>A newly created <see cref="NeuralNetwork"/> instance with initialized parameters.</returns>
        public static NeuralNetwork Generate(LayerConfig[] layerConfigs, float weightsScaleMin, float weightsScaleMax)
        {
            // Initialize the neural network with appropriate dimensions
            NeuralNetwork network = new()
            {
                ActivationFunctions = new ActivationFunctionType[layerConfigs.Length][],
                ActivationFunctionParameters = new List<string>[layerConfigs.Length][],
                Activations = new float[layerConfigs.Length][],
                Biases = new float[layerConfigs.Length][],
                SumResults = new float[layerConfigs.Length][],
                Errors = new float[layerConfigs.Length][],
                Weights = new float[layerConfigs.Length][][],
                WeightResults = new float[layerConfigs.Length][][],
            };

            // Variables to track the min and max weight values for scaling later
            float minWeight = float.MaxValue;
            float maxWeight = float.MinValue;

            // Iterate through each layer configuration
            for (int layerIndex = 0; layerIndex < layerConfigs.Length; layerIndex++)
            {
                LayerConfig layerConfig = layerConfigs[layerIndex];

                // Initialize arrays for activations in the current layer
                network.Activations[layerIndex] = new float[layerConfig.NeuronCount];

                // Skip the first layer as it does not have incoming weights
                if (layerIndex == 0) continue;

                LayerConfig layerConfigPrevious = layerConfigs[layerIndex - 1];
                LayerConfig layerConfigNext = layerIndex + 1 < layerConfigs.Length ? layerConfigs[layerIndex + 1] : null;

                // Initialize arrays and parameters for the current layer
                network.ActivationFunctions[layerIndex] = new ActivationFunctionType[layerConfig.NeuronCount];
                network.ActivationFunctionParameters[layerIndex] = new List<string>[layerConfig.NeuronCount];
                network.Biases[layerIndex] = new float[layerConfig.NeuronCount];
                network.SumResults[layerIndex] = new float[layerConfig.NeuronCount];
                network.Errors[layerIndex] = new float[layerConfig.NeuronCount];
                network.Weights[layerIndex] = new float[layerConfig.NeuronCount][];
                network.WeightResults[layerIndex] = new float[layerConfig.NeuronCount][];

                // Initialize each neuron's weights and biases
                for (int neuronIndex = 0; neuronIndex < layerConfig.NeuronCount; neuronIndex++)
                {
                    network.Weights[layerIndex][neuronIndex] = new float[layerConfigPrevious.NeuronCount];
                    network.WeightResults[layerIndex][neuronIndex] = new float[layerConfigPrevious.NeuronCount];
                    network.ActivationFunctions[layerIndex][neuronIndex] = layerConfig.ActivationFunction;
                    network.ActivationFunctionParameters[layerIndex][neuronIndex] = layerConfig.ActivationFunctionArgs;
                    network.Biases[layerIndex][neuronIndex] = layerConfig.BiasesInitializationStrategy switch
                    {
                        BiasesInitializationStrategyType.VALUE1 => 1,
                        BiasesInitializationStrategyType.VALUE0D01 => 0.01f,
                        BiasesInitializationStrategyType.VALUE0 => 0,
                        _ => throw new System.Exception($"Biases initialization strategy type is not supported ({layerConfig.BiasesInitializationStrategy})"),
                    };
                    // Track the minimum and maximum biases for later scaling
                    minWeight = network.Biases[layerIndex][neuronIndex] < minWeight ? network.Biases[layerIndex][neuronIndex] : minWeight;
                    maxWeight = network.Biases[layerIndex][neuronIndex] > maxWeight ? network.Biases[layerIndex][neuronIndex] : maxWeight;

                    // Initialize each weight in the neuron's weight array
                    for (int weightIndex = 0; weightIndex < network.Weights[layerIndex][neuronIndex].Length; weightIndex++)
                    {
                        network.Weights[layerIndex][neuronIndex][weightIndex] = layerConfig.WeightsInitializationStrategy switch
                        {
                            WeightsInitializationStrategyType.NORMAL => WeightInitializerUtil.NormalInit(),
                            WeightsInitializationStrategyType.RANDOM => WeightInitializerUtil.RandomInit(),
                            WeightsInitializationStrategyType.XAVIER_NORMAL => WeightInitializerUtil.XavierNormalInit(layerConfigPrevious.NeuronCount, layerConfigNext == null ? 0 : layerConfigNext.NeuronCount),
                            WeightsInitializationStrategyType.XAVIER_UNIFORM => WeightInitializerUtil.XavierUniformInit(layerConfigPrevious.NeuronCount, layerConfigNext == null ? 0 : layerConfigNext.NeuronCount),
                            _ => throw new System.Exception($"Weights initialization strategy type is not supported ({layerConfig.WeightsInitializationStrategy})"),
                        };

                        // Track the minimum and maximum weights for later scaling
                        minWeight = network.Weights[layerIndex][neuronIndex][weightIndex] < minWeight ? network.Weights[layerIndex][neuronIndex][weightIndex] : minWeight;
                        maxWeight = network.Weights[layerIndex][neuronIndex][weightIndex] > maxWeight ? network.Weights[layerIndex][neuronIndex][weightIndex] : maxWeight;
                    }
                }
            }

            // Scale the weights to the desired range
            ScaleWeights(weightsScaleMin, weightsScaleMax, network, minWeight, maxWeight);

            // Set the min and max weight values for the network
            network.WeightMin = weightsScaleMin;
            network.WeightMax = weightsScaleMax;
            network.WeightDeltaMin = 0;
            network.WeightDeltaMax = 0;

            return network;
        }

        /// <summary>
        /// Scales the weights of the neural network to the specified range.
        /// </summary>
        /// <param name="weightsScaleMin">The minimum value to scale weights to.</param>
        /// <param name="weightsScaleMax">The maximum value to scale weights to.</param>
        /// <param name="network">The neural network whose weights are being scaled.</param>
        /// <param name="minWeight">The minimum weight value before scaling.</param>
        /// <param name="maxWeight">The maximum weight value before scaling.</param>
        private static void ScaleWeights(float weightsScaleMin, float weightsScaleMax, NeuralNetwork network, float minWeight, float maxWeight)
        {
            ScalerService scalerService = new()
            {
                OriginalMin = minWeight,
                OriginalMax = maxWeight,
                NewMin = weightsScaleMin,
                NewMax = weightsScaleMax
            };

            // Scale each weight in the network
            for (int layerIndex = 1; layerIndex < network.Weights.Length; layerIndex++)
            {
                for (int neuronIndex = 0; neuronIndex < network.Weights[layerIndex].Length; neuronIndex++)
                {
                    for (int weightIndex = 0; weightIndex < network.Weights[layerIndex][neuronIndex].Length; weightIndex++)
                    {
                        network.Weights[layerIndex][neuronIndex][weightIndex] = scalerService.Scale(network.Weights[layerIndex][neuronIndex][weightIndex]);
                    }
                }
            }
        }
    }
}
