using SharpDX;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LimeJelly.CurveSimplifier.Visualization
{
    /// <summary>
    /// A simple visualization step
    /// Displays the initial curve, and any points which have been discovered to be part of the solution.
    /// </summary>
    class BaseVisualizationStep : IVisualizationStep
    {
        private IReadOnlyList<Vector2> _curve;

        private ICollection<Tuple<Vector2, Vector2, Color, float>> _originalSegments;
        private ICollection<Tuple<Vector2, Vector2, Color, float>> _solutionSegments;
        private ICollection<Tuple<Vector2, Color, float>> _points;

        public BaseVisualizationStep(IReadOnlyList<Vector2> curve)
        {
            _curve = curve;
            _originalSegments = new List<Tuple<Vector2, Vector2, Color, float>>();
            for (int i = 0; i < curve.Count - 1; ++i)
            {
                _originalSegments.Add(Tuple.Create(curve[i], curve[i + 1], Color.LightSlateGray, 2f));
            }
            _solutionSegments = new List<Tuple<Vector2, Vector2, Color, float>>();
            _points = _curve.Select(p => Tuple.Create(p, Color.LightSlateGray, 2f)).ToList();
        }
        public BaseVisualizationStep(IReadOnlyList<Vector2> curve, IEnumerable<Tuple<int, int>> solutionSegments, IEnumerable<int> solutionPoints)
            : this(curve)
        {
            SetSolution(solutionSegments, solutionPoints);
        }

        public void SetSolution(IEnumerable<Tuple<int, int>> solutionSegments, IEnumerable<int> solutionPoints)
        {
            _solutionSegments = solutionSegments
                .Select(s => Tuple.Create(_curve[s.Item1], _curve[s.Item2], Color.Black, 2f))
                .ToList();

            var set = solutionPoints as ISet<int> ?? new HashSet<int>(solutionPoints);
            _points = _curve.Select((p, i) => Tuple.Create(p, set.Contains(i) ? Color.Black : Color.LightSlateGray, 2f)).ToList();
        }


        public virtual IEnumerable<Tuple<Vector2, Vector2, Color, float>> GetSegments()
        {
            // draw original line
            foreach (var original in _originalSegments)
            {
                yield return original;
            }

            // draw simplified line
            foreach (var solution in _solutionSegments)
            {
                yield return solution;
            }
        }

        public virtual IEnumerable<Tuple<Vector2, Color, float>> GetPoints()
        {
            return _points;
        }


        protected Tuple<Vector2, Vector2, Color, float> Segment(int start, int end, Color color, float width)
        {
            return Tuple.Create(_curve[start], _curve[end], color, width);
        }

        protected Tuple<Vector2, Color, float> Point(int index, Color color, float radius = 2)
        {
            return Tuple.Create(_curve[index], color, radius);
        }

    }
}
