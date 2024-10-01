using UnityEngine;

namespace Assets.Scripts.Utils
{
    /// <summary>
    /// Utility class for geometric calculations.
    /// </summary>
    internal static class GeometrieUtil
    {
        /// <summary>
        /// Converts polar coordinates to Cartesian coordinates.
        /// </summary>
        /// <param name="radius">The radius (distance from the center).</param>
        /// <param name="angleDegree">The angle in degrees.</param>
        /// <param name="center">The center point (default is the origin).</param>
        /// <returns>A <see cref="Vector2"/> representing the Cartesian coordinates.</returns>
        public static Vector2 PolarToCartesian(float radius, float angleDegree, Vector2 center = new Vector2())
        {
            float alpha = angleDegree * (Mathf.PI / 180);
            float x = center.x + radius * Mathf.Cos(alpha);
            float y = center.y + radius * Mathf.Sin(alpha);
            return new(x, y);
        }
    }
}
