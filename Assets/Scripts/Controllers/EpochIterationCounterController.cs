using TMPro;
using UnityEngine;

namespace Assets.Scripts.Controllers
{
    /// <summary>
    /// Updates a UI text element to display the current epoch and iteration counts of the neural network training process.
    /// </summary>
    public class EpochIterationCounterController : MonoBehaviour
    {
        /// <summary>
        /// Reference to the TextMeshProUGUI component used to display the epoch and iteration information.
        /// </summary>
        private TextMeshProUGUI label = null;

        /// <summary>
        /// Initializes the label component on start.
        /// </summary>
#pragma warning disable IDE0051 // Remove unused private members
        private void Start()
#pragma warning restore IDE0051 // Remove unused private members
        {
            // Get the TextMeshProUGUI component attached to the GameObject
            label = gameObject.GetComponent<TextMeshProUGUI>();
        }

#pragma warning disable IDE0051 // Remove unused private members
        private void Update()
#pragma warning restore IDE0051 // Remove unused private members
        {
            // Find the NeuralTrainerController object in the scene
            GameObject trainerObject = GameObject.Find(Constants.TRAINER_OBJECT_NAME);
            NeuralTrainerController neuralTrainerController = trainerObject.GetComponent<NeuralTrainerController>();
            // Update the label text with the current epoch and iteration counts
            label.text = $"Epoch {neuralTrainerController.CurrentEpoch + 1}/{neuralTrainerController.EpochCount} Iteration {neuralTrainerController.CurrentIteration + 1}/{neuralTrainerController.IterationCount}";
        }
    }
}
