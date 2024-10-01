using Assets.Scripts.Enums;
using Assets.Scripts.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Assets.Scripts.Entities
{
    internal class NeuralNetwork
    {
        /// <summary>
        /// Minimum value of the weights in the neural network. 
        /// Used for tracking the range of weight values.
        /// </summary>
        public float WeightMin { get; set; } = float.MaxValue;

        /// <summary>
        /// Maximum value of the weights in the neural network.
        /// Used for tracking the range of weight values.
        /// </summary>
        public float WeightMax { get; set; } = float.MinValue;

        public float WeightDeltaMin { get; set; } = float.MaxValue;

        public float WeightDeltaMax { get; set; } = float.MinValue;

        public float ActivationMin { get; set; } = float.MaxValue;

        public float ActivationMax { get; set; } = float.MinValue;

        /// <summary>
        /// 3D array of weights in the neural network.
        /// Weights[layerIndex][neuronIndex][weightIndex] represents the weights for the given neuron in a specific layer.
        /// </summary>
        public float[][][] Weights { get; set; } = new float[0][][];

        /// <summary>
        /// 3D array of weight results during the forward propagation.
        /// WeightResults[layerIndex][neuronIndex][weightIndex] represents the result of multiplying the weight with the neuron's activation.
        /// </summary>
        public float[][][] WeightResults { get; set; } = new float[0][][];

        /// <summary>
        /// 2D array of biases for the neurons in the neural network.
        /// Biases[layerIndex][neuronIndex] represents the bias for the given neuron in a specific layer.
        /// </summary>
        public float[][] Biases { get; set; } = new float[0][];

        /// <summary>
        /// 2D array of activations for the neurons in the neural network.
        /// Activations[layerIndex][neuronIndex] represents the activation value of the given neuron in a specific layer.
        /// </summary>
        public float[][] Activations { get; set; } = new float[0][];

        /// <summary>
        /// 2D array of sum results for the neurons in the neural network.
        /// SumResults[layerIndex][neuronIndex] represents the weighted sum of the inputs plus the bias for the given neuron in a specific layer.
        /// </summary>
        public float[][] SumResults { get; set; } = new float[0][];

        /// <summary>
        /// 2D array of activation functions used for each neuron.
        /// ActivationFunctions[layerIndex][neuronIndex] represents the activation function type applied to the given neuron in a specific layer.
        /// </summary>
        public ActivationFunctionType[][] ActivationFunctions { get; set; } = new ActivationFunctionType[0][];

        /// <summary>
        /// 2D array of activation function parameters.
        /// ActivationFunctionParameters[layerIndex][neuronIndex] contains parameters required for the activation function applied to the given neuron in a specific layer.
        /// </summary>
        public List<string>[][] ActivationFunctionParameters { get; set; } = new List<string>[0][];

        /// <summary>
        /// 2D array of errors for each neuron in the neural network.
        /// Errors[layerIndex][neuronIndex] represents the error value associated with the given neuron in a specific layer.
        /// </summary>
        public float[][] Errors { get; set; } = new float[0][];

        /// <summary>
        /// Total error across the entire neural network.
        /// This value aggregates the errors from all output neurons to provide an overall measure of error.
        /// </summary>
        public float ErrorTotal { get; set; } = 0;

        /// <summary>
        /// Converts the neural network's state into a human-readable string format.
        /// Includes information about activations, errors, biases, weights, and other parameters.
        /// </summary>
        /// <returns>A string representation of the neural network's state.</returns>
        public override string ToString()
        {
            StringBuilder stringBuilder = new();
            stringBuilder.AppendLine($"ErrorTotal: {ErrorTotal}");
            stringBuilder.AppendLine($"WeightMin: {WeightMin}");
            stringBuilder.AppendLine($"WeightMin: {WeightMax}");
            stringBuilder.AppendLine($"Layers:");
            for (int layerIndex = 0; layerIndex < Activations.Length; layerIndex++)
            {
                stringBuilder.AppendLine($"  {layerIndex + 1}. Layer:");
                for (int neuronIndex = 0; neuronIndex < Activations[layerIndex].Length; neuronIndex++)
                {
                    stringBuilder.AppendLine($"    {neuronIndex + 1}. Neuron:");
                    stringBuilder.AppendLine($"      Activation: {Activations[layerIndex][neuronIndex]}");
                    if (layerIndex == 0) continue;
                    stringBuilder.AppendLine($"      Error: {Errors[layerIndex][neuronIndex]}");
                    stringBuilder.AppendLine($"      ActivationFunction: {ActivationFunctions[layerIndex][neuronIndex]}");
                    stringBuilder.AppendLine($"      ActivationFunctionParameter: {string.Join(", ", ActivationFunctionParameters[layerIndex][neuronIndex])}");
                    stringBuilder.AppendLine($"      SumResult: {SumResults[layerIndex][neuronIndex]}");
                    stringBuilder.AppendLine($"      Bias: {Biases[layerIndex][neuronIndex]}");
                    stringBuilder.AppendLine($"      Weights:");
                    for (int weightIndex = 0; weightIndex < Weights[layerIndex][neuronIndex].Length; weightIndex++)
                    {
                        stringBuilder.AppendLine($"        {weightIndex + 1}. Weight:");
                        stringBuilder.AppendLine($"          Weight: {Weights[layerIndex][neuronIndex][weightIndex]}");
                        stringBuilder.AppendLine($"          WeightResult: {WeightResults[layerIndex][neuronIndex][weightIndex]}");
                        if (weightIndex != Weights[layerIndex][neuronIndex].Length - 1) stringBuilder.AppendLine();
                    }
                    if (neuronIndex != Activations[layerIndex].Length - 1) stringBuilder.AppendLine();
                }
                if (layerIndex != Activations.Length - 1) stringBuilder.AppendLine();
            }
            return stringBuilder.ToString();
        }

        /// <summary>
        /// Creates a deep copy of the current neural network instance.
        /// </summary>
        /// <returns>A new <see cref="NeuralNetwork"/> instance with the same properties as the current one.</returns>
        public NeuralNetwork Clone()
        {
            return new()
            {
                WeightMax = WeightMax,
                WeightMin = WeightMin,
                WeightDeltaMin = WeightDeltaMin,
                WeightDeltaMax = WeightDeltaMax,
                Weights = CopyUtil.CopyArray(Weights),
                WeightResults = CopyUtil.CopyArray(WeightResults),
                Biases = CopyUtil.CopyArray(Biases),
                Activations = CopyUtil.CopyArray(Activations),
                SumResults = CopyUtil.CopyArray(SumResults),
                ActivationFunctions = CopyUtil.CopyArray(ActivationFunctions),
                ActivationFunctionParameters = CopyUtil.CopyArray(ActivationFunctionParameters),
                Errors = CopyUtil.CopyArray(Errors),
                ErrorTotal = ErrorTotal
            };
        }

        /// <summary>
        /// Performs forward propagation through all layers of the neural network.
        /// This method iterates through each layer and processes each neuron.
        /// </summary>
        public void ForwardPropagate()
        {
            // Forward propagate through each layer starting from the second layer
            for (int layerIndex = 1; layerIndex < Activations.Length; layerIndex++)
            {
                ForwardPropagateLayer(layerIndex);
            }
        }

        /// <summary>
        /// Performs forward propagation for a specific layer.
        /// Processes each neuron in the specified layer.
        /// </summary>
        /// <param name="layerIndex">The index of the layer to process.</param>
        public void ForwardPropagateLayer(int layerIndex)
        {
            //if (layerIndex == 0) return;

            // Process each neuron in the specified layer
            for (int neuronIndex = 0; neuronIndex < Activations[layerIndex].Length; neuronIndex++)
            {
                ForwardPropagateNeuron(layerIndex, neuronIndex);
            }
        }

        /// <summary>
        /// Performs forward propagation for a specific neuron in a given layer.
        /// Computes the weighted sum and applies the activation function.
        /// </summary>
        /// <param name="layerIndex">The index of the layer containing the neuron.</param>
        /// <param name="neuronIndex">The index of the neuron to process.</param>
        public void ForwardPropagateNeuron(int layerIndex, int neuronIndex)
        {
            //if (layerIndex == 0) return;

            // Compute the weighted results for the neuron
            for (int weightIndex = 0; weightIndex < Weights[layerIndex][neuronIndex].Length; weightIndex++)
            {
                ForwardPropagateWeight(layerIndex, neuronIndex, weightIndex);
            }
            // Compute the weighted sum and apply the activation function
            ForwardPropagateSum(layerIndex, neuronIndex);
            ForwardPropagateActivationFunction(layerIndex, neuronIndex);
        }

        /// <summary>
        /// Computes the result of multiplying the activation of the previous neuron by the weight.
        /// Stores the result for later use.
        /// </summary>
        /// <param name="layerIndex">The index of the layer containing the neuron.</param>
        /// <param name="neuronIndex">The index of the neuron whose weight result is being computed.</param>
        /// <param name="weightIndex">The index of the weight to process.</param>
        public void ForwardPropagateWeight(int layerIndex, int neuronIndex, int weightIndex)
        {
            //if (layerIndex == 0) return;

            // Get the weight and previous neuron's activation
            float weight = Weights[layerIndex][neuronIndex][weightIndex];
            float previousNeuronActivation = Activations[layerIndex - 1][weightIndex];

            // Calculate the weight result
            float weightResult = previousNeuronActivation * weight;

            // Store the weight result
            WeightResults[layerIndex][neuronIndex][weightIndex] = weightResult;
        }

        /// <summary>
        /// Computes the sum of all weighted results for a neuron, including its bias.
        /// </summary>
        /// <param name="layerIndex">The index of the layer containing the neuron.</param>
        /// <param name="neuronIndex">The index of the neuron whose sum is being computed.</param>
        public void ForwardPropagateSum(int layerIndex, int neuronIndex)
        {
            //if (layerIndex == 0) return;

            // Retrieve bias and weighted results for the neuron
            float bias = Biases[layerIndex][neuronIndex];
            float[] weightResults = WeightResults[layerIndex][neuronIndex];

            // Calculate the sum of weights and add bias
            float sumWeights = weightResults.Sum();
            float sum = bias + sumWeights;

            // Store the computed sum result
            SumResults[layerIndex][neuronIndex] = sum;
        }

        /// <summary>
        /// Applies the activation function to the computed sum for a neuron.
        /// Updates the neuron's activation value based on the activation function.
        /// </summary>
        /// <param name="layerIndex">The index of the layer containing the neuron.</param>
        /// <param name="neuronIndex">The index of the neuron whose activation function is being applied.</param>
        public void ForwardPropagateActivationFunction(int layerIndex, int neuronIndex)
        {
            //if (layerIndex == 0) return;

            // Get the activation function type for the neuron
            ActivationFunctionType activationFunction = ActivationFunctions[layerIndex][neuronIndex];

            float sum = SumResults[layerIndex][neuronIndex];

            // Apply the activation function based on its type
            float activation = activationFunction switch
            {
                ActivationFunctionType.RE_LU => ActivationFunctionUtil.ReLU(sum),
                ActivationFunctionType.LRE_LU => ActivationFunctionUtil.LReLU(sum, float.Parse(ActivationFunctionParameters[layerIndex][neuronIndex][0])),
                ActivationFunctionType.SIGMOID => ActivationFunctionUtil.Sigmoid(sum),
                ActivationFunctionType.TAN_H => ActivationFunctionUtil.TanH(sum),
                ActivationFunctionType.LINEAR => ActivationFunctionUtil.Linear(sum),
                _ => throw new System.Exception($"Activation function type is not supported ({activationFunction})"),
            };

            // Store the activation result
            Activations[layerIndex][neuronIndex] = activation;
            ActivationMin = Activations.Select(x => x.Min()).Min();
            ActivationMax = Activations.Select(x => x.Max()).Max();
        }
    }
}
