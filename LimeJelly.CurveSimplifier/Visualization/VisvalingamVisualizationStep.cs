using SharpDX;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LimeJelly.CurveSimplifier.Geometry;

namespace LimeJelly.CurveSimplifier.Visualization
{
    class VisvalingamVisualizationStep : BaseVisualizationStep
    {
        private Polygon _smallestTriangle;

        public VisvalingamVisualizationStep(IReadOnlyList<Vector2> curve, IEnumerable<Tuple<int, int>> solutionSegments, IEnumerable<int> solutionPoints,
            Vector2 first, Vector2 middle, Vector2 last)
            : base(curve, solutionSegments, solutionPoints)
        {
            _smallestTriangle = new Polygon(Color.Green, first, middle, last);
        }

        public override IEnumerable<Polygon> GetPolygons()
        {
            yield return _smallestTriangle;
        }
    }
}
