using System;

namespace Assets.Scripts.Types
{
    /// <summary>
    /// Represents a single entry in a dataset.
    /// </summary>
    [Serializable]
    public class DataSetEntry
    {
        /// <summary>
        /// An array of float values associated with this dataset entry.
        /// The array is initialized with zero elements by default.
        /// </summary>
        public float[] Values = new float[0];
    }
}
