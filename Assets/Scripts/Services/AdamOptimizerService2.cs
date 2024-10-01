using System;

namespace Assets.Scripts.Services
{
    /// <summary>
    /// Implements the Adam optimization algorithm for updating weights and biases in a neural network.
    /// </summary>
    internal class AdamOptimizerService2
    {
        /// <summary>
        /// The learning rate used for the weight updates.
        /// </summary>
        public float LearningRate { get; set; }

        /// <summary>
        /// The exponential decay rate for the first moment estimates.
        /// </summary>
        public float Beta1 { get; set; }

        /// <summary>
        /// The exponential decay rate for the second moment estimates.
        /// </summary>
        public float Beta2 { get; set; }

        /// <summary>
        /// A small constant to prevent division by zero.
        /// </summary>
        public float Epsilon { get; set; }

        /// <summary>
        /// The first moment estimates for weights.
        /// </summary>
        public float[][][] M { get; set; }

        /// <summary>
        /// The second moment estimates for weights.
        /// </summary>
        public float[][][] V { get; set; }

        /// <summary>
        /// The first moment estimates for biases.
        /// </summary>
        public float[] MBias { get; set; }

        /// <summary>
        /// The second moment estimates for biases.
        /// </summary>
        public float[] VBias { get; set; }

        public bool IsInitialized { get; set; } = false;

        /// <summary>
        /// Initializes a the existing instance of the <see cref="AdamOptimizerService"/> class.
        /// </summary>
        /// <param name="layerSizes">The sizes of the layers in the network, used to initialize moment estimates.</param>
        /// <param name="learningRate">The learning rate for weight updates (default is 0.001).</param>
        /// <param name="beta1">The exponential decay rate for the first moment estimates (default is 0.9).</param>
        /// <param name="beta2">The exponential decay rate for the second moment estimates (default is 0.999).</param>
        /// <param name="epsilon">A small constant to prevent division by zero (default is 1e-8).</param>
        public void Init(int[] layerSizes, float learningRate = 0.001f, float beta1 = 0.9f, float beta2 = 0.999f, float epsilon = 1e-8f)
        {
            this.LearningRate = learningRate;
            this.Beta1 = beta1;
            this.Beta2 = beta2;
            this.Epsilon = epsilon;

            M = new float[layerSizes.Length][][];
            V = new float[layerSizes.Length][][];

            for (int i = 1; i < layerSizes.Length; i++)
            {
                M[i] = new float[layerSizes[i]][];
                V[i] = new float[layerSizes[i]][];

                for (int j = 0; j < layerSizes[i]; j++)
                {
                    M[i][j] = new float[layerSizes[i - 1]];
                    V[i][j] = new float[layerSizes[i - 1]];
                }
            }

            MBias = new float[layerSizes.Length];
            VBias = new float[layerSizes.Length];
            IsInitialized = true;
        }

