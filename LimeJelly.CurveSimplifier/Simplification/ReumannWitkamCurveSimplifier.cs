using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LimeJelly.CurveSimplifier.Visualization;
using SharpDX;

namespace LimeJelly.CurveSimplifier.Simplification
{
    class ReumannWitkamCurveSimplifier : BaseCurveSimplifier
    {
        private readonly float _tolerance;

        private int _keyPointIndex;
        public ReumannWitkamCurveSimplifier(IEnumerable<Vector2> points, float tolerance) : base(points)
        {
            _tolerance = tolerance;
            _keyPointIndex = 0;

            AddEntireCurveToSolution();
        }

        public override IVisualizationStep NextStep()
        {
            if (Points.Count < _keyPointIndex + 3)
                return null;

            var lineStart = Points[_keyPointIndex];
            var lineEnd = Points[_keyPointIndex + 1];

            var testPointIndex = _keyPointIndex + 2;
            while (testPointIndex < Points.Count && Points[testPointIndex].DistanceToLine(lineStart, lineEnd) < _tolerance)
            {
                ++testPointIndex;
            }

            for (var i = _keyPointIndex + 1; i < testPointIndex - 1; ++i)
            {
                RemovePointFromSolution(i);
            }
            _keyPointIndex = testPointIndex - 1;
            return new BaseVisualizationStep(Points, SolutionSegments, SolutionPoints);

        }
    }
}
