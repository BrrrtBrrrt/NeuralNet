using Assets.Scripts.Enums;
using Assets.Scripts.Extensions;
using Assets.Scripts.Services;
using Assets.Scripts.Types;
using Assets.Scripts.Utils;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Scripts.Entities
{
    /// <summary>
    /// Generates synthetic data based on a target function and prepares it for training and testing.
    /// </summary>
    internal class DataGenerator
    {
        /// <summary>
        /// Specifies the type of target function used to generate the data.
        /// The available functions are EXPONENTIAL, LINEAR, and SIN.
        /// </summary>
        public TargetFunctionType TargetFunction = TargetFunctionType.SIN;

        /// <summary>
        /// Number of data points to generate.
        /// </summary>
        public int DataCount { get; set; } = 1500;

        /// <summary>
        /// Minimum value of the X-axis range for data generation.
        /// </summary>
        public float XRangeMin { get; set; } = -2 * Mathf.PI;

        /// <summary>
        /// Maximum value of the X-axis range for data generation.
        /// </summary>
        public float XRangeMax { get; set; } = 2 * Mathf.PI;

        /// <summary>
        /// Minimum value of the scale for normalizing the data.
        /// </summary>
        public float ScaleMin { get; set; } = -1.0f;

        /// <summary>
        /// Maximum value of the scale for normalizing the data.
        /// </summary>
        public float ScaleMax { get; set; } = 1.0f;

        /// <summary>
        /// Percentage of data used for testing. The remaining data is used for training.
        /// </summary>
        public float TrainTestSplitFactor { get; set; } = 75;

        /// <summary>
        /// Indicates whether to pick test data from the entire dataset or only from the range of the original data.
        /// </summary>
        public bool PickTestDataFromWholeSet { get; set; } = true;

        /// <summary>
        /// Indicates whether to evenly pick test data points or use random sampling.
        /// Only applicable if <see cref="PickTestDataFromWholeSet"/> is true.
        /// </summary>
        public bool TestDataIsEvenlyPicked { get; set; } = true;

        /// <summary>
        /// Indicates whether to shuffle the training and testing data after preparation.
        /// </summary>
        public bool ShuffleData { get; set; } = true;

        /// <summary>
        /// The factor of noise to add to the generated data.
        /// </summary>
        public float NoiseFactor { get; set; } = 0.15f;

        /// <summary>
        /// Holds the raw training data before any processing.
        /// </summary>
        private XYData DataTraining { get; set; } = new();

        /// <summary>
        /// Holds the raw test data before any processing.
        /// </summary>
        private XYData DataTest { get; set; } = new();

        /// <summary>
        /// Holds the prepared training data after scaling and shuffling (if applicable).
        /// </summary>
        private XYData DataTrainingPrepared { get; set; } = new();

        /// <summary>
        /// Holds the prepared test data after scaling and shuffling (if applicable).
        /// </summary>
        private XYData DataTestPrepared { get; set; } = new();

        /// <summary>
        /// Holds the prepared training data in its original order before shuffling.
        /// </summary>
        private XYData DataTrainingPreparedOriginalOrder { get; set; } = new();

        /// <summary>
        /// Holds the prepared test data in its original order before shuffling.
        /// </summary>
        private XYData DataTestPreparedOriginalOrder { get; set; } = new();

        /// <summary>
        /// Minimum Y value observed in the generated data.
        /// </summary>
        private float YMinValue { get; set; } = float.MaxValue;

        /// <summary>
        /// Maximum Y value observed in the generated data.
        /// </summary>
        private float YMaxValue { get; set; } = float.MinValue;

        /// <summary>
        /// Minimum X value observed in the generated data.
        /// </summary>
        private float XMinValue { get; set; } = float.MaxValue;

        /// <summary>
        /// Maximum X value observed in the generated data.
        /// </summary>
        private float XMaxValue { get; set; } = float.MinValue;

        /// <summary>
        /// Generates the synthetic data, splits it into training and testing sets, prepares it by scaling, and optionally shuffles it.
        /// </summary>
        /// <returns>
        /// A tuple containing the raw training data, raw test data, prepared training data, prepared test data,
        /// prepared training data in original order, and prepared test data in original order.
        /// </returns>
        public (XYData, XYData, XYData, XYData, XYData, XYData) Generate()
        {
            // Initialize data containers and min/max values
            DataTraining = new();
            DataTest = new();
            DataTrainingPrepared = new();
            DataTestPrepared = new();
            DataTrainingPreparedOriginalOrder = new();
            DataTestPreparedOriginalOrder = new();

            YMinValue = float.MaxValue;
            YMaxValue = float.MinValue;
            XMinValue = float.MaxValue;
            XMaxValue = float.MinValue;

            // Generate raw data
            List<List<float>> data = GenerateRawData();

            // Split data into training and testing sets
            SplitTrainTest(data);

            // Prepare (scale) the training and testing data
            PrepareTrainTest();

            // Shuffle data if specified
            if (ShuffleData) ShuffleTrainTestPrepared();

            return (
                DataTraining,
                DataTest,
                DataTrainingPrepared,
                DataTestPrepared,
                DataTrainingPreparedOriginalOrder,
                DataTestPreparedOriginalOrder
            );
        }

        /// <summary>
        /// Generates raw data based on the target function, X range, and data count.
        /// Adds Gaussian noise to the generated Y values.
        /// </summary>
        /// <returns>A list of data points where each point is a pair [x, y].</returns>
        private List<List<float>> GenerateRawData()
        {
            float xStepSize = (XRangeMax - XRangeMin) / DataCount;
            List<List<float>> data = new();

            // Generate data points based on the specified target function
            for (float xValue = XRangeMin; xValue <= xStepSize * DataCount + XRangeMin; xValue += xStepSize)
            {
                // Compute the y value using the selected target function
                float y = TargetFunction switch
                {
                    TargetFunctionType.EXPONENTIAL => TargetFunctionUtil.Exponential(xValue),
                    TargetFunctionType.LINEAR => TargetFunctionUtil.Linear(xValue),
                    TargetFunctionType.SIN => TargetFunctionUtil.Sin(xValue),
                    TargetFunctionType.CUSTOM_STEP => TargetFunctionUtil.CustomStep(xValue),
                    _ => throw new System.Exception($"Target function type is not supported ({TargetFunction})"),
                };

                // Add noise to the y value
                y += NoiseFactor * MathUtil.GenerateGaussianNoise();

                // Add the (x, y) pair to the data list
                data.Add(new()
                {
                    xValue, y
                });

                // Update min/max values for scaling later
                XMinValue = xValue < XMinValue ? xValue : XMinValue;
                YMinValue = y < YMinValue ? y : YMinValue;
                XMaxValue = xValue > XMaxValue ? xValue : XMaxValue;
                YMaxValue = y > YMaxValue ? y : YMaxValue;
            }
            return data;
        }

        /// <summary>
        /// Splits the raw data into training and testing sets based on the configuration.
        /// Data is split either by evenly picking or randomly sampling.
        /// </summary>
        /// <param name="data">The raw data to be split.</param>
        private void SplitTrainTest(List<List<float>> data)
        {
            if (PickTestDataFromWholeSet)
            {
                if (TestDataIsEvenlyPicked)
                {
                    // Evenly pick test data points from the entire dataset
                    int testDataSize = DataCount - Mathf.FloorToInt(TrainTestSplitFactor / 100 * DataCount);
                    float testDataIndexStepRaw = (float)data.Count / testDataSize;
                    float testDataIndexRaw = 0;

                    for (int dataIndex = 0; dataIndex < data.Count; dataIndex++)
                    {
                        if (dataIndex >= Mathf.FloorToInt(testDataIndexRaw))
                        {
                            DataTest.XY.Add(new()
                            {
                                X = new() { data[dataIndex][0] },
                                Y = new() { data[dataIndex][1] },
                            });
                            testDataIndexRaw += testDataIndexStepRaw;
                            continue;
                        }

                        DataTraining.XY.Add(new()
                        {
                            X = new() { data[dataIndex][0] },
                            Y = new() { data[dataIndex][1] },
                        });
                    }
                }
                else
                {
                    // Randomly pick test data points from the entire dataset
                    System.Random random = new();
                    HashSet<int> testIndices = new();
                    int testDataSize = DataCount - Mathf.FloorToInt(TrainTestSplitFactor / 100 * DataCount);
                    while (testIndices.Count < testDataSize)
                    {
                        testIndices.Add(random.Next(data.Count));
                    }

                    for (int dataIndex = 0; dataIndex < data.Count; dataIndex++)
                    {
                        if (testIndices.Contains(dataIndex))
                        {
                            DataTest.XY.Add(new()
                            {
                                X = new() { data[dataIndex][0] },
                                Y = new() { data[dataIndex][1] },
                            });
                        }
                        else
                        {
                            DataTraining.XY.Add(new()
                            {
                                X = new() { data[dataIndex][0] },
                                Y = new() { data[dataIndex][1] },
                            });
                        }
                    }
                }
            }
            else
            {
                // Split the data based on the configured percentage for training and testing
                int dataSplitIndex = Mathf.FloorToInt(DataCount * (TrainTestSplitFactor / 100));
                for (int dataIndex = 0; dataIndex < data.Count; dataIndex++)
                {
                    (dataIndex < dataSplitIndex ? DataTraining : DataTest).XY.Add(new()
                    {
                        X = new() { data[dataIndex][0] },
                        Y = new() { data[dataIndex][1] },
                    });
                }
            }
        }

        /// <summary>
        /// Prepares the training and testing data by scaling the X and Y values to the specified range.
        /// Also maintains the original order of the data for both training and testing sets.
        /// </summary>
        private void PrepareTrainTest()
        {
            ScalerService scalerServiceX = new();
            ScalerService scalerServiceY = new();

            // Configure the scaler for X values
            scalerServiceX.OriginalMin = XMinValue;
            scalerServiceX.OriginalMax = XMaxValue;
            scalerServiceX.NewMin = ScaleMin;
            scalerServiceX.NewMax = ScaleMax;

            // Configure the scaler for Y values
            scalerServiceY.OriginalMin = YMinValue;
            scalerServiceY.OriginalMax = YMaxValue;
            scalerServiceY.NewMin = ScaleMin;
            scalerServiceY.NewMax = ScaleMax;

            // Scale and store training data
            foreach (XYDataEntry xyDataEntries in DataTraining.XY)
            {
                DataTrainingPrepared.XY.Add(new()
                {
                    X = scalerServiceX.Scale(xyDataEntries.X),
                    Y = scalerServiceY.Scale(xyDataEntries.Y),
                });
                DataTrainingPreparedOriginalOrder.XY.Add(new()
                {
                    X = scalerServiceX.Scale(xyDataEntries.X),
                    Y = scalerServiceY.Scale(xyDataEntries.Y),
                });
            }

            // Scale and store test data
            foreach (XYDataEntry xyDataEntries in DataTest.XY)
            {
                DataTestPrepared.XY.Add(new()
                {
                    X = scalerServiceX.Scale(xyDataEntries.X),
                    Y = scalerServiceY.Scale(xyDataEntries.Y),
                });
                DataTestPreparedOriginalOrder.XY.Add(new()
                {
                    X = scalerServiceX.Scale(xyDataEntries.X),
                    Y = scalerServiceY.Scale(xyDataEntries.Y),
                });
            }
        }

        /// <summary>
        /// Shuffles the prepared training and testing data if <see cref="ShuffleData"/> is true.
        /// </summary>
        private void ShuffleTrainTestPrepared()
        {
            // Shuffle the prepared training and testing data to ensure randomness
            DataTrainingPrepared.XY.Shuffle();
            DataTestPrepared.XY.Shuffle();
        }
    }
}
