using System.Collections.Generic;
using LimeJelly.CurveSimplifier.Visualization;
using SharpDX;

namespace LimeJelly.CurveSimplifier.Simplification
{
    class OpheimCurveSimplifier : BaseCurveSimplifier
    {
        private readonly float _pTolerance;
        private readonly float _rTolerance;

        private int _keyPointIndex;


        public OpheimCurveSimplifier(IEnumerable<Vector2> points, float perpendicularTolerance, float radialTolerance) : base(points)
        {
            _pTolerance = perpendicularTolerance;
            _rTolerance = radialTolerance;
            _keyPointIndex = 0;

            if (Points.Count > 0) // you never know
                AddPointToSolution(_keyPointIndex);
        }

        public override IVisualizationStep NextStep()
        {
            if (Points.Count < _keyPointIndex + 3)
                return null;

            var lineStart = Points[_keyPointIndex];
            var lineEnd = Points[_keyPointIndex + 1];

            var testPointIndex = _keyPointIndex + 2;
            while (testPointIndex < Points.Count 
                && Points[testPointIndex].DistanceToLine(lineStart, lineEnd) < _pTolerance
                && Vector2.Distance(Points[testPointIndex], lineEnd) < _rTolerance)
            {
                ++testPointIndex;
            }

            var start = _keyPointIndex;
            var end = testPointIndex - 1;
            AddSegmentToSolution(start, end);
            _keyPointIndex = end;
            return new OpheimVisualizationStep(Points, SolutionSegments, SolutionPoints, start, start + 1, _pTolerance, _rTolerance);
        }
    }
}
