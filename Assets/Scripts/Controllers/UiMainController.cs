using Assets.Scripts.Controllers.NeuralNetworkControllers;
using Assets.Scripts.Enums;
using Assets.Scripts.Extensions;
using System;
using System.Linq;
using System.Xml.Linq;
using UnityEngine;
using UnityEngine.UIElements;

namespace Assets.Scripts.Controllers
{
    public class UiMainController : MonoBehaviour
    {
        private NeuralGeneratorController neuralGeneratorController = null;
        private DataGeneratorController dataGeneratorController = null;
        private NeuralTrainerController neuralTrainerController = null;

        private Toggle toggleNetworkNeuronHullVisibility = null;

        private Label labelTrainerCurrentEpoch = null;
        private Label labelTrainerEpochCount = null;
        private Label labelTrainerCurrentIteration = null;
        private Label labelTrainerIterationCount = null;
        private TextField textFieldTrainerLearningRate = null;
        private TextField textFieldTrainerBatchCountPerRender = null;
        private DropdownField dropdownTrainerFieldLossFunction = null;
        private Button buttonTrainerCalculateError = null;
        private Button buttonTrainerBackpropagateErrorUpdateWeights = null;
        private Button buttonTrainerBackpropagate = null;
        private Button buttonTrainerTrain = null;
        private Button buttonTrainerStopTraining = null;

