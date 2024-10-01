using Assets.Scripts.Entities;
using Assets.Scripts.Enums;
using Assets.Scripts.Services;
using Assets.Scripts.UiModifications.Attributes;
using Assets.Scripts.UiModifications.PropertyAttributes;
using Assets.Scripts.Utils;
using System;
using UnityEngine;

namespace Assets.Scripts.Controllers.NeuralNetworkControllers
{
    /// <summary>
    /// Controls the weights of neurons in a neural network.
    /// Allows for forward propagation and updating of weight properties.
    /// </summary>
    public class NeuralWeightController : MonoBehaviour
    {
        /// <summary>
        /// Flag to trigger forward propagation action.
        /// Set to true to initiate forward propagation in the next frame.
        /// </summary>
        [Header("Actions")]
#if UNITY_EDITOR
        [DisplayNameProperty("Forward propagate")]
#endif
        public bool IsForwardPropagateActionPressed = false;

        /// <summary>
        /// Gets or sets the weight value of the neuron.
        /// If the neuron is a bias, it gets or sets the bias value.
        /// </summary>
#if UNITY_EDITOR
        [ExposeProperty]
#endif
        public float Weight
        {
            get
            {
                return IsBias ? NetworkComponent.Biases[LayerIndex][NeuronIndex] : NetworkComponent.Weights[LayerIndex][NeuronIndex][WeightIndex];
            }
            set
            {
                if (IsBias)
                {
                    NetworkComponent.Biases[LayerIndex][NeuronIndex] = value;
                }
                else
                {
                    NetworkComponent.Weights[LayerIndex][NeuronIndex][WeightIndex] = value;
                }
            }
        }

#if UNITY_EDITOR
        [ExposeProperty]
#endif
        public TimeSpan WeightColorsUpdateDelay
        {
            get
            {
                return NetworkController == null ? default : NetworkController.WeightColorsUpdateDelay;
            }
            set
            {
                if (NetworkController == null) return;
                NetworkController.WeightColorsUpdateDelay = value;
            }
        }

        private float weightPrevious = float.NaN;
        private DateTime WeightColorsUpdateTimestamp = DateTime.Now;

        /// <summary>
        /// Gets or sets the result of the weight calculation.
        /// Returns NaN if the neuron is a bias.
        /// </summary>
#if UNITY_EDITOR
        [ExposeProperty]
#endif
        public float Result
        {
            get
            {
                return IsBias ? float.NaN : NetworkComponent.WeightResults[LayerIndex][NeuronIndex][WeightIndex];
            }
            set
            {
                if (IsBias) return;

                NetworkComponent.WeightResults[LayerIndex][NeuronIndex][WeightIndex] = value;
            }
        }

        /// <summary>
        /// The previous activation function game object.
        /// </summary>
        public GameObject ActivationFunctionPrevious = null;

        /// <summary>
        /// The neural network component associated with this controller.
        /// </summary>
        internal NeuralNetwork NetworkComponent { get; set; }

        /// <summary>
        /// Indicates if the neuron is a bias.
        /// </summary>
        internal bool IsBias { get; set; }

        /// <summary>
        /// The index of the layer in the neural network.
        /// </summary>
        internal int LayerIndex { get; set; }

        /// <summary>
        /// The index of the neuron in the layer.
        /// </summary>
        internal int NeuronIndex { get; set; }

        /// <summary>
        /// The index of the weight in the neuron.
        /// </summary>
        internal int WeightIndex { get; set; }
        public NeuralNetworkController NetworkController { get; internal set; }

#pragma warning disable IDE0051 // Remove unused private members
        void Start()
#pragma warning restore IDE0051 // Remove unused private members
        {
            weightPrevious = Weight;
        }

        /// <summary>
        /// Unity's Update method, called once per frame.
        /// Checks if forward propagation is triggered, and if so, performs the forward propagation.
        /// </summary>
#pragma warning disable IDE0051 // Remove unused private members
        void Update()
#pragma warning restore IDE0051 // Remove unused private members
        {
            float weightDelta = Weight - weightPrevious;

            // Update the connection colors based on the weight values
            //UpdateConnectionColors(weightDelta);

            if (DateTime.Now - WeightColorsUpdateTimestamp >= WeightColorsUpdateDelay)
            {
                UpdateConnectionColors(weightDelta);
                WeightColorsUpdateTimestamp = DateTime.Now;
            }

            // Check if forward propagation action is triggered
            if (IsForwardPropagateActionPressed)
            {
                // Reset the trigger
                IsForwardPropagateActionPressed = false;
                ForwardPropagate();
            }

            weightPrevious = Weight;
        }

