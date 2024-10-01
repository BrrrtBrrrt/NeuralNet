using Assets.Scripts.Enums;
using Assets.Scripts.Services;
using Assets.Scripts.Types;
using Assets.Scripts.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace Assets.Scripts.Entities
{
    /// <summary>
    /// A class responsible for training a neural network using mini batch gradient descent and backpropagation with adam optimizer.
    /// </summary>
    internal class NeuralTrainer
    {
        /// <summary>
        /// Gets or sets the number of epochs for training.
        /// </summary>
        public int EpochCount { get; set; } = 50;

        /// <summary>
        /// Gets or sets the current epoch during training.
        /// </summary>
        public int CurrentEpoch { get; set; } = 0;

        /// <summary>
        /// Gets or sets the total number of iterations for training.
        /// </summary>
        public int IterationCount { get; set; } = 1;

        /// <summary>
        /// Gets or sets the current iteration during training.
        /// </summary>
        public int CurrentIteration { get; set; } = 0;

        /// <summary>
        /// Gets or sets the learning rate for weight updates.
        /// </summary>
        public float LearningRate { get; set; } = 0.05f;

        /// <summary>
        /// Gets or sets the type of loss function used for error calculation.
        /// </summary>
        public LossFunctionType LossFunction { get; set; } = LossFunctionType.MAE;

        /// <summary>
        /// Gets or sets the neural network to be trained.
        /// </summary>
        public NeuralNetwork Network { get; set; } = null;

        /// <summary>
        /// Gets or sets the expected output values for training examples.
        /// </summary>
        public float[] OutputExpected { get; set; } = new float[0];

        /// <summary>
        /// Gets or sets the training predictions for the current epoch.
        /// </summary>
        public XYData EpochTrainingPredictions { get; set; } = new();

        /// <summary>
        /// Gets or sets the test predictions for the current epoch.
        /// </summary>
        public XYData EpochTestPredictions { get; set; } = new();

        /// <summary>
        /// Gets or sets the list of epoch errors.
        /// </summary>
        public List<float> EpochErrors { get; set; } = new();

        /// <summary>
        /// Gets or sets the batch size for training.
        /// </summary>
        public int BatchSize { get; set; } = 32;

        /// <summary>
        /// Gets or sets the number of batches after which the training process should render.
        /// </summary>
        public int BatchCountPerRender { get; set; } = 1;


        public bool ReinitializeOptimizer { get; set; } = true;

        private AdamOptimizerService adamOptimizer = new();

        /*public NeuralTrainer()
        {
            adamOptimizer = new(Network.Activations.Select((layerActivations) => layerActivations.Length).ToArray(), LearningRate);
        }*/

        /// <summary>
        /// Calculates the total error of the network's predictions compared to the expected output using the specified loss function.
        /// </summary>
        public void CalculateError()
        {
            Network.ErrorTotal = LossFunction switch
            {
                LossFunctionType.MSE => LossFunctionUtil.MSE(OutputExpected, Network.Activations[^1]),
                LossFunctionType.SE => LossFunctionUtil.SE(OutputExpected, Network.Activations[^1]),
                LossFunctionType.E => LossFunctionUtil.E(OutputExpected, Network.Activations[^1]),
                LossFunctionType.MBCE => LossFunctionUtil.MBCE(OutputExpected, Network.Activations[^1]),
                LossFunctionType.LOG_COSH => LossFunctionUtil.LogCosh(OutputExpected, Network.Activations[^1]),
                LossFunctionType.HUBER => LossFunctionUtil.Huber(OutputExpected, Network.Activations[^1]),
                LossFunctionType.RMSE => LossFunctionUtil.RMSE(OutputExpected, Network.Activations[^1]),
                LossFunctionType.MAE => LossFunctionUtil.MAE(OutputExpected, Network.Activations[^1]),
                LossFunctionType.RMLSE => LossFunctionUtil.RMLSE(OutputExpected, Network.Activations[^1]),
                _ => throw new System.Exception($"Loss function type is not supported ({LossFunction})"),
            };
        }

        /// <summary>
        /// Backpropagates the error through the network to compute gradients.
        /// </summary>
        public void BackpropagateError()
        {
            // Traverse through the network layers from the last to the first
            for (int layerIndex = Network.Activations.Length - 1; layerIndex >= 1; layerIndex--)
            {
                if (layerIndex == Network.Activations.Length - 1)
                {
                    // Compute the error for the output layer
                    Network.Errors[layerIndex] = LossFunction switch
                    {
                        LossFunctionType.MSE => LossFunctionUtil.MSEDerivative(OutputExpected, Network.Activations[layerIndex]).ToArray(),
                        LossFunctionType.SE => LossFunctionUtil.SEDerivative(OutputExpected, Network.Activations[layerIndex]).ToArray(),
                        LossFunctionType.E => LossFunctionUtil.EDerivative(OutputExpected, Network.Activations[layerIndex]).ToArray(),
                        LossFunctionType.MBCE => LossFunctionUtil.MBCEDerivative(OutputExpected, Network.Activations[layerIndex]).ToArray(),
                        LossFunctionType.LOG_COSH => LossFunctionUtil.LogCoshDerivative(OutputExpected, Network.Activations[layerIndex]).ToArray(),
                        LossFunctionType.HUBER => LossFunctionUtil.HuberDerivative(OutputExpected, Network.Activations[layerIndex]).ToArray(),
                        LossFunctionType.RMSE => LossFunctionUtil.RMSEDerivative(OutputExpected, Network.Activations[layerIndex]).ToArray(),
                        LossFunctionType.MAE => LossFunctionUtil.MAEDerivative(OutputExpected, Network.Activations[layerIndex]).ToArray(),
                        LossFunctionType.RMLSE => LossFunctionUtil.RMLSEDerivative(OutputExpected, Network.Activations[layerIndex]).ToArray(),
                        _ => throw new System.Exception($"Loss function type is not supported ({LossFunction})"),
                    };
                }

                // Compute errors for hidden layers
                for (int neuronIndex = 0; neuronIndex < Network.Activations[layerIndex].Length; neuronIndex++)
                {
                    if (layerIndex == Network.Activations.Length - 1)
                    {
                        // Adjust the error for the output layer based on the activation function
                        Network.Errors[layerIndex][neuronIndex] *= Network.ActivationFunctions[layerIndex][neuronIndex] switch
                        {
                            ActivationFunctionType.TAN_H => ActivationFunctionUtil.TanHDerivative(Network.SumResults[layerIndex][neuronIndex]),
                            ActivationFunctionType.SIGMOID => ActivationFunctionUtil.SigmoidDerivative(Network.SumResults[layerIndex][neuronIndex]),
                            ActivationFunctionType.RE_LU => ActivationFunctionUtil.ReLUDerivative(Network.SumResults[layerIndex][neuronIndex]),
                            ActivationFunctionType.LRE_LU => ActivationFunctionUtil.LReLUDerivative(Network.SumResults[layerIndex][neuronIndex], float.Parse(Network.ActivationFunctionParameters[layerIndex][neuronIndex][0])),
                            ActivationFunctionType.LINEAR => ActivationFunctionUtil.LinearDerivative(Network.SumResults[layerIndex][neuronIndex]),
                            _ => throw new System.Exception($"Activation function type is not supported ({Network.ActivationFunctions[layerIndex][neuronIndex]})"),
                        };

                        continue; // Skip to the next neuron
                    }

                    // Reset error for hidden layer neurons
                    Network.Errors[layerIndex][neuronIndex] = 0;

                    // Accumulate errors from the subsequent layer
                    for (int neuronNextIndex = 0; neuronNextIndex < Network.Activations[layerIndex + 1].Length; neuronNextIndex++)
                    {
                        Network.Errors[layerIndex][neuronIndex] += Network.Weights[layerIndex + 1][neuronNextIndex][neuronIndex] * Network.Errors[layerIndex + 1][neuronNextIndex];
                    }

                    // Adjust the error based on the activation function
                    Network.Errors[layerIndex][neuronIndex] *= Network.ActivationFunctions[layerIndex][neuronIndex] switch
                    {
                        ActivationFunctionType.TAN_H => ActivationFunctionUtil.TanHDerivative(Network.SumResults[layerIndex][neuronIndex]),
                        ActivationFunctionType.SIGMOID => ActivationFunctionUtil.SigmoidDerivative(Network.SumResults[layerIndex][neuronIndex]),
                        ActivationFunctionType.RE_LU => ActivationFunctionUtil.ReLUDerivative(Network.SumResults[layerIndex][neuronIndex]),
                        ActivationFunctionType.LRE_LU => ActivationFunctionUtil.LReLUDerivative(Network.SumResults[layerIndex][neuronIndex], float.Parse(Network.ActivationFunctionParameters[layerIndex][neuronIndex][0])),
                        ActivationFunctionType.LINEAR => ActivationFunctionUtil.LinearDerivative(Network.SumResults[layerIndex][neuronIndex]),
                        _ => throw new System.Exception($"Activation function type is not supported ({Network.ActivationFunctions[layerIndex][neuronIndex]})"),
                    };
                }
            }
        }

        /// <summary>
        /// Executes the backpropagation process which includes calculating the error and propagating it back through the network.
        /// This method is a wrapper that calls <see cref="CalculateError"/> and <see cref="BackpropagateError"/>.
        /// </summary>
        public void Backpropagate()
        {
            CalculateError();
            BackpropagateError();
        }

        /// <summary>
        /// Trains the neural network using the provided training and test data.
        /// This method handles the mini-batch gradient descent, performs weight updates, and evaluates the network on test data.
        /// </summary>
        /// <param name="dataTraining">The training data consisting of input-output pairs.</param>
        /// <param name="dataTest">The test data consisting of input-output pairs.</param>
        /// <returns>An enumerator for iterating through the training process.</returns>
        public IEnumerator Train(XYData dataTraining, XYData dataTest)
        {
            // Check if the Adam optimizer needs to be initialized or reinitialized
            if (!adamOptimizer.IsInitialized || ReinitializeOptimizer)
            {
                // Initialize the Adam optimizer using the size of each layer and the learning rate
                adamOptimizer.Init(Network.Activations.Select((layerActivations) => layerActivations.Length).ToArray(), LearningRate);
                ReinitializeOptimizer = false; // Reset the reinitialization flag
            }

            // Clear the list of epoch errors and initialize iteration count
            EpochErrors.Clear();
            IterationCount = dataTraining.XY.Count + dataTest.XY.Count;  // Total iterations for both training and test data
            int numberOfBatches = (int)Math.Ceiling((double)dataTraining.XY.Count / BatchSize);  // Determine number of batches

            // Loop through each epoch
            for (int epoch = 0; epoch < EpochCount; epoch++)
            {
                // Reset epoch-specific data such as predictions and counters
                EpochTrainingPredictions = new();
                EpochTestPredictions = new();
                CurrentEpoch = epoch; // Track the current epoch
                CurrentIteration = 0; // Reset the iteration counter for the current epoch
                float epochError = 0; // Initialize the total error for this epoch

                // Loop through each batch within the training data
                for (int batchIndex = 0; batchIndex < numberOfBatches; batchIndex++)
                {
                    int batchStart = batchIndex * BatchSize; // Start index of the current batch
                    int batchEnd = Math.Min(batchStart + BatchSize, dataTraining.XY.Count); // End index of the current batch

                    // Get the current batch of data
                    List<XYDataEntry> batch = dataTraining.XY.GetRange(batchStart, batchEnd - batchStart);

                    // Initialize gradients for weights and biases
                    float[][][] weightGradients = new float[Network.Weights.Length][][]; // Weight gradients for each layer
                    float[][] biasGradients = new float[Network.Biases.Length][]; // Bias gradients for each layer

                    // Initialize arrays for storing gradients for all layers starting from layer 1 (ignoring input layer)
                    for (int layerIndex = 1; layerIndex < Network.Weights.Length; layerIndex++)
                    {
                        weightGradients[layerIndex] = new float[Network.Weights[layerIndex].Length][]; // Gradients for weights of each neuron
                        for (int neuronIndex = 0; neuronIndex < Network.Weights[layerIndex].Length; neuronIndex++)
                        {
                            weightGradients[layerIndex][neuronIndex] = new float[Network.Weights[layerIndex][neuronIndex].Length]; // Gradient for each weight of a neuron
                        }

                        biasGradients[layerIndex] = new float[Network.Biases[layerIndex].Length]; // Gradients for biases of each neuron
                    }

                    // Loop through each data entry in the current batch
                    // This loop calculates the gradients for the entire batch by accumulating the errors
                    foreach (XYDataEntry xyDataEntry in batch)
                    {
                        // Set the input layer activations to the input data for the current data entry
                        Network.Activations[0] = xyDataEntry.X.ToArray();

                        // Perform forward propagation through the network
                        Network.ForwardPropagate();

                        // Store the expected output for comparison later
                        OutputExpected = xyDataEntry.Y.ToArray();

                        // Store the predicted values for analysis later
                        EpochTrainingPredictions.XY.Add(new()
                        {
                            X = xyDataEntry.X,
                            Y = Network.Activations[^1].ToList(), // Network's final output (last layer's activations)
                        });

                        // Calculate the error for this data entry
                        CalculateError();

                        // Accumulate the total error for this 
                        epochError += Network.ErrorTotal;

                        // Perform backpropagation to calculate the error gradients
                        BackpropagateError();

                        // Accumulate the gradients for the weights and biases
                        for (int layerIndex = 1; layerIndex < Network.Weights.Length; layerIndex++)
                        {
                            for (int neuronIndex = 0; neuronIndex < Network.Weights[layerIndex].Length; neuronIndex++)
                            {
                                for (int weightIndex = 0; weightIndex < Network.Weights[layerIndex][neuronIndex].Length; weightIndex++)
                                {
                                    // Calculate the weight gradients for each weight in the current neuron
                                    weightGradients[layerIndex][neuronIndex][weightIndex] += Network.Errors[layerIndex][neuronIndex] * Network.Activations[layerIndex - 1][weightIndex];
                                }

                                // Calculate the bias gradients for each neuron
                                biasGradients[layerIndex][neuronIndex] += Network.Errors[layerIndex][neuronIndex];
                            }
                        }
                    }

                    // Average the gradients over the batch to normalize the accumulated gradients
                    for (int layerIndex = 1; layerIndex < Network.Weights.Length; layerIndex++)
                    {
                        for (int neuronIndex = 0; neuronIndex < Network.Weights[layerIndex].Length; neuronIndex++)
                        {
                            for (int weightIndex = 0; weightIndex < Network.Weights[layerIndex][neuronIndex].Length; weightIndex++)
                            {
                                weightGradients[layerIndex][neuronIndex][weightIndex] /= batch.Count; // Average weight gradient
                            }

                            biasGradients[layerIndex][neuronIndex] /= batch.Count; // Average bias gradient
                        }
                    }

                    // Update weights using Adam optimizer
                    adamOptimizer.UpdateWeights(Network.Weights, Network.Biases, weightGradients, biasGradients, epoch + 1, out float newWeightMin, out float newWeightMax, out float newWeightDeltaMin, out float newWeightDeltaMax);
                    Network.WeightMin = newWeightMin;
                    Network.WeightMax = newWeightMax;
                    Network.WeightDeltaMin = newWeightDeltaMin;
                    Network.WeightDeltaMax = newWeightDeltaMax;

                    // Increment the current iteration count by the size of the batch
                    CurrentIteration += batch.Count;

                    // Yield control periodically to allow for rendering between batches
                    if (batchIndex % BatchCountPerRender == 0) yield return null;
                }

                // Yield control after each epoch for UI updates, etc.
                yield return null;

                // Record the average error for this epoch
                EpochErrors.Add(epochError / dataTraining.XY.Count);

                // Evaluate the network on the test data after each epoch
                for (int dataTestIndex = 0; dataTestIndex < dataTest.XY.Count; dataTestIndex++)
                {
                    // Set the input layer activations to the test input data
                    XYDataEntry xyDataEntry = dataTest.XY[dataTestIndex];
                    Network.Activations[0] = xyDataEntry.X.ToArray();

                    // Perform forward propagation to get the network's output
                    Network.ForwardPropagate();

                    // Store the test predictions for analysis later
                    EpochTestPredictions.XY.Add(new()
                    {
                        X = xyDataEntry.X,
                        Y = Network.Activations[^1].ToList(), // Network's final output (predicted value)
                    });

                    // Yield control periodically during the test evaluation
                    if (dataTestIndex < dataTest.XY.Count - 1) CurrentIteration++;
                    if (dataTestIndex % BatchSize * BatchCountPerRender == 0) yield return null;
                }

                // Yield control at the end of each epoch to ensure smooth rendering
                yield return null;
            }
        }
    }
}
