using UnityEngine;
using UnityEngine.UI;

namespace Assets.Scripts.Utils
{
    /// <summary>
    /// Utility class for rendering vertices.
    /// </summary>
    public static class VertexUtil
    {
        /// <summary>
        /// Renders a rectangular diagram using vertex helpers.
        /// </summary>
        /// <param name="vh">The vertex helper used to render the diagram.</param>
        /// <param name="vertexCount">The current count of vertices.</param>
        /// <param name="center">The center point of the diagram.</param>
        /// <param name="width">The width of the diagram.</param>
        /// <param name="height">The height of the diagram.</param>
        /// <param name="axisLineWidth">The width of the axis lines.</param>
        /// <param name="axislineColor">The color of the axis lines.</param>
        /// <returns>The new count of vertices after rendering the diagram.</returns>
        public static int RenderDiagram(VertexHelper vh, int vertexCount, Vector2 center, float width, float height, float axisLineWidth, Color axislineColor)
        {
            int newCertexCount = vertexCount;

            newCertexCount = RenderLine(vh, newCertexCount, new(center.x, center.y), new(center.x, center.y + height), axisLineWidth, axislineColor);
            newCertexCount = RenderLine(vh, newCertexCount, new(center.x, center.y), new(center.x + width, center.y), axisLineWidth, axislineColor);
            newCertexCount = RenderLine(vh, newCertexCount, new(center.x, center.y + height), new(center.x + width, center.y + height), axisLineWidth, axislineColor);
            newCertexCount = RenderLine(vh, newCertexCount, new(center.x + width, center.y), new(center.x + width, center.y + height), axisLineWidth, axislineColor);

            return newCertexCount;
        }

        /// <summary>
        /// Renders a line between two points using vertex helpers.
        /// </summary>
        /// <param name="vh">The vertex helper used to render the line.</param>
        /// <param name="vertexCount">The current count of vertices.</param>
        /// <param name="pointA">The starting point of the line.</param>
        /// <param name="pointB">The ending point of the line.</param>
        /// <param name="width">The width of the line.</param>
        /// <param name="color">The color of the line.</param>
        /// <returns>The new count of vertices after rendering the line.</returns>
        public static int RenderLine(VertexHelper vh, int vertexCount, Vector2 pointA, Vector2 pointB, float width, Color32 color)
        {
            UIVertex vertex = UIVertex.simpleVert;
            vertex.color = color;

            Vector2 direction = (pointB - pointA).normalized;

            Vector2 perpendicular = new(-direction.y, direction.x);

            float halfWidth = width / 2f;

            Vector2 offset = perpendicular * halfWidth;

            Vector2 positionA1 = pointA + offset;
            Vector2 positionA2 = pointA - offset;
            Vector2 positionB1 = pointB - offset;
            Vector2 positionB2 = pointB + offset;

            vertex.position = new Vector3(positionA1.x, positionA1.y);
            vh.AddVert(vertex);
            vertex.position = new Vector3(positionA2.x, positionA2.y);
            vh.AddVert(vertex);
            vertex.position = new Vector3(positionB1.x, positionB1.y);
            vh.AddVert(vertex);
            vertex.position = new Vector3(positionB2.x, positionB2.y);
            vh.AddVert(vertex);

            vh.AddTriangle(0 + vertexCount, 1 + vertexCount, 2 + vertexCount);
            vh.AddTriangle(2 + vertexCount, 3 + vertexCount, 0 + vertexCount);

            return vertexCount + 4;
        }

        /// <summary>
        /// Renders a polygon using vertex helpers.
        /// </summary>
        /// <param name="vh">The vertex helper used to render the polygon.</param>
        /// <param name="vertexCount">The current count of vertices.</param>
        /// <param name="center">The center point of the polygon.</param>
        /// <param name="radius">The radius of the polygon.</param>
        /// <param name="edgeCount">The number of edges of the polygon.</param>
        /// <param name="color">The color of the polygon.</param>
        /// <returns>The new count of vertices after rendering the polygon.</returns>
        public static int RenderPoligon(VertexHelper vh, int vertexCount, Vector2 center, float radius, int edgeCount, Color32 color)
        {
            UIVertex vertex = UIVertex.simpleVert;
            vertex.color = color;

            for (int i = 0; i < edgeCount; i++)
            {
                vertex.position = GeometrieUtil.PolarToCartesian(radius, i * (360f / edgeCount), center);
                vh.AddVert(vertex);
            }

            vertex.position = new Vector3(center.x, center.y);
            vh.AddVert(vertex);

            for (int i = 1; i < edgeCount; i++)
            {
                vh.AddTriangle(i - 1 + vertexCount, i + vertexCount, edgeCount + vertexCount);
            }
            vh.AddTriangle(edgeCount - 1 + vertexCount, 0 + vertexCount, edgeCount + vertexCount);

            return vertexCount + edgeCount + 1;
        }
    }
}
