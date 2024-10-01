using System;
using System.Collections.Generic;

namespace Assets.Scripts.Types
{
    /// <summary>
    /// Represents a collection of XY data entries.
    /// </summary>
    [Serializable]
    public class XYData
    {
        /// <summary>
        /// A list of <see cref="XYDataEntry"/> objects, each containing X and Y values.
        /// </summary>
        public List<XYDataEntry> XY = new();
    }
}
