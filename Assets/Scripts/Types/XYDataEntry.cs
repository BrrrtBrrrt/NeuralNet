using System;
using System.Collections.Generic;

namespace Assets.Scripts.Types
{
    /// <summary>
    /// Represents a data entry consisting of a collection of X and Y values.
    /// </summary>
    [Serializable]
    public class XYDataEntry
    {
        /// <summary>
        /// A list of X values for the data entry.
        /// </summary>
        public List<float> X = new();

        /// <summary>
        /// A list of Y values for the data entry.
        /// </summary>
        public List<float> Y = new();
    }
}
