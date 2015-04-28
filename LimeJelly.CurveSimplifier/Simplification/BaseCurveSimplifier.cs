using LimeJelly.CurveSimplifier.Visualization;
using SharpDX;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LimeJelly.CurveSimplifier.Simplification
{
    abstract class BaseCurveSimplifier : ICurveSimplifier
    {
        private List<Vector2> _points;
        private List<Tuple<int, int>> _solutionSegments;
        private HashSet<int> _solutionPoints;

        /// <summary>
        /// The points from the initial curve.
        /// </summary>
        protected IReadOnlyList<Vector2> Points { get { return _points; } }

        /// <summary>
        /// All segments of the initial curve determined to be in the final solution.
        /// </summary>
        protected IEnumerable<Tuple<int, int>> SolutionSegments { get { return _solutionSegments; } }

        /// <summary>
        /// All points from the initial curve determined to be in the final solution.
        /// </summary>
        protected IEnumerable<int> SolutionPoints { get { return _solutionPoints; } }


        protected BaseCurveSimplifier(IEnumerable<Vector2> points)
        {
            _points = points.ToList();
            _solutionSegments = new List<Tuple<int, int>>();
            _solutionPoints = new HashSet<int>();
        }

        /// <summary>
        /// Marks a segment as part of the simplified curve.
        /// </summary>
        /// <param name="start">The index of the start vertex in Points.</param>
        /// <param name="end">The index of the end vertex in Points.</param>
        protected void AddSegmentToSolution(int start, int end)
        {
            _solutionSegments.Add(Tuple.Create(start, end));
            _solutionPoints.Add(start);
            _solutionPoints.Add(end);
        }

        /// <summary>
        /// Marks a point as part of the simplified curve.
        /// </summary>
        /// <param name="index">The index of the vertex to mark in Points.</param>
        protected void AddPointToSolution(int index)
        {
            _solutionPoints.Add(index);
        }

        public abstract IVisualizationStep NextStep();

        public IVisualizationStep Initial()
        {
            return new BaseVisualizationStep(Points);
        }
    }
}
