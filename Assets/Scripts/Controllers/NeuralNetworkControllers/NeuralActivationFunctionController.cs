using Assets.Scripts.Entities;
using Assets.Scripts.Enums;
using Assets.Scripts.UiModifications.Attributes;
using Assets.Scripts.UiModifications.PropertyAttributes;
using UnityEngine;

namespace Assets.Scripts.Controllers.NeuralNetworkControllers
{
    /// <summary>
    /// Controls the behavior and properties of an individual neuron's activation function within a neural network.
    /// Manages forward propagation for the activation function and provides access to the activation function type and activation value.
    /// </summary>
    public class NeuralActivationFunctionController : MonoBehaviour
    {
        /// <summary>
        /// Flag to trigger forward propagation action.
        /// Set to true to initiate forward propagation of the activation function in the next frame.
        /// </summary>
        [Header("Actions")]
#if UNITY_EDITOR
        [DisplayNameProperty("Forward propagate")]
#endif
        public bool IsForwardPropagateActionPressed = false;

        /// <summary>
        /// Gets or sets the activation function type for this neuron.
        /// </summary>
#if UNITY_EDITOR
        [ExposeProperty]
#endif
        public ActivationFunctionType ActivationFunction
        {
            get
            {
                return NetworkComponent.ActivationFunctions[LayerIndex][NeuronIndex];
            }
            set
            {
                NetworkComponent.ActivationFunctions[LayerIndex][NeuronIndex] = value;
            }

        }

        /// <summary>
        /// Gets or sets the activation value for this neuron.
        /// </summary>
#if UNITY_EDITOR
        [ExposeProperty]
#endif
        public float Activation
        {
            get
            {
                return NetworkComponent.Activations[LayerIndex][NeuronIndex];
            }
            set
            {
                NetworkComponent.Activations[LayerIndex][NeuronIndex] = value;
            }

        }

        /// <summary>
        /// The neural network component associated with this activation function controller.
        /// </summary>
        internal NeuralNetwork NetworkComponent { get; set; }

        /// <summary>
        /// The index of the layer within the neural network to which this neuron belongs.
        /// </summary>
        internal int LayerIndex { get; set; }

        /// <summary>
        /// The index of the neuron within the layer.
        /// </summary>
        internal int NeuronIndex { get; set; }

        /// <summary>
        /// Unity's Update method, called once per frame.
        /// Checks if forward propagation action is triggered and performs forward propagation for the activation function.
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
        /// Performs forward propagation for this neuron's activation function.
        /// </summary>
        private void ForwardPropagate()
        {
            NetworkComponent.ForwardPropagateActivationFunction(LayerIndex, NeuronIndex);
        }
    }
}
