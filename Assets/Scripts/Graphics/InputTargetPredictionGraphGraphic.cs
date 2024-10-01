using Assets.Scripts.Controllers;
using Assets.Scripts.Services;
using Assets.Scripts.Types;
using Assets.Scripts.Utils;
using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Graphics
{
    /// <summary>
    /// Custom UI graphic component that visualizes training and test data points, as well as predictions, in a graph.
    /// </summary>
    public class InputTargetPredictionGraphGraphic : Graphic
    {
        /// <summary>
        /// Determines if the original order of data points should be used.
        /// </summary>
        public bool IsOriginalOrder = false;

        /// <summary>
        /// Service for scaling data points for visualization.
        /// </summary>
        private readonly ScalerService scalerService = new();

        /// <summary>
        /// Populates the mesh with vertices to render the graph.
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

            // Find and get the DataGeneratorController
            GameObject dataGeneratorObject = GameObject.Find("DataGenerator");
            if (dataGeneratorObject == null) return;

            DataGeneratorController dataGeneratorController = dataGeneratorObject.GetComponent<DataGeneratorController>();
            if (dataGeneratorController == null) return;

            // Determine which data to use based on the IsOriginalOrder flag
            XYData dataTrain;
            XYData dataTest;
            if (IsOriginalOrder)
            {
                dataTrain = dataGeneratorController.DataTrainingPreparedOriginalOrder;
                dataTest = dataGeneratorController.DataTestPreparedOriginalOrder;
            }
            else
            {
                dataTrain = dataGeneratorController.DataTrainingPrepared;
                dataTest = dataGeneratorController.DataTestPrepared;
            }

            // Combine training and test data
            XYData data = new();
            data.XY.AddRange(dataTrain.XY);
            data.XY.AddRange(dataTest.XY);

            if (data.XY.Count == 0) return;

            // Set up the scaler service
            scalerService.OriginalMin = dataGeneratorController.ScaleMin;
            scalerService.OriginalMax = dataGeneratorController.ScaleMax;
            scalerService.NewMin = center.y;
            scalerService.NewMax = center.y + panelHeight;

            float xStepSize = panelWidth / data.XY.Count;

            // Render data points
            for (int dataIndex = 0; dataIndex < data.XY.Count; dataIndex++)
            {
                XYDataEntry xyDataEntry = data.XY[dataIndex];
                float yScaled = scalerService.Scale(xyDataEntry.Y[0]);
                newCertexCount = VertexUtil.RenderPoligon(vh, newCertexCount, new(center.x + (dataIndex * xStepSize), yScaled), Constants.DATA_POINT_RADIUS, 10, Constants.DATA_POINT_TARGET_COLOR);
            }

            // Render the line separating training and test data
            float trainTestLineOffset = dataTrain.XY.Count * xStepSize;
            newCertexCount = VertexUtil.RenderLine(vh, newCertexCount, new(center.x + trainTestLineOffset, center.y), new(center.x + trainTestLineOffset, center.y + panelHeight), Constants.AXIS_LINE_WIDTH, Constants.AXIS_LINE_COLOR);

            // Find and get the NeuralTrainerController
            GameObject trainerObject = GameObject.Find(Constants.TRAINER_OBJECT_NAME);
            NeuralTrainerController neuralTrainerController = trainerObject.GetComponent<NeuralTrainerController>();

            // If no predictions, render the diagram and exit
            if (neuralTrainerController.EpochTrainingPredictions == null)
            {
                newCertexCount = VertexUtil.RenderDiagram(vh, newCertexCount, center, panelWidth, panelHeight, Constants.AXIS_LINE_WIDTH, Constants.AXIS_LINE_COLOR);
                return;
            }

            // Render training predictions
            for (int dataIndex = 0; dataIndex < neuralTrainerController.EpochTrainingPredictions.XY.Count; dataIndex++)
            {
                XYDataEntry xyDataEntry = neuralTrainerController.EpochTrainingPredictions.XY[dataIndex];
                float yPredicted = scalerService.Scale(xyDataEntry.Y[0]);
                int index = dataIndex;
                if (IsOriginalOrder)
                {
                    index = dataGeneratorController.DataTrainingPreparedOriginalOrder.XY.FindIndex((xyDataEntryOriginalOrder) =>
                    {
                        return xyDataEntryOriginalOrder.X[0] == xyDataEntry.X[0];
                    });
                }
                float x = center.x + (index * xStepSize);
                newCertexCount = VertexUtil.RenderPoligon(vh, newCertexCount, new(x, yPredicted), Constants.DATA_POINT_RADIUS, 10, Constants.DATA_POINT_PREDICTION_COLOR);
            }

            // Render test predictions
            for (int dataIndex = 0; dataIndex < neuralTrainerController.EpochTestPredictions.XY.Count; dataIndex++)
            {
                XYDataEntry xyDataEntry = neuralTrainerController.EpochTestPredictions.XY[dataIndex];
                float yPredicted = scalerService.Scale(xyDataEntry.Y[0]);
                int index = neuralTrainerController.EpochTrainingPredictions.XY.Count + dataIndex;
                if (IsOriginalOrder)
                {
                    index = dataGeneratorController.DataTestPreparedOriginalOrder.XY.FindIndex((xyDataEntryOriginalOrder) =>
                    {
                        return xyDataEntryOriginalOrder.X[0] == xyDataEntry.X[0];
                    });
                    index += neuralTrainerController.EpochTrainingPredictions.XY.Count;
                }
                float x = center.x + (index * xStepSize);
                newCertexCount = VertexUtil.RenderPoligon(vh, newCertexCount, new(x, yPredicted), Constants.DATA_POINT_RADIUS, 10, Constants.DATA_POINT_PREDICTION_COLOR);
            }

            // Render the diagram
            newCertexCount = VertexUtil.RenderDiagram(vh, newCertexCount, center, panelWidth, panelHeight, Constants.AXIS_LINE_WIDTH, Constants.AXIS_LINE_COLOR);
        }

#pragma warning disable IDE0051 // Remove unused private members
        private void Update()
#pragma warning restore IDE0051 // Remove unused private members
        {
            UpdateGeometry();
        }
    }
}
