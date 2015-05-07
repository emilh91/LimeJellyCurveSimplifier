using System;
using System.Collections.Generic;
using System.Linq;
using LimeJelly.CurveSimplifier.Visualization;
using SharpDX;

namespace LimeJelly.CurveSimplifier.Simplification
{
    /// <summary>
    /// Implements the Lang simplification algorithm.
    /// </summary>
    class LangCurveSimplifier : BaseCurveSimplifier
    {
        private readonly float _epsilon;

        private readonly Stack<Tuple<int, int>> _visitNext;

        public LangCurveSimplifier(IEnumerable<Vector2> points, float epsilon) : base(points)
        {
            _epsilon = epsilon;
            _visitNext = new Stack<Tuple<int, int>>();
            AddPointToSolution(0);
            _visitNext.Push(Tuple.Create(0, Points.Count - 1));
        }

        public override IVisualizationStep NextStep()
        {
            if (!_visitNext.Any())
                return null;

            var top = _visitNext.Pop();
            var start = top.Item1;
            var end = top.Item2;
            return Process(start, end);
        }

        private IVisualizationStep Process(int start, int end)
        {
            if (start == end)
            {
                return null;
            }

            if (end - start == 1)
            {
                AddSegmentToSolution(start, end);
                AddPointToSolution(end);
                _visitNext.Push(Tuple.Create(end, Points.Count - 1));
                return Visualize(end, Points.Count - 1);
            }

            var keyPoint = Points[start];
            var endPoint = Points[end];
            var farthestDistance = Points
                .Where((p, i) => start < i && i < end)
                .Max(p => p.DistanceToLine(keyPoint, endPoint));

            if (farthestDistance <= _epsilon)
            {
                AddSegmentToSolution(start, end);
                AddPointToSolution(end);
                _visitNext.Push(Tuple.Create(end, Points.Count - 1));
                return Visualize(end, Points.Count - 1);
            }

            return TrimRight(start, end);
        }

        private IVisualizationStep TrimRight(int start, int end)
        {
            _visitNext.Push(Tuple.Create(start, end - 1));
            return Visualize(start, end - 1);
        }

        private IVisualizationStep Visualize(int start, int end)
        {
            return new RdpVisualizationStep(Points, SolutionSegments, SolutionPoints, _epsilon, start, end, null);
        }
    }
}