        /// <summary>
        /// Performs forward propagation for the weight in the neural network.
        /// </summary>
        private void ForwardPropagate()
        {
            NetworkComponent.ForwardPropagateWeight(LayerIndex, NeuronIndex, WeightIndex);
        }

        /// <summary>
        /// Updates the colors of the connections based on the weight values.
        /// </summary>
        private void UpdateConnectionColors(float weightDelta)
        {
            UpdateConnectionColor(transform.Find("NeuronConnection"), weightDelta);
            UpdateConnectionColor(transform.Find("SumConnection"), weightDelta);
        }

        /// <summary>
        /// Updates the color of the specified connection line based on the weight value.
        /// </summary>
        /// <param name="transform">The transform of the connection line to update.</param>
        private void UpdateConnectionColor(Transform transform, float weightDelta)
        {
            if (transform == null) return;

            LineRenderer lineRenderer = transform.GetComponent<LineRenderer>();
            if (lineRenderer == null || NetworkComponent == null || NetworkController == null) return;

            Color color;

            if (NetworkController.WeightColorMap == WeightColorMapType.WEIGHTS_STRENGTH)
            {
                color = Color.white;
                ScalerService scalerService = new()
                {
                    OriginalMin = 0,
                    OriginalMax = Mathf.Abs(NetworkComponent.WeightMin)
                };
                float weightMaxAbs = Mathf.Abs(NetworkComponent.WeightMax);
                if (weightMaxAbs > scalerService.OriginalMax)
                    scalerService.OriginalMax = weightMaxAbs;
                scalerService.NewMin = 0;
                scalerService.NewMax = 1;

                color.a = scalerService.Scale(Weight);
            }
            else if (NetworkController.WeightColorMap == WeightColorMapType.WEIGHTS_CHANGE_MOMENTUM)
            {

                Color targetColor;
                float minMaxWeightDelta;

                if (weightDelta < 0)
                {
                    targetColor = Color.red;
                    minMaxWeightDelta = NetworkComponent.WeightDeltaMin;
                }
                else
                {
                    targetColor = Color.green;
                    minMaxWeightDelta = NetworkComponent.WeightDeltaMax;
                }

                color = ColorUtil.Interpolate(new Color(1, 1, 1, 0), targetColor, 0, Mathf.Abs(minMaxWeightDelta), Mathf.Abs(weightDelta));
            }
            else if (NetworkController.WeightColorMap == WeightColorMapType.WEIGHTS_VALUE_GRADIENT)
            {
                color = ColorUtil.Interpolate(Constants.WEIGHT_MIN_COLOR, Constants.WEIGHT_MAX_COLOR, NetworkComponent.WeightMin, NetworkComponent.WeightMax, Weight);
            }
            else if (NetworkController.WeightColorMap == WeightColorMapType.ACTIVATIONS_VALUE_GRADIENT)
            {
                Color targetColor;
                float minMaxActivation;
                float activation = NetworkComponent.Activations[LayerIndex - 1][WeightIndex];

                if (activation < 0)
                {
                    targetColor = Color.red;
                    minMaxActivation = NetworkComponent.ActivationMin;
                }
                else
                {
                    targetColor = Color.green;
                    minMaxActivation = NetworkComponent.ActivationMax;
                }

                color = ColorUtil.Interpolate(new Color(1, 1, 1, 0), targetColor, 0, Mathf.Abs(minMaxActivation), Mathf.Abs(activation));
            }
            else if (NetworkController.WeightColorMap == WeightColorMapType.WHITE)
            {
                color = Color.white;
            }
            else if (NetworkController.WeightColorMap == WeightColorMapType.NONE)
            {
                color = new(0, 0, 0, 0);
            }
            else
            {
                throw new System.Exception("Weight color map type is not supported");
            }

            lineRenderer.startColor = color;
            lineRenderer.endColor = color;
        }
    }
}
