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
        private LinkedList<Tuple<int, int>> _solutionSegments;
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
            _solutionSegments = new LinkedList<Tuple<int, int>>();
            _solutionPoints = new HashSet<int>();
        }

        /// <summary>
        /// Marks a segment as part of the simplified curve.
        /// </summary>
        /// <param name="start">The index of the start vertex in Points.</param>
        /// <param name="end">The index of the end vertex in Points.</param>
        protected void AddSegmentToSolution(int start, int end)
        {
            _solutionSegments.AddLast(Tuple.Create(start, end));
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

        /// <summary>
        /// Marks the entire curve as part of the "simplified" version.
        /// Useful when iteratively removing points from the solution.
        /// </summary>
        protected void AddEntireCurveToSolution()
        {
            _solutionSegments = new LinkedList<Tuple<int, int>>();
            for(var i = 0; i < _points.Count - 1; ++i)
            {
                _solutionSegments.AddLast(Tuple.Create(i, i + 1));
            }
            _solutionPoints = new HashSet<int>(Enumerable.Range(0, _points.Count));

        }

        /// <summary>
        /// Removes a vertex from the simplified curve.
        /// Also joins any segments intersecting that vertex.
        /// </summary>
        /// <param name="index">The index of the vertex to remove.</param>
        protected void RemovePointFromSolution(int index)
        {
            _solutionPoints.Remove(index);
            var node = _solutionSegments.First;
            while (node != null && node.Value.Item1 <= index)
            {
                if (node.Value.Item1 == index)
                {
                    if (node.Previous != null)
                        node.Previous.Value = Tuple.Create(node.Previous.Value.Item1, node.Value.Item2);
                    _solutionSegments.Remove(node);
                }
                if (node.Value.Item2 == index)
                {
                    if (node.Next != null)
                        node.Next.Value = Tuple.Create(node.Value.Item1, node.Next.Value.Item2);
                    _solutionSegments.Remove(node);
                }
                node = node.Next;
            }
        }

        public abstract IVisualizationStep NextStep();

        public IVisualizationStep Initial()
        {
            return new BaseVisualizationStep(Points);
        }

        public IVisualizationStep Solution()
        {
            return new BaseVisualizationStep(Points, SolutionSegments, SolutionPoints);
        }
    }
}