        /// <summary>
        /// Updates the weights and biases of a neural network using the Adam optimization algorithm.
        /// This method implements the Adam optimizer, which adjusts the network's parameters
        /// based on first (momentum) and second (adaptive learning rate) moment estimates of the gradients.
        /// 
        /// Adam combines the benefits of:
        /// 1. **Momentum-based optimization (using moving averages of past gradients)**.
        /// 2. **Adaptive learning rates (using moving averages of squared gradients)**.
        /// 
        /// The method also tracks the minimum and maximum values of weights and weight deltas
        /// (changes in weights) to provide insight into the scale of the updates.
        ///
        /// <para>
        /// Adam formula for updating weights:
        /// w(t+1) = w(t) - learning_rate * m_hat / (sqrt(v_hat) + epsilon)
        /// Where:
        /// - m_hat is the bias-corrected first moment estimate (gradient momentum).
        /// - v_hat is the bias-corrected second moment estimate (adaptive learning rate).
        /// </para>
        /// </summary>
        /// <param name="weights">The current weights of the network.</param>
        /// <param name="biases">The current biases of the network.</param>
        /// <param name="weightGradients">The gradients of the weights obtained from backpropagation.</param>
        /// <param name="biasGradients">The gradients of the biases obtained from backpropagation.</param>
        /// <param name="epoch">The current epoch or time step, used to compute bias-corrected moment estimates.</param>
        /// <param name="weightMin">Outputs the minimum updated weight after applying the Adam update.</param>
        /// <param name="weightMax">Outputs the maximum updated weight after applying the Adam update.</param>
        /// <param name="newWeightDeltaMin">Outputs the minimum change in weight after the update.</param>
        /// <param name="newWeightDeltaMax">Outputs the maximum change in weight after the update.</param>
        public void UpdateWeights(float[][][] weights, float[][] biases, float[][][] weightGradients, float[][] biasGradients, int epoch, out float weightMin, out float weightMax, out float newWeightDeltaMin, out float newWeightDeltaMax)
        {
            // Initialize the minimum and maximum weights and weight deltas with extreme values
            float newMinWeight = float.MaxValue;
            float newMaxWeight = float.MinValue;

            float newMinWeightDelta = float.MaxValue;
            float newMaxWeightDelta = float.MinValue;

            // Compute the learning rate corrected for bias (due to initialization of moving averages)
            float correctedLearningRate = LearningRate * MathF.Sqrt(1f - MathF.Pow(Beta2, epoch)) / (1f - MathF.Pow(Beta1, epoch));

            // Iterate over each layer in the neural network (skipping input layer)
            for (int layerIndex = 1; layerIndex < weights.Length; layerIndex++)
            {
                // Iterate over each neuron in the current layer
                for (int neuronIndex = 0; neuronIndex < weights[layerIndex].Length; neuronIndex++)
                {
                    // Iterate over each weight connected to the current neuron
                    for (int weightIndex = 0; weightIndex < weights[layerIndex][neuronIndex].Length; weightIndex++)
                    {
                        // Update the first moment estimate (momentum of gradients)
                        M[layerIndex][neuronIndex][weightIndex] = Beta1 * M[layerIndex][neuronIndex][weightIndex] + (1f - Beta1) * weightGradients[layerIndex][neuronIndex][weightIndex];

                        // Update the second moment estimate (squared gradients for adaptive learning rate)
                        V[layerIndex][neuronIndex][weightIndex] = Beta2 * V[layerIndex][neuronIndex][weightIndex] + (1f - Beta2) * weightGradients[layerIndex][neuronIndex][weightIndex] * weightGradients[layerIndex][neuronIndex][weightIndex];

                        // Compute bias-corrected moment estimates (to address bias due to initialization)
                        float mHat = M[layerIndex][neuronIndex][weightIndex] / (1f - MathF.Pow(Beta1, epoch));
                        float vHat = V[layerIndex][neuronIndex][weightIndex] / (1f - MathF.Pow(Beta2, epoch));

                        // Store original weight before update for delta calculation
                        float originalWeight = weights[layerIndex][neuronIndex][weightIndex];

                        // Update the weight using the Adam formula
                        float newWeight = weights[layerIndex][neuronIndex][weightIndex] - correctedLearningRate * mHat / (MathF.Sqrt(vHat) + Epsilon);
                        float weightDelta = newWeight - originalWeight;

                        // Apply the weight update
                        weights[layerIndex][neuronIndex][weightIndex] = newWeight;

                        // Track minimum and maximum weight values
                        if (newWeight < newMinWeight) newMinWeight = newWeight;
                        if (newWeight > newMaxWeight) newMaxWeight = newWeight;

                        // Track the minimum and maximum changes in weights
                        if (weightDelta < newMinWeightDelta) newMinWeightDelta = weightDelta;
                        if (weightDelta > newMaxWeightDelta) newMaxWeightDelta = weightDelta;
                    }

                    // Update the first moment estimate for biases
                    MBias[layerIndex] = Beta1 * MBias[layerIndex] + (1f - Beta1) * biasGradients[layerIndex][neuronIndex];

                    // Update the second moment estimate for biases (squared bias gradients)
                    VBias[layerIndex] = Beta2 * VBias[layerIndex] + (1f - Beta2) * biasGradients[layerIndex][neuronIndex] * biasGradients[layerIndex][neuronIndex];

                    // Compute bias-corrected bias estimates
                    float mBiasHat = MBias[layerIndex] / (1f - MathF.Pow(Beta1, epoch));
                    float vBiasHat = VBias[layerIndex] / (1f - MathF.Pow(Beta2, epoch));

                    // Update the bias using the Adam formula
                    float originalBias = biases[layerIndex][neuronIndex];
                    float newBias = biases[layerIndex][neuronIndex] - correctedLearningRate * mBiasHat / (MathF.Sqrt(vBiasHat) + Epsilon);
                    float biasDelta = newBias - originalBias;

                    // Apply the bias update
                    biases[layerIndex][neuronIndex] = newBias;

                    // Track minimum and maximum values of biases
                    if (newBias < newMinWeight) newMinWeight = newBias;
                    if (newBias > newMaxWeight) newMaxWeight = newBias;

                    // Track the minimum and maximum changes in biases (as part of weight deltas)
                    if (biasDelta < newMinWeightDelta) newMinWeightDelta = biasDelta;
                    if (biasDelta > newMaxWeightDelta) newMaxWeightDelta = biasDelta;
                }
            }

            // Output the tracked minimum and maximum values for weights and deltas
            weightMin = newMinWeight;
            weightMax = newMaxWeight;
            newWeightDeltaMin = newMinWeightDelta;
            newWeightDeltaMax = newMaxWeightDelta;
        }
    }
}