        private TextField textFieldNetworkGeneratorWeightsScaleFrom = null;
        private TextField textFieldNetworkGeneratorWeightsScaleTo = null;
        private Toggle toggleNetworkGeneratorGenerateNetworkConnections = null;
        private TextField textFieldNetworkGeneratorDistanceBetweenNeurons = null;
        private TextField textFieldNetworkGeneratorDistanceBetweenLayers = null;
        private Button buttonNetworkGeneratorGenerate = null;
        private Button buttonNetworkGeneratorClear = null;

#pragma warning disable IDE0051 // Remove unused private members
        private void OnEnable()
#pragma warning restore IDE0051 // Remove unused private members
        {
            UIDocument uiDocument = GetComponent<UIDocument>();

            toggleNetworkNeuronHullVisibility = FindToggleNeuronHullVisibility(uiDocument);
            
            labelTrainerCurrentEpoch = FindLabelCurrentEpoch(uiDocument);
            labelTrainerEpochCount = FindLabelEpochCount(uiDocument);
            labelTrainerCurrentIteration = FindLabelCurrentIteration(uiDocument);
            labelTrainerIterationCount = FindLabelIterationCount(uiDocument);
            textFieldTrainerLearningRate = FindTextFieldLearningRate(uiDocument);
            textFieldTrainerBatchCountPerRender = FindTextFieldBatchCountPerRender(uiDocument);
            dropdownTrainerFieldLossFunction = FindDropdownFieldLossFunction(uiDocument);
            buttonTrainerCalculateError = FindButtonCalculateError(uiDocument);
            buttonTrainerBackpropagateErrorUpdateWeights = FindButtonBackpropagateErrorUpdateWeights(uiDocument);
            buttonTrainerBackpropagate = FindButtonBackpropagate(uiDocument);
            buttonTrainerTrain = FindButtonTrain(uiDocument);
            buttonTrainerStopTraining = FindButtonStopTraining(uiDocument);

            textFieldNetworkGeneratorWeightsScaleFrom = FindTextFieldNetworkGeneratorWeightsScaleFrom(uiDocument);
            textFieldNetworkGeneratorWeightsScaleTo = FindTextFieldNetworkGeneratorWeightsScaleTo(uiDocument);
            toggleNetworkGeneratorGenerateNetworkConnections = FindToggleNetworkGeneratorGenerateNetworkConnections(uiDocument);
            textFieldNetworkGeneratorDistanceBetweenNeurons = FindTextFieldNetworkGeneratorDistanceBetweenNeurons(uiDocument);
            textFieldNetworkGeneratorDistanceBetweenLayers = FindTextFieldNetworkGeneratorDistanceBetweenLayers(uiDocument);
            buttonNetworkGeneratorGenerate = FindButtonGenerateNetwork(uiDocument);
            buttonNetworkGeneratorClear = FindButtonClearNetwork(uiDocument);

            neuralGeneratorController = GameObject.Find(Constants.NETWORK_GENERATOR_OBJECT_NAME).GetComponent<NeuralGeneratorController>();
            dataGeneratorController = GameObject.Find(Constants.DATA_GENERATOR_OBJECT_NAME).GetComponent<DataGeneratorController>();
            neuralTrainerController = GameObject.Find(Constants.TRAINER_OBJECT_NAME).GetComponent<NeuralTrainerController>();

            textFieldTrainerLearningRate.value = neuralTrainerController.LearningRate.ToString();
            textFieldTrainerBatchCountPerRender.value = neuralTrainerController.BatchCountPerRender.ToString();
            dropdownTrainerFieldLossFunction.choices = Enum.GetNames(typeof(LossFunctionType)).Cast<string>().ToList();
            dropdownTrainerFieldLossFunction.value = neuralTrainerController.LossFunction.ToString();
            buttonTrainerCalculateError.RegisterCallback<MouseUpEvent>((evt) => {
                neuralTrainerController.IsCalculateErrorActionPressed = true;
            });
            buttonTrainerBackpropagateErrorUpdateWeights.RegisterCallback<MouseUpEvent>((evt) => {
                neuralTrainerController.IsBackpropagateErrorActionPressed = true;
            });
            buttonTrainerBackpropagate.RegisterCallback<MouseUpEvent>((evt) => {
                neuralTrainerController.IsBackpropagateActionPressed = true;
            });
            buttonTrainerTrain.RegisterCallback<MouseUpEvent>((evt) => {
                neuralTrainerController.IsTrainActionPressed = true;
            });
            buttonTrainerStopTraining.RegisterCallback<MouseUpEvent>((evt) => {
                neuralTrainerController.IsStopTrainingActionPressed = true;
            });


            textFieldNetworkGeneratorWeightsScaleFrom.value = neuralGeneratorController.WeightsMin.ToString();
            textFieldNetworkGeneratorWeightsScaleTo.value = neuralGeneratorController.WeightsMax.ToString();
            toggleNetworkGeneratorGenerateNetworkConnections.value = neuralGeneratorController.GenerateNetworkConnections;
            textFieldNetworkGeneratorDistanceBetweenNeurons.value = neuralGeneratorController.DistanceBetweenNeurons.ToString();
            textFieldNetworkGeneratorDistanceBetweenLayers.value = neuralGeneratorController.DistanceBetweenLayers.ToString();
            buttonNetworkGeneratorGenerate.RegisterCallback<MouseUpEvent>((evt) => {
                neuralGeneratorController.IsGenerateActionPressed = true;
            });
            buttonNetworkGeneratorClear.RegisterCallback<MouseUpEvent>((evt) => {
                neuralGeneratorController.IsClearActionPressed = true;
            });
        }

#pragma warning disable IDE0051 // Remove unused private members
        private void Update()
#pragma warning restore IDE0051 // Remove unused private members
        {
            labelTrainerCurrentEpoch.text = neuralTrainerController.CurrentEpoch.ToString();
            labelTrainerEpochCount.text = neuralTrainerController.EpochCount.ToString();
            labelTrainerCurrentIteration.text = neuralTrainerController.CurrentIteration.ToString();
            labelTrainerIterationCount.text = neuralTrainerController.IterationCount.ToString();
            neuralTrainerController.LearningRate = float.Parse(textFieldTrainerLearningRate.value);
            neuralTrainerController.LossFunction = Enum.Parse<LossFunctionType>(dropdownTrainerFieldLossFunction.value);

            neuralGeneratorController.WeightsMin = float.Parse(textFieldNetworkGeneratorWeightsScaleFrom.value);
            neuralGeneratorController.WeightsMax = float.Parse(textFieldNetworkGeneratorWeightsScaleTo.value);
            neuralGeneratorController.GenerateNetworkConnections = toggleNetworkGeneratorGenerateNetworkConnections.value;
            neuralGeneratorController.DistanceBetweenNeurons = float.Parse(textFieldNetworkGeneratorDistanceBetweenNeurons.value);
            neuralGeneratorController.DistanceBetweenLayers = float.Parse(textFieldNetworkGeneratorDistanceBetweenLayers.value);

            GameObject networkObject = GameObject.Find(Constants.NETWORK_OBJECT_NAME);
            if (networkObject == null) return;
            NeuralNetworkController neuralNetworkController = networkObject.GetComponent<NeuralNetworkController>();
            neuralNetworkController.IsNeuronHullVisible = toggleNetworkNeuronHullVisibility.value;
        }

