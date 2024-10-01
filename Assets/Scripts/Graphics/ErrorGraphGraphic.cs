using Assets.Scripts.Controllers;
using Assets.Scripts.Services;
using Assets.Scripts.Utils;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Graphics
{
    /// <summary>
    /// Custom UI graphic component that visualizes error metrics over training epochs in a graph.
    /// </summary>
    public class ErrorGraphGraphic : Graphic
    {
        /// <summary>
        /// Service for scaling error values for visualization.
        /// </summary>
        private readonly ScalerService scalerService = new();

        /// <summary>
        /// Populates the mesh with vertices to render the error graph.
        /// </summary>
        /// <param name="vh">The VertexHelper used to build the mesh.</param>
        protected override void OnPopulateMesh(VertexHelper vh)
        {
            vh.Clear();

            // Calculate the center of the graph
            Vector2 center = new()
            {
                x = -rectTransform.rect.width / 2,
                y = -rectTransform.rect.height / 2,
            };

            int newCertexCount = 0;
            float panelHeight = rectTransform.rect.height;
            float panelWidth = rectTransform.rect.width;

            // Render the graph's boundary and axes
            newCertexCount = VertexUtil.RenderDiagram(vh, newCertexCount, center, panelWidth, panelHeight, Constants.AXIS_LINE_WIDTH, Constants.AXIS_LINE_COLOR);

            // Find and get the NeuralTrainerController
            GameObject trainerObject = GameObject.Find(Constants.TRAINER_OBJECT_NAME);
            NeuralTrainerController neuralTrainerController = trainerObject.GetComponent<NeuralTrainerController>();

            // Exit if the NeuralTrainerController or epoch errors are not available
            if (neuralTrainerController == null || neuralTrainerController.EpochErrors == null || neuralTrainerController.EpochErrors.Count == 0) return;

            // Set up the scaler service with the range of epoch errors
            scalerService.OriginalMin = neuralTrainerController.EpochErrors.Min();
            scalerService.OriginalMax = neuralTrainerController.EpochErrors.Max();
            scalerService.NewMin = center.y;
            scalerService.NewMax = center.y + panelHeight;

            // Scale and render the first error point
            float errorScaled = scalerService.Scale(neuralTrainerController.EpochErrors[0]);
            newCertexCount = VertexUtil.RenderPoligon(vh, newCertexCount, new(center.x, errorScaled), Constants.ERROR_DATA_POINT_RADIUS, 10, Constants.ERROR_LINE_COLOR);

            // Calculate the step size for x-axis based on the number of epochs
            float xStepSize = panelWidth / neuralTrainerController.EpochCount;

            // Render lines and points for subsequent epoch errors
            for (int i = 1; i < neuralTrainerController.EpochErrors.Count; i++)
            {
                errorScaled = scalerService.Scale(neuralTrainerController.EpochErrors[i]);
                float errorScaledPrevious = scalerService.Scale(neuralTrainerController.EpochErrors[i - 1]);
                float x = i * xStepSize + center.x;
                float xPrevious = (i - 1) * xStepSize + center.x;

                // Render line between the previous and current error points
                newCertexCount = VertexUtil.RenderLine(vh, newCertexCount, new(xPrevious, errorScaledPrevious), new(x, errorScaled), 1, Constants.ERROR_LINE_COLOR);

                // Render the current error point
                newCertexCount = VertexUtil.RenderPoligon(vh, newCertexCount, new(x, errorScaled), Constants.ERROR_DATA_POINT_RADIUS, 10, Constants.ERROR_LINE_COLOR);
            }
        }

#pragma warning disable IDE0051 // Remove unused private members
        private void Update()
#pragma warning restore IDE0051 // Remove unused private members
        {
            UpdateGeometry();
        }
    }
}
