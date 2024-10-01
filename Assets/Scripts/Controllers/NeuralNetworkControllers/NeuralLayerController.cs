using Assets.Scripts.Entities;
using Assets.Scripts.Enums;
using Assets.Scripts.UiModifications.Attributes;
using Assets.Scripts.UiModifications.PropertyAttributes;
using UnityEngine;

namespace Assets.Scripts.Controllers.NeuralNetworkControllers
{
    /// <summary>
    /// Controls the behavior and properties of a neural network layer.
    /// Manages forward propagation for the layer and provides access to various properties such as activations, biases, sum results, activation functions, and errors.
    /// </summary>
    public class NeuralLayerController : MonoBehaviour
    {
        /// <summary>
        /// Indicates if the neuron is in the output or hidden layer.
        /// </summary>
        [HideInInspector()]
        public bool IsOutputOrHiddenLayerNeuron = false;

        /// <summary>
        /// Flag to trigger forward propagation action.
        /// Set to true to initiate forward propagation in the next frame.
        /// </summary>
        [Header("Actions")]
#if UNITY_EDITOR
        [DisplayNameProperty("Forward propagate")]
        [ConditionalHideProperty(nameof(IsOutputOrHiddenLayerNeuron), true)]
#endif
        public bool IsForwardPropagateActionPressed = false;

        /// <summary>
        /// Gets or sets the activation values of the layer.
        /// </summary>
#if UNITY_EDITOR
        [ExposeProperty]
#endif
        public float[] Activations
        {
            get
            {
                return NetworkComponent.Activations[LayerIndex];
            }
            set
            {
                NetworkComponent.Activations[LayerIndex] = value;
            }
        }

        /// <summary>
        /// Gets or sets the bias values of the layer.
        /// </summary>
#if UNITY_EDITOR
        [ExposeProperty]
#endif
        public float[] Biases
        {
            get
            {
                return NetworkComponent.Biases[LayerIndex];
            }
            set
            {
                NetworkComponent.Biases[LayerIndex] = value;
            }
        }

        /// <summary>
        /// Gets or sets the sum result values of the layer.
        /// </summary>
#if UNITY_EDITOR
        [ExposeProperty]
#endif
        public float[] SumResults
        {
            get
            {
                return NetworkComponent.SumResults[LayerIndex];
            }
            set
            {
                NetworkComponent.SumResults[LayerIndex] = value;
            }
        }

        /// <summary>
        /// Gets or sets the activation function type for the layer.
        /// </summary>
#if UNITY_EDITOR
        [ExposeProperty]
#endif
        public ActivationFunctionType ActivationFunction
        {
            get
            {
                if (NetworkComponent.ActivationFunctions[LayerIndex] == null)
                {
                    return ActivationFunctionType.RE_LU;
                }
                return NetworkComponent.ActivationFunctions[LayerIndex][0];
            }
            set
            {
                if (NetworkComponent.ActivationFunctions[LayerIndex] == null) return;
                for (int neuronIndex = 0; neuronIndex < NetworkComponent.ActivationFunctions[LayerIndex].Length; neuronIndex++)
                {
                    NetworkComponent.ActivationFunctions[LayerIndex][neuronIndex] = value;
                }
            }
        }

        /// <summary>
        /// Gets or sets the activation function types for each neuron in the layer.
        /// </summary>
#if UNITY_EDITOR
        [ExposeProperty]
#endif
        public ActivationFunctionType[] ActivationFunctions
        {
            get
            {
                return NetworkComponent.ActivationFunctions[LayerIndex];
            }
            set
            {
                NetworkComponent.ActivationFunctions[LayerIndex] = value;
            }
        }

        /// <summary>
        /// Gets or sets the error values of the layer.
        /// </summary>
#if UNITY_EDITOR
        [ExposeProperty]
#endif
        public float[] Errors
        {
            get
            {
                return NetworkComponent.Errors[LayerIndex];
            }
            set
            {
                NetworkComponent.Errors[LayerIndex] = value;
            }
        }

        /// <summary>
        /// The neural network component associated with this layer controller.
        /// </summary>
        internal NeuralNetwork NetworkComponent { get; set; }

        /// <summary>
        /// The index of this layer within the neural network.
        /// </summary>
        internal int LayerIndex { get; set; }

        /// <summary>
        /// Unity's Update method, called once per frame.
        /// Checks if forward propagation action is triggered and performs forward propagation for the layer.
        /// </summary>
#pragma warning disable IDE0051 // Remove unused private members
        void Update()
#pragma warning restore IDE0051 // Remove unused private members
        {
            if (IsForwardPropagateActionPressed)
            {
                IsForwardPropagateActionPressed = false;
                ForwardPropagate();
            }
        }

        /// <summary>
        /// Performs forward propagation for this layer.
        /// </summary>
        private void ForwardPropagate()
        {
            NetworkComponent.ForwardPropagateLayer(LayerIndex);
        }
    }
}
