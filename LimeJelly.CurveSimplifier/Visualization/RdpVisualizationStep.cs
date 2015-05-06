using System;
using System.Collections.Generic;
using System.Linq;
using SharpDX;

namespace LimeJelly.CurveSimplifier.Visualization
{
    class RdpVisualizationStep : BaseVisualizationStep
    {
        private int _start;
        private int _end;
        private float _epsilon;

        private int? _farthest;

        public RdpVisualizationStep(IReadOnlyList<Vector2> curve,
            IEnumerable<Tuple<int, int>> solvedSegments, IEnumerable<int> solvedPoints,
            float epsilon,
            int start, int end, int? farthest)
            : base(curve, solvedSegments, solvedPoints)
        {
            _start = start;
            _end = end;
            _epsilon = epsilon;
            _farthest = farthest;
        }

        public override IEnumerable<Tuple<Vector2, Vector2, Color, float>> GetSegments()
        {
            // highlight epsilon
            yield return Highlight(_start, _end, Color.LightBlue, _epsilon * 2);
            yield return Highlight(_start, _end, Color.Blue, 1f);

            foreach (var seg in base.GetSegments())
                yield return seg;
        }

        public override IEnumerable<Tuple<Vector2, Color, float>> GetPoints()
        {
            if (_farthest.HasValue)
                yield return Point(_farthest.Value, Color.Green, 10);

            foreach (var pt in base.GetPoints())
                yield return pt;

        }
    }
}
