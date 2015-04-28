using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LimeJelly.CurveSimplifier.Visualization;
using SharpDX;

namespace LimeJelly.CurveSimplifier.Simplification
{
    /// <summary>
    /// Implements the Ramer Douglas Peucker simplification algorithm.
    /// </summary>
    class RdpCurveSimplifier : BaseCurveSimplifier
    {
        private float _epsilon;

        private Stack<Tuple<int, int>> _visitNext;

        public RdpCurveSimplifier(IEnumerable<Vector2> points, float epsilon) : base(points)
        {
            _epsilon = epsilon;
            _visitNext = new Stack<Tuple<int, int>>();
            _visitNext.Push(Tuple.Create(0, Points.Count - 1));
        }

        public override IVisualizationStep NextStep()
        {
            if (!_visitNext.Any())
                return null;

            var top = _visitNext.Pop();
            var start = top.Item1; var end = top.Item2;
            return Process(start, end);
        }

        private IVisualizationStep Process(int start, int end)
        {
            if (end - start < 2)
            {
                return KeepEntireRange(start, end);
            }

            var farthest = 0;
            var farthestDist = _epsilon;
            var lStart = Points[start]; var lEnd = Points[end];

            for (int i = start + 1; i < end; ++i)
            {
                var dist = Points[i].DistanceToLine(lStart, lEnd);
                if (dist > farthestDist)
                {
                    farthest = i;
                    farthestDist = dist;
                }
            }
            if (farthestDist <= _epsilon)
            {
                return SimplifyRange(start, end);
            }

            return Recurse(start, farthest, end);
        }


        public IVisualizationStep KeepEntireRange(int start, int end)
        {
            for (int i = start; i < end; ++i)
            {
                AddSegmentToSolution(i, i + 1);
            }
            return Visualize(start, end);
        }

        public IVisualizationStep SimplifyRange(int start, int end)
        {
            AddSegmentToSolution(start, end);
            return Visualize(start, end);
        }

        public IVisualizationStep Recurse(int start, int farthest, int end)
        {
            AddPointToSolution(farthest);
            _visitNext.Push(Tuple.Create(farthest, end));
            _visitNext.Push(Tuple.Create(start, farthest));
            return Visualize(start, end, farthest);
        }

        private IVisualizationStep Visualize(int start, int end, int? farthest = null)
        {
            return new RdpVisualizationStep(Points, SolutionSegments, SolutionPoints, _epsilon, start, end, farthest);
        }
    }
}
