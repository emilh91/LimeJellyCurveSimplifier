using System;
using SharpDX;

namespace LimeJelly.CurveSimplifier
{
    static class Extensions
    {
        /// <summary>
        /// Determines the (shortest) distance between this point and the line which passes between two other points.
        /// </summary>
        /// <param name="point">The point in question.</param>
        /// <param name="start">One endpoint of the line.</param>
        /// <param name="end">Another endpoint of the line.</param>
        /// <returns>The shortest distance between this point and the line.</returns>
        public static float DistanceToLine(this Vector2 point, Vector2 start, Vector2 end)
        {
            var lineLength = Vector2.Distance(start, end);
            if (lineLength == 0) // Prevent a divide by zero.
                return Vector2.Distance(point, start);

            var yLength = end.Y - start.Y;
            var xLength = end.X - start.X;
            return Math.Abs((yLength * point.X) - (xLength * point.Y) + (end.X * start.Y) - (end.Y * start.X))
                / lineLength;
        }
    }
}
