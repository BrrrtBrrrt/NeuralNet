using Assets.Scripts.Controllers.NeuralNetworkControllers;
using Assets.Scripts.Entities;
using Assets.Scripts.Types;
using Assets.Scripts.UiModifications.PropertyAttributes;
using System;
using System.Collections;
using UnityEngine;

namespace Assets.Scripts.Controllers
{
    /// <summary>
    /// Manages the creation and visualization of neural networks within the Unity environment.
    /// Provides functionality to generate and clear neural networks, and configure their visual representation.
    /// </summary>
    public class NeuralGeneratorController : MonoBehaviour
    {
        [Header("Actions")]
        /// <summary>
        /// Flag indicating whether the Generate action has been triggered.
        /// </summary>
#if UNITY_EDITOR
        [DisplayNameProperty("Generate")]
#endif
        public bool IsGenerateActionPressed = false;

        /// <summary>
        /// Flag indicating whether the Clear action has been triggered.
        /// </summary>
#if UNITY_EDITOR
        [DisplayNameProperty("Clear")]
#endif
        public bool IsClearActionPressed = false;

        [Header("Network settings")]
        /// <summary>
        /// Minimum value for the weights in the neural network.
        /// </summary>
        public float WeightsMin = -0.01f;

        /// <summary>
        /// Maximum value for the weights in the neural network.
        /// </summary>
        public float WeightsMax = 0.01f;

        /// <summary>
        /// Configuration for the layers of the neural network.
        /// </summary>
        public LayerConfig[] LayerConfigs = new LayerConfig[0];

        [Header("Display settings")]
        /// <summary>
        /// Flag indicating whether the Generate action has been triggered.
        /// </summary>
        public bool GenerateNetworkConnections = true;

        /// <summary>
        /// Distance between neurons in the same layer.
        /// </summary>
        public float DistanceBetweenNeurons = 5;

        /// <summary>
        /// Distance between different layers of neurons.
        /// </summary>
        public float DistanceBetweenLayers = 100;

        /// <summary>
        /// Memory storage for temporary data during network generation.
        /// </summary>
        private NeuralGeneratorMemory memory = new();

        // Update is called once per frame
#pragma warning disable IDE0051 // Remove unused private members
        void Update()
#pragma warning restore IDE0051 // Remove unused private members
        {
            if (IsGenerateActionPressed)
            {
                IsGenerateActionPressed = false;
                Clear();
                StartCoroutine(Generate());
            }

            if (IsClearActionPressed)
            {
                IsClearActionPressed = false;
                Clear();
            }
        }

        /// <summary>
        /// Coroutine to generate the neural network and its visualization.
        /// </summary>
        /// <returns>An IEnumerator for the coroutine.</returns>
        private IEnumerator Generate()
        {
            // Initialize memory and generate the network
            memory = new();
            NeuralNetwork network = NeuralGenerator.Generate(LayerConfigs, WeightsMin, WeightsMax);
            Debug.Log(network.ToString());

            // Create the network object in the scene
            yield return GenerateNetworkObject(network);

            // Find and configure the NeuralTrainerController
            GameObject trainerObject = GameObject.Find(Constants.TRAINER_OBJECT_NAME);
            NeuralTrainerController neuralTrainerController = trainerObject.GetComponent<NeuralTrainerController>();
            neuralTrainerController.Network = network;
            neuralTrainerController.OutputExpected = new float[LayerConfigs[^1].NeuronCount];

            // Set the name of the generator object and complete the coroutine. Nessecary beacause Unity overwrites the attached game object name while generating other objects.
            name = Constants.NETWORK_GENERATOR_OBJECT_NAME;
            yield return null;

        }

