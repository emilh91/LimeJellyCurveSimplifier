using System;
using System.Collections.Generic;
using SharpDX;

namespace LimeJelly.CurveSimplifier.Visualization
{
    class OpheimVisualizationStep : BaseVisualizationStep
    {
        private readonly int _start;
        private readonly int _end;
        private readonly float _pTolerance;
        private float _rTolerance;

        public OpheimVisualizationStep(IReadOnlyList<Vector2> curve, IEnumerable<Tuple<int, int>> solutionSegments, IEnumerable<int> solutionPoints,
            int start, int end, float pTolerance, float rTolerance)
            : base(curve, solutionSegments, solutionPoints)
        {
            _start = start;
            _end = end;
            _pTolerance = pTolerance;
            _rTolerance = rTolerance;
        }

        public override IEnumerable<Tuple<Vector2, Color, float>> GetCircles()
        {
            yield return Point(_end, Color.Aqua, _rTolerance);
        }

        public override IEnumerable<Tuple<Vector2, Vector2, Color, float>> GetSegments()
        {
            // highlight epsilon
            yield return Highlight(_start, _end, Color.LightBlue, _pTolerance * 2);
            yield return Highlight(_start, _end, Color.Blue, 1f);

            foreach (var seg in base.GetSegments())
                yield return seg;
        }

        public override IEnumerable<Tuple<Vector2, Color, float>> GetPoints()
        {
            foreach (var pt in base.GetPoints())
                yield return pt;

            yield return Point(_start, Color.Blue, 3f);
            yield return Point(_end, Color.Blue, 3f);
        }
    }
}
