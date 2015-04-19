using System;
using System.Collections.Generic;
using SharpDX;

namespace LimeJelly.CurveSimplifier
{
    public static class Extensions
    {
        /// <summary>
        /// Determines the (shortest) distance between this point and the line which passes between two other points.
        /// </summary>
        /// <param name="point">The point in question.</param>
        /// <param name="start">One endpoint of the line.</param>
        /// <param name="end">Another endpoint of the line.</param>
        /// <returns>The shortest distance between this point and the line.</returns>
        public static double DistanceToLine(this Vector3 point, Vector3 start, Vector3 end)
        {
            var lineLength = Vector3.Distance(start, end);
            if (lineLength == 0) // Prevent a divide by zero.
                return Vector3.Distance(point, start);

            var yLength = end.Y - start.Y;
            var xLength = end.X - start.X;
            return Math.Abs((yLength * point.X) - (xLength * point.Y) + (end.X * start.Y) - (end.Y * start.X))
                / lineLength;
        }

        public static Curve ToCurve(this IEnumerable<Vector3> source)
        {
            return new Curve(source);
        }
    }
}