        /// <summary>
        /// Creates the GameObject representation of the neural network and its layers.
        /// </summary>
        /// <param name="network">The neural network to be visualized.</param>
        /// <returns>An IEnumerator for the coroutine.</returns>
        private IEnumerator GenerateNetworkObject(NeuralNetwork network)
        {
            GameObject networkObject = new(
                name = "Network"
            );
            NeuralNetworkController neuralNetworkController = networkObject.AddComponent<NeuralNetworkController>();
            neuralNetworkController.NetworkComponent = network;
            neuralNetworkController.NetworkComponentBackup = network.Clone();

            // Generate layer objects for each layer in the network
            for (int layerIndex = 0; layerIndex < network.Activations.Length; layerIndex++)
            {
                yield return GenerateLayerObject(networkObject, network, layerIndex, neuralNetworkController);
            }

            // Set camera to view the generated network
            GameObject mainCameraObject = GameObject.FindWithTag("MainCamera");
            Camera mainCamera = mainCameraObject.GetComponent<Camera>();
            SetCameraToViewTarget(mainCamera, networkObject.transform);
        }

        /// <summary>
        /// Creates the GameObject representation of a specific layer in the neural network.
        /// </summary>
        /// <param name="parent">The parent GameObject to attach the layer to.</param>
        /// <param name="network">The neural network containing the layer.</param>
        /// <param name="layerIndex">The index of the layer to be generated.</param>
        /// <returns>An IEnumerator for the coroutine.</returns>
        private IEnumerator GenerateLayerObject(GameObject parent, NeuralNetwork network, int layerIndex, NeuralNetworkController neuralNetworkController)
        {
            GameObject layerObject = new(
                name = $"Layer{layerIndex}"
            );
            NeuralLayerController neuralLayerController = layerObject.AddComponent<NeuralLayerController>();
            neuralLayerController.NetworkComponent = network;
            neuralLayerController.LayerIndex = layerIndex;
            neuralLayerController.IsOutputOrHiddenLayerNeuron = layerIndex > 0;

            layerObject.transform.parent = parent.transform;
            layerObject.transform.localPosition = new(layerIndex * DistanceBetweenLayers, (network.Activations[layerIndex].Length - 1) * DistanceBetweenNeurons / 2, 0);

            // Generate neuron objects for each neuron in the layer
            for (int neuronIndex = 0; neuronIndex < network.Activations[layerIndex].Length; neuronIndex++)
            {
                yield return GenerateNeuronObject(layerObject, network, layerIndex, neuronIndex, neuralNetworkController);
            }

            // Store the reference to the current layer object for later use
            memory.LayerPreviousObject = layerObject;
        }

        /// <summary>
        /// Creates the GameObject representation of a specific neuron in a layer.
        /// </summary>
        /// <param name="parent">The parent GameObject to attach the neuron to.</param>
        /// <param name="network">The neural network containing the neuron.</param>
        /// <param name="layerIndex">The index of the layer containing the neuron.</param>
        /// <param name="neuronIndex">The index of the neuron to be generated.</param>
        /// <returns>An IEnumerator for the coroutine.</returns>
        private IEnumerator GenerateNeuronObject(GameObject parent, NeuralNetwork network, int layerIndex, int neuronIndex, NeuralNetworkController neuralNetworkController)
        {
            GameObject neuronObject = GameObject.CreatePrimitive(PrimitiveType.Cube);
            neuronObject.name = $"Neuron{neuronIndex}";
            NeuralNeuronController neuralNeuronController = neuronObject.AddComponent<NeuralNeuronController>();
            neuralNeuronController.NetworkComponent = network;
            neuralNeuronController.LayerIndex = layerIndex;
            neuralNeuronController.NeuronIndex = neuronIndex;
            neuralNeuronController.IsOutputOrHiddenLayerNeuron = layerIndex > 0;
            neuronObject.transform.parent = parent.transform;
            neuronObject.transform.localPosition = new(0, -neuronIndex * DistanceBetweenNeurons, 0);

            // Generate additional objects such as weights, biases, etc.
            if (layerIndex > 0)
            {
                GenerateSumObject(neuronObject, network, layerIndex, neuronIndex);

                GenerateBiasObject(neuronObject, network, layerIndex, neuronIndex, neuralNetworkController);

                GenerateWeightsObject(neuronObject, network, layerIndex, neuronIndex, neuralNetworkController);
            }

            GenerateActivationFunctionObject(neuronObject, network, layerIndex, neuronIndex);
            yield return null;
        }


