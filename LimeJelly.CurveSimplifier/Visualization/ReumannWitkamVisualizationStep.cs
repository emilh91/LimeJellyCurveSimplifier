using SharpDX;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LimeJelly.CurveSimplifier.Visualization
{
    class ReumannWitkamVisualizationStep : BaseVisualizationStep
    {
        private int _start;
        private int _end;
        private float _tolerance;

        public ReumannWitkamVisualizationStep(IReadOnlyList<Vector2> curve, IEnumerable<Tuple<int, int>> solutionSegments, IEnumerable<int> solutionPoints,
            int start, int end, float tolerance)
            : base(curve, solutionSegments, solutionPoints)
        {
            _start = start;
            _end = end;
            _tolerance = tolerance;
        }

        public override IEnumerable<Tuple<Vector2, Vector2, Color, float>> GetSegments()
        {
            // highlight epsilon
            yield return Highlight(_start, _end, Color.LightBlue, _tolerance * 2);

            foreach (var seg in base.GetSegments())
                yield return seg;
        }
    }
}
