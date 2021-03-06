﻿using System;
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
            while (testPointIndex < Points.Count && Points[testPointIndex].DistanceToLine(lineStart, lineEnd) < _tolerance)
            {
                ++testPointIndex;
            }

            var start = _keyPointIndex;
            var end = testPointIndex - 1;
            AddSegmentToSolution(start, end);
            _keyPointIndex = end;
            return new ReumannWitkamVisualizationStep(Points, SolutionSegments, SolutionPoints, start, start + 1, _tolerance);

        }
    }
}