        /// <summary>
        /// Creates and configures weight objects for a specific neuron.
        /// </summary>
        /// <param name="parent">The parent GameObject to attach the weights to.</param>
        /// <param name="network">The neural network containing the weights.</param>
        /// <param name="layerIndex">The index of the layer containing the neuron.</param>
        /// <param name="neuronIndex">The index of the neuron for which weights are created.</param>
        private void GenerateWeightsObject(GameObject parent, NeuralNetwork network, int layerIndex, int neuronIndex, NeuralNetworkController neuralNetworkController)
        {
            GameObject weightsObject = new(
                name = "Weights"
            );
            weightsObject.transform.parent = parent.transform;
            weightsObject.transform.localPosition = new(0, 0, 0);

            int neuronWeightsCount = network.Activations[layerIndex - 1].Length;

            float weightsDistance = 1f / (neuronWeightsCount + 1);

            for (int weightIndex = 0; weightIndex < neuronWeightsCount; weightIndex++)
            {
                GenerateWeightObject(weightsObject, layerIndex, neuronIndex, weightIndex, network, weightsDistance, neuralNetworkController);
            }
        }


        /// <summary>
        /// Creates and configures a weight object for a specific weight.
        /// </summary>
        /// <param name="parent">The parent GameObject to attach the weight to.</param>
        /// <param name="layerIndex">The index of the layer containing the weight.</param>
        /// <param name="neuronIndex">The index of the neuron containing the weight.</param>
        /// <param name="weightIndex">The index of the weight.</param>
        /// <param name="network">The neural network containing the weight.</param>
        /// <param name="weightsDistance">Distance between weight objects.</param>
        private void GenerateWeightObject(GameObject parent, int layerIndex, int neuronIndex, int weightIndex, NeuralNetwork network, float weightsDistance, NeuralNetworkController neuralNetworkController)
        {
            GameObject weightObject = GameObject.CreatePrimitive(PrimitiveType.Cube);
            weightObject.name = $"Weight{weightIndex}";
            NeuralWeightController neuralWeightController = weightObject.AddComponent<NeuralWeightController>();
            neuralWeightController.NetworkController = neuralNetworkController;
            neuralWeightController.NetworkComponent = network;
            neuralWeightController.IsBias = false;
            neuralWeightController.LayerIndex = layerIndex;
            neuralWeightController.NeuronIndex = neuronIndex;
            neuralWeightController.WeightIndex = weightIndex;
            weightObject.transform.parent = parent.transform;
            weightObject.transform.localPosition = new(-0.55f, weightIndex * weightsDistance * -1 - weightsDistance + 0.5f, 0);
            weightObject.transform.localScale = new(0.1f, 0.1f, 0.1f);
            GenerateSumConnectionObject(weightObject);
            GameObject layerPreviousObject = memory.LayerPreviousObject;
            GameObject neuronPreviousObject = layerPreviousObject.transform.GetChild(weightIndex).gameObject;
            GameObject activationFunctionPreviousObject = neuronPreviousObject.transform.Find("ActivationFunction").gameObject;
            neuralWeightController.ActivationFunctionPrevious = activationFunctionPreviousObject;
            GenerateNeuronConnectionObject(weightObject, activationFunctionPreviousObject.transform.position);
        }

        /// <summary>
        /// Creates a connection object to visualize the connection between neurons.
        /// </summary>
        /// <param name="parent">The parent GameObject to attach the connection to.</param>
        /// <param name="activationFunctionPreviousPosition">The position of the previous activation function object.</param>
        private void GenerateNeuronConnectionObject(GameObject parent, Vector3 activationFunctionPreviousPosition)
        {
            GameObject connectionObject = new()
            {
                name = "NeuronConnection",
            };
            connectionObject.transform.parent = parent.transform;
            connectionObject.transform.localPosition = new(0, 0, 0);
            if (GenerateNetworkConnections)
            {
                LineRenderer lineRenderer = connectionObject.AddComponent<LineRenderer>();
                lineRenderer.widthCurve = new(new Keyframe[] {
                    new(0, Constants.NEURON_CONNECTION_LINE_WIDTH),
                });
                lineRenderer.useWorldSpace = false;
                lineRenderer.SetPositions(new[]
                {
                    new Vector3(0, 0, 0),
                    connectionObject.transform.InverseTransformPoint(activationFunctionPreviousPosition),
                });
                lineRenderer.material = new(Shader.Find("Legacy Shaders/Particles/Alpha Blended Premultiply"));
            }
        }

