using SharpDX;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LimeJelly.CurveSimplifier.Visualization
{
    class VisvalingamVisualizationStep : BaseVisualizationStep
    {
        private Polygon _smallestTriangle;

        public VisvalingamVisualizationStep(IReadOnlyList<Vector2> curve, IEnumerable<Tuple<int, int>> solutionSegments, IEnumerable<int> solutionPoints,
            int smallestTriangle)
            : base(curve, solutionSegments, solutionPoints)
        {
            _smallestTriangle = new Polygon(Color.Green,
                Curve[smallestTriangle - 1],
                Curve[smallestTriangle],
                Curve[smallestTriangle + 1]);
        }

        public override IEnumerable<Polygon> GetPolygons()
        {
            yield return _smallestTriangle;
        }
    }
}
