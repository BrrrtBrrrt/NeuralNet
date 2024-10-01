using System;

namespace Assets.Scripts.Services
{
    /// <summary>
    /// Implements the Adam optimization algorithm for updating weights and biases in a neural network.
    /// </summary>
    internal class AdamOptimizerService
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
        /// Updates the weights and biases using the Adam optimization algorithm.
        /// </summary>
        /// <param name="weights">The current weights of the network.</param>
        /// <param name="biases">The current biases of the network.</param>
        /// <param name="weightGradients">The gradients of the weights.</param>
        /// <param name="biasGradients">The gradients of the biases.</param>
        /// <param name="epoch">The current epoch, used to compute bias-corrected moment estimates.</param>
        public void UpdateWeights(float[][][] weights, float[][] biases, float[][][] weightGradients, float[][] biasGradients, int epoch, out float weightMin, out float weightMax, out float newWeightDeltaMin, out float newWeightDeltaMax)
        {
            float newMinWeight = float.MaxValue;
            float newMaxWeight = float.MinValue;

            float newMinWeightDelta = float.MaxValue;
            float newMaxWeightDelta = float.MinValue;

            // Compute the bias-corrected learning rate
            float correctedLearningRate = LearningRate * MathF.Sqrt(1f - MathF.Pow(Beta2, epoch)) / (1f - MathF.Pow(Beta1, epoch));

            for (int layerIndex = 1; layerIndex < weights.Length; layerIndex++)
            {
                for (int neuronIndex = 0; neuronIndex < weights[layerIndex].Length; neuronIndex++)
                {
                    for (int weightIndex = 0; weightIndex < weights[layerIndex][neuronIndex].Length; weightIndex++)
                    {
                        // Update moment estimates
                        M[layerIndex][neuronIndex][weightIndex] = Beta1 * M[layerIndex][neuronIndex][weightIndex] + (1f - Beta1) * weightGradients[layerIndex][neuronIndex][weightIndex];
                        V[layerIndex][neuronIndex][weightIndex] = Beta2 * V[layerIndex][neuronIndex][weightIndex] + (1f - Beta2) * weightGradients[layerIndex][neuronIndex][weightIndex] * weightGradients[layerIndex][neuronIndex][weightIndex];

                        // Compute bias-corrected moment estimates
                        float mHat = M[layerIndex][neuronIndex][weightIndex] / (1f - MathF.Pow(Beta1, epoch));
                        float vHat = V[layerIndex][neuronIndex][weightIndex] / (1f - MathF.Pow(Beta2, epoch));

                        // Update weights
                        float originalWeight = weights[layerIndex][neuronIndex][weightIndex];
                        float newWeight = weights[layerIndex][neuronIndex][weightIndex] - correctedLearningRate * mHat / (MathF.Sqrt(vHat) + Epsilon);
                        float weightDelta = newWeight - originalWeight;

                        weights[layerIndex][neuronIndex][weightIndex] = newWeight;

                        if (newWeight < newMinWeight) newMinWeight = newWeight;
                        if (newWeight > newMaxWeight) newMaxWeight = newWeight;
                        if (weightDelta < newMinWeightDelta) newMinWeightDelta = weightDelta;
                        if (weightDelta > newMaxWeightDelta) newMaxWeightDelta = weightDelta;
                    }

                    // Update bias estimates
                    MBias[layerIndex] = Beta1 * MBias[layerIndex] + (1f - Beta1) * biasGradients[layerIndex][neuronIndex];
                    VBias[layerIndex] = Beta2 * VBias[layerIndex] + (1f - Beta2) * biasGradients[layerIndex][neuronIndex] * biasGradients[layerIndex][neuronIndex];

                    // Compute bias-corrected bias estimates
                    float mBiasHat = MBias[layerIndex] / (1f - MathF.Pow(Beta1, epoch));
                    float vBiasHat = VBias[layerIndex] / (1f - MathF.Pow(Beta2, epoch));

                    // Update biases
                    float originalBias = biases[layerIndex][neuronIndex];
                    float newBias = biases[layerIndex][neuronIndex] - correctedLearningRate * mBiasHat / (MathF.Sqrt(vBiasHat) + Epsilon);
                    float biasDelta = newBias - originalBias;

                    biases[layerIndex][neuronIndex] = newBias;

                    if (newBias < newMinWeight) newMinWeight = newBias;
                    if (newBias > newMaxWeight) newMaxWeight = newBias;
                    if (biasDelta < newMinWeightDelta) newMinWeightDelta = biasDelta;
                    if (biasDelta > newMaxWeightDelta) newMaxWeightDelta = biasDelta;
                }
            }

            weightMin = newMinWeight;
            weightMax = newMaxWeight;
            newWeightDeltaMin = newMinWeightDelta;
            newWeightDeltaMax = newMaxWeightDelta;
        }
    }
}