        private static Toggle FindToggleNeuronHullVisibility(UIDocument uiDocument)
        {
            VisualElement groupBoxTrainerControllerElement = uiDocument.rootVisualElement.FirstRek(
                (visualElement) => visualElement.name == "GroupBoxNetworkController"
            );

            return groupBoxTrainerControllerElement.FirstRek(
                (visualElement) => visualElement.name == "ToggleNeuronHullVisibility"
            ) as Toggle;
        }

        private static Label FindLabelCurrentEpoch(UIDocument uiDocument)
        {
            VisualElement groupBoxTrainerControllerElement = uiDocument.rootVisualElement.FirstRek(
                (visualElement) => visualElement.name == "GroupBoxTrainerController"
            );

            return groupBoxTrainerControllerElement.FirstRek(
                (visualElement) => visualElement.name == "LabelEpochCurrent"
            ) as Label;
        }

        private static Label FindLabelEpochCount(UIDocument uiDocument)
        {
            VisualElement groupBoxTrainerControllerElement = uiDocument.rootVisualElement.FirstRek(
                (visualElement) => visualElement.name == "GroupBoxTrainerController"
            );

            return groupBoxTrainerControllerElement.FirstRek(
                (visualElement) => visualElement.name == "LabelEpochTotal"
            ) as Label;
        }

        private static Label FindLabelCurrentIteration(UIDocument uiDocument)
        {
            VisualElement groupBoxTrainerControllerElement = uiDocument.rootVisualElement.FirstRek(
                (visualElement) => visualElement.name == "GroupBoxTrainerController"
            );

            return groupBoxTrainerControllerElement.FirstRek(
                (visualElement) => visualElement.name == "LabelIterationCurrent"
            ) as Label;
        }

        private static Label FindLabelIterationCount(UIDocument uiDocument)
        {
            VisualElement groupBoxTrainerControllerElement = uiDocument.rootVisualElement.FirstRek(
                (visualElement) => visualElement.name == "GroupBoxTrainerController"
            );

            return groupBoxTrainerControllerElement.FirstRek(
                (visualElement) => visualElement.name == "LabelIterationTotal"
            ) as Label;
        }

        private static TextField FindTextFieldLearningRate(UIDocument uiDocument)
        {
            VisualElement groupBoxTrainerControllerElement = uiDocument.rootVisualElement.FirstRek(
                (visualElement) => visualElement.name == "GroupBoxTrainerController"
            );

            return groupBoxTrainerControllerElement.FirstRek(
                (visualElement) => visualElement.name == "TextFieldLearningRate"
            ) as TextField;
        }

        private static TextField FindTextFieldBatchCountPerRender(UIDocument uiDocument)
        {
            VisualElement groupBoxTrainerControllerElement = uiDocument.rootVisualElement.FirstRek(
                (visualElement) => visualElement.name == "GroupBoxTrainerController"
            );

            return groupBoxTrainerControllerElement.FirstRek(
                (visualElement) => visualElement.name == "TextFieldBatchCountPerRender"
            ) as TextField;
        }

        private static DropdownField FindDropdownFieldLossFunction(UIDocument uiDocument)
        {
            VisualElement groupBoxTrainerControllerElement = uiDocument.rootVisualElement.FirstRek(
                (visualElement) => visualElement.name == "GroupBoxTrainerController"
            );

            return groupBoxTrainerControllerElement.FirstRek(
                (visualElement) => visualElement.name == "DropdownFieldLossFunction"
            ) as DropdownField;
        }

        private static Button FindButtonCalculateError(UIDocument uiDocument)
        {
            VisualElement groupBoxTrainerControllerElement = uiDocument.rootVisualElement.FirstRek(
                (visualElement) => visualElement.name == "GroupBoxTrainerController"
            );

            return groupBoxTrainerControllerElement.FirstRek(
                (visualElement) => visualElement.name == "ButtonCalculateError"
            ) as Button;
        }

        private static Button FindButtonBackpropagateErrorUpdateWeights(UIDocument uiDocument)
        {
            VisualElement groupBoxTrainerControllerElement = uiDocument.rootVisualElement.FirstRek(
                (visualElement) => visualElement.name == "GroupBoxTrainerController"
            );

            return groupBoxTrainerControllerElement.FirstRek(
                (visualElement) => visualElement.name == "ButtonBackpropagateErrorUpdateWeights"
            ) as Button;
        }

