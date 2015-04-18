using System;

namespace LimeJelly.CurveSimplifier
{
    public static class PointExtensions
    {
        /// <summary>
        /// Calculates the distance between this point and another.
        /// </summary>
        /// <param name="point">The point in question.</param>
        /// <param name="other">The other point in question.</param>
        /// <returns>The distance between the two points given.</returns>
        public static double DistanceToPoint(this IPoint point, IPoint other)
        {
            var xDist = other.X - point.X;
            var yDist = other.Y - point.Y;
            return Math.Sqrt(xDist * xDist + yDist * yDist);
        }

        /// <summary>
        /// Determines the (shortest) distance between this point and the line which passes between two other points
        /// </summary>
        /// <param name="point">The point in question.</param>
        /// <param name="start">One endpoint of the line.</param>
        /// <param name="end">Another endpoint of a line.</param>
        /// <returns>The shortest distance between this point and the line.</returns>
        public static double DistanceToLine(this IPoint point, IPoint start, IPoint end)
        {
            var lineLength = start.DistanceToPoint(end);
            if (lineLength == 0) // Prevent a divide by zero.
                return point.DistanceToPoint(start);

            var yLength = end.Y - start.Y;
            var xLength = end.X - start.X;
            return Math.Abs((yLength * point.X) - (xLength * point.Y) + (end.X * start.Y) - (end.Y * start.X))
                / lineLength;
        }
    }
}