        /// <summary>
        /// Creates a bias object for a specific neuron.
        /// </summary>
        /// <param name="parent">The parent GameObject to attach the bias to.</param>
        /// <param name="network">The neural network containing the bias.</param>
        /// <param name="layerIndex">The index of the layer containing the bias.</param>
        /// <param name="neuronIndex">The index of the neuron containing the bias.</param>
        private void GenerateBiasObject(GameObject parent, NeuralNetwork network, int layerIndex, int neuronIndex, NeuralNetworkController neuralNetworkController)
        {
            GameObject biasObject = GameObject.CreatePrimitive(PrimitiveType.Cube);
            biasObject.name = "Bias";
            NeuralWeightController neuralWeightController = biasObject.AddComponent<NeuralWeightController>();
            neuralWeightController.NetworkComponent = network;
            neuralWeightController.NetworkController = neuralNetworkController;
            neuralWeightController.IsBias = true;
            neuralWeightController.LayerIndex = layerIndex;
            neuralWeightController.NeuronIndex = neuronIndex;

            biasObject.transform.parent = parent.transform;
            biasObject.transform.localPosition = new(0, 0.55f, 0);
            biasObject.transform.localScale = new(0.1f, 0.1f, 0.1f);
            GenerateSumConnectionObject(biasObject);
        }

        /// <summary>
        /// Creates a sum object for a specific neuron.
        /// </summary>
        /// <param name="parent">The parent GameObject to attach the sum to.</param>
        /// <param name="network">The neural network containing the sum.</param>
        /// <param name="layerIndex">The index of the layer containing the sum.</param>
        /// <param name="neuronIndex">The index of the neuron containing the sum.</param>
        private void GenerateSumObject(GameObject parent, NeuralNetwork network, int layerIndex, int neuronIndex)
        {
            GameObject sumObject = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            sumObject.name = "Sum";
            NeuralSumController neuralSumController = sumObject.AddComponent<NeuralSumController>();
            neuralSumController.NetworkComponent = network;
            neuralSumController.LayerIndex = layerIndex;
            neuralSumController.NeuronIndex = neuronIndex;
            sumObject.transform.parent = parent.transform;
            sumObject.transform.localPosition = new(0, 0, 0);
            sumObject.transform.localScale = new(0.2f, 0.2f, 0.2f);
            memory.SumObject = sumObject;
        }

        /// <summary>
        /// Creates an activation function object for a specific neuron.
        /// </summary>
        /// <param name="parent">The parent GameObject to attach the activation function to.</param>
        /// <param name="network">The neural network containing the activation function.</param>
        /// <param name="layerIndex">The index of the layer containing the activation function.</param>
        /// <param name="neuronIndex">The index of the neuron containing the activation function.</param>
        private void GenerateActivationFunctionObject(GameObject parent, NeuralNetwork network, int layerIndex, int neuronIndex)
        {
            GameObject activationFunctionObject = GameObject.CreatePrimitive(PrimitiveType.Cube);
            activationFunctionObject.name = "ActivationFunction";
            activationFunctionObject.transform.parent = parent.transform;
            activationFunctionObject.transform.localPosition = new(0.6f, 0, 0);
            activationFunctionObject.transform.localScale = new(0.2f, 0.2f, 0.2f);

            if (layerIndex > 0)
            {
                NeuralActivationFunctionController neuralActivationFunctionController = activationFunctionObject.AddComponent<NeuralActivationFunctionController>();
                neuralActivationFunctionController.NetworkComponent = network;
                neuralActivationFunctionController.LayerIndex = layerIndex;
                neuralActivationFunctionController.NeuronIndex = neuronIndex;
                GenerateSumConnectionObject(activationFunctionObject);
            }
        }