        private static Button FindButtonBackpropagate(UIDocument uiDocument)
        {
            VisualElement groupBoxTrainerControllerElement = uiDocument.rootVisualElement.FirstRek(
                (visualElement) => visualElement.name == "GroupBoxTrainerController"
            );

            return groupBoxTrainerControllerElement.FirstRek(
                (visualElement) => visualElement.name == "ButtonBackpropagate"
            ) as Button;
        }

        private static Button FindButtonTrain(UIDocument uiDocument)
        {
            VisualElement groupBoxTrainerControllerElement = uiDocument.rootVisualElement.FirstRek(
                (visualElement) => visualElement.name == "GroupBoxTrainerController"
            );

            return groupBoxTrainerControllerElement.FirstRek(
                (visualElement) => visualElement.name == "ButtonTrain"
            ) as Button;
        }

        private static Button FindButtonStopTraining(UIDocument uiDocument)
        {
            VisualElement groupBoxTrainerControllerElement = uiDocument.rootVisualElement.FirstRek(
                (visualElement) => visualElement.name == "GroupBoxTrainerController"
            );

            return groupBoxTrainerControllerElement.FirstRek(
                (visualElement) => visualElement.name == "ButtonStopTraining"
            ) as Button;
        }

        private static TextField FindTextFieldNetworkGeneratorWeightsScaleFrom(UIDocument uiDocument)
        {
            VisualElement groupBoxTrainerControllerElement = uiDocument.rootVisualElement.FirstRek(
                (visualElement) => visualElement.name == "GroupBoxNetworkGeneratorController"
            );

            return groupBoxTrainerControllerElement.FirstRek(
                (visualElement) => visualElement.name == "TextFieldWeightsScaleFrom"
            ) as TextField;
        }

        private static TextField FindTextFieldNetworkGeneratorWeightsScaleTo(UIDocument uiDocument)
        {
            VisualElement groupBoxTrainerControllerElement = uiDocument.rootVisualElement.FirstRek(
                (visualElement) => visualElement.name == "GroupBoxNetworkGeneratorController"
            );

            return groupBoxTrainerControllerElement.FirstRek(
                (visualElement) => visualElement.name == "TextFieldWeightsScaleTo"
            ) as TextField;
        }

        private static Toggle FindToggleNetworkGeneratorGenerateNetworkConnections(UIDocument uiDocument)
        {
            VisualElement groupBoxTrainerControllerElement = uiDocument.rootVisualElement.FirstRek(
                (visualElement) => visualElement.name == "GroupBoxNetworkGeneratorController"
            );

            return groupBoxTrainerControllerElement.FirstRek(
                (visualElement) => visualElement.name == "ToggleGenerateNetworkConnections"
            ) as Toggle;
        }

        private static TextField FindTextFieldNetworkGeneratorDistanceBetweenNeurons(UIDocument uiDocument)
        {
            VisualElement groupBoxTrainerControllerElement = uiDocument.rootVisualElement.FirstRek(
                (visualElement) => visualElement.name == "GroupBoxNetworkGeneratorController"
            );

            return groupBoxTrainerControllerElement.FirstRek(
                (visualElement) => visualElement.name == "TextFieldDistanceBetweenNeurons"
            ) as TextField;
        }

        private static TextField FindTextFieldNetworkGeneratorDistanceBetweenLayers(UIDocument uiDocument)
        {
            VisualElement groupBoxTrainerControllerElement = uiDocument.rootVisualElement.FirstRek(
                (visualElement) => visualElement.name == "GroupBoxNetworkGeneratorController"
            );

            return groupBoxTrainerControllerElement.FirstRek(
                (visualElement) => visualElement.name == "TextFieldDistanceBetweenLayers"
            ) as TextField;
        }

        private static Button FindButtonGenerateNetwork(UIDocument uiDocument)
        {
            VisualElement groupBoxTrainerControllerElement = uiDocument.rootVisualElement.FirstRek(
                (visualElement) => visualElement.name == "GroupBoxNetworkGeneratorController"
            );

            return groupBoxTrainerControllerElement.FirstRek(
                (visualElement) => visualElement.name == "ButtonGenerate"
            ) as Button;
        }

        private static Button FindButtonClearNetwork(UIDocument uiDocument)
        {
            VisualElement groupBoxTrainerControllerElement = uiDocument.rootVisualElement.FirstRek(
                (visualElement) => visualElement.name == "GroupBoxNetworkGeneratorController"
            );

            return groupBoxTrainerControllerElement.FirstRek(
                (visualElement) => visualElement.name == "ButtonClear"
            ) as Button;
        }
    }
}
