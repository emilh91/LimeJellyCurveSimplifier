using System.Collections.Generic;
using System.Linq;
using SharpDX;

namespace LimeJelly.CurveSimplifier.Simplifier
{
    class RamerDouglasPeuckerSimplifier : MemoizedCurveSimplifier
    {
        internal RamerDouglasPeuckerSimplifier(Curve curve) : base(curve) { }

        protected override Curve SimplifyImpl(Curve curve, double epsilon)
        {
            return SimplifyImpl(curve.Points, epsilon).ToCurve();
        }

        private IEnumerable<Vector3> SimplifyImpl(IEnumerable<Vector3> region, double epsilon)
        {
            var points = region.ToList();
            if (points.Count < 2)
                return points; // A point or an empty list are about as simple as can be.

            var farthest = 0;
            var farthestDist = epsilon;
            var start = points.First(); var end = points.Last();

            // Find the point farthest from the line containing both endpoints of the curve.
            for(var i = 1; i < points.Count - 1; ++i)
            {
                var dist = points[i].DistanceToLine(start, end);
                if (dist > farthestDist)
                {
                    farthest = i;
                    farthestDist = dist;
                }
            }
            if (farthestDist <= epsilon)
            {
                // No point in this region is farther than epsilon from the curve,
                // so we discard everything but the endpoints.
                return new[] { start, end };
            }

            // Recurse on the sections before and after the farthest point,
            // and be sure to keep the farthest point too (which is definitely > epsilon).
            var firstHalf = SimplifyImpl(points.Take(farthest + 1), epsilon);
            var secondHalf = SimplifyImpl(points.Skip(farthest), epsilon);

            return firstHalf.Concat(secondHalf.Skip(1)); // Skip farthest so we don't return it twice.
        }
    }
}