        /// <summary>
        /// Creates a connection object to visualize the connection between sum objects.
        /// </summary>
        /// <param name="parent">The parent GameObject to attach the connection to.</param>
        private void GenerateSumConnectionObject(GameObject parent)
        {
            GameObject connectionObject = new()
            {
                name = "SumConnection",
            };
            connectionObject.transform.parent = parent.transform;
            connectionObject.transform.localPosition = new(0, 0, 0);
            if (GenerateNetworkConnections)
            {
                LineRenderer lineRenderer = connectionObject.AddComponent<LineRenderer>();
                lineRenderer.widthCurve = new(new Keyframe[] {
                new(0, Constants.NEURON_CONNECTION_LINE_WIDTH),
            });
                lineRenderer.useWorldSpace = false;
                lineRenderer.SetPositions(new[]
                {
                new Vector3(0, 0, 0),
                connectionObject.transform.InverseTransformPoint(memory.SumObject.transform.position),
            });
                lineRenderer.material = new(Shader.Find("Legacy Shaders/Particles/Alpha Blended Premultiply"));
            }
        }

        /// <summary>
        /// Removes the current network object from the scene.
        /// </summary>
        private void Clear()
        {
            GameObject networkObject = GameObject.Find(Constants.NETWORK_OBJECT_NAME);
            if (networkObject == null) return;
            Destroy(networkObject);
        }

        /// <summary>
        /// Configures the camera to view the target object, adjusting its position and orientation.
        /// </summary>
        /// <param name="targetCamera">The camera to be positioned.</param>
        /// <param name="targetObject">The object that the camera should focus on.</param>
        private void SetCameraToViewTarget(Camera targetCamera, Transform targetObject)
        {
            if (targetCamera == null || targetObject == null)
            {
                Debug.LogError("Target Camera or Target Object is not set.");
                return;
            }

            // Calculate the bounds of the target object including all its children
            Bounds bounds = GetBounds(targetObject);

            // Calculate the distance needed to fit the bounds within the camera's view
            float distance = CalculateCameraDistance(bounds, targetCamera);

            // Position the camera in front of the target object
            Vector3 cameraDirection = targetCamera.transform.forward;
            targetCamera.transform.position = bounds.center - cameraDirection * distance;

            // Look at the target object
            targetCamera.transform.LookAt(bounds.center);
        }

        /// <summary>
        /// Calculates the bounding box of the target object including all its children.
        /// </summary>
        /// <param name="target">The target Transform to calculate bounds for.</param>
        /// <returns>The calculated bounds of the target object.</returns>
        private Bounds GetBounds(Transform target)
        {
            Bounds bounds = new(target.position, Vector3.zero);
            Renderer[] renderers = target.GetComponentsInChildren<Renderer>();

            foreach (Renderer renderer in renderers)
            {
                bounds.Encapsulate(renderer.bounds);
            }

            return bounds;
        }

        /// <summary>
        /// Calculates the distance from the camera needed to view the target object.
        /// </summary>
        /// <param name="bounds">The bounds of the target object.</param>
        /// <param name="camera">The camera to calculate distance for.</param>
        /// <returns>The calculated distance from the camera.</returns>
        private float CalculateCameraDistance(Bounds bounds, Camera camera)
        {
            float frustumHeight = bounds.size.y;
            float frustumWidth = bounds.size.x;

            if (camera.aspect > 1.0f)
            {
                frustumHeight = frustumWidth / camera.aspect;
            }

            float distance = frustumHeight / (2.0f * Mathf.Tan(0.5f * Mathf.Deg2Rad * camera.fieldOfView));

            // Add some margin to ensure the object is fully visible
            distance += bounds.extents.magnitude;

            return distance;
        }
    }

    /// <summary>
    /// Stores memory for neural network generation within the scene, including references to previous layers and positions of key objects.
    /// </summary>
    internal class NeuralGeneratorMemory
    {
        /// <summary>
        /// Gets or sets the GameObject representing the previous layer in the neural network.
        /// This reference is used to establish connections and relationships between layers in the network visualization.
        /// </summary>
        public GameObject LayerPreviousObject { get; set; }

        /// <summary>
        /// Gets or sets the position of the sum object in the scene.
        /// The sum object is a visual representation of the summation operation performed in a neuron.
        /// This position is used to create connections to other objects, such as weights and biases, in the neural network visualization.
        /// </summary>
        public GameObject SumObject { get; set; }
    }
}