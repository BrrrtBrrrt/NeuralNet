namespace Assets.Scripts.Utils
{
    /// <summary>
    /// Utility class for copying arrays of various dimensions.
    /// </summary>
    public static class CopyUtil
    {
        /// <summary>
        /// Creates a deep copy of a one-dimensional array.
        /// </summary>
        /// <typeparam name="T">The type of elements in the array.</typeparam>
        /// <param name="array">The array to copy.</param>
        /// <returns>A deep copy of the array.</returns>
        public static T[] CopyArray<T>(T[] array)
        {
            if (array == null) return null;
            T[] copy = new T[array.Length];
            for (int i = 0; i < array.Length; i++)
            {
                copy[i] = array[i];
            }
            return copy;
        }

        /// <summary>
        /// Creates a deep copy of a two-dimensional array.
        /// </summary>
        /// <typeparam name="T">The type of elements in the array.</typeparam>
        /// <param name="array">The array to copy.</param>
        /// <returns>A deep copy of the array.</returns>
        public static T[][] CopyArray<T>(T[][] array)
        {
            if (array == null) return null;
            T[][] copy = new T[array.Length][];
            for (int i = 0; i < array.Length; i++)
            {
                copy[i] = CopyArray(array[i]);
            }
            return copy;
        }

        /// <summary>
        /// Creates a deep copy of a three-dimensional array.
        /// </summary>
        /// <typeparam name="T">The type of elements in the array.</typeparam>
        /// <param name="array">The array to copy.</param>
        /// <returns>A deep copy of the array.</returns>
        public static T[][][] CopyArray<T>(T[][][] array)
        {
            if (array == null) return null;
            T[][][] copy = new T[array.Length][][];
            for (int i = 0; i < array.Length; i++)
            {
                copy[i] = CopyArray(array[i]);
            }
            return copy;
        }
    }
}
