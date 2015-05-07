﻿using SharpDX;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LimeJelly.CurveSimplifier.Geometry;

namespace LimeJelly.CurveSimplifier.Visualization
{
    /// <summary>
    /// A simple visualization step
    /// Displays the initial curve, and any points which have been discovered to be part of the solution.
    /// </summary>
    class BaseVisualizationStep : IVisualizationStep
    {
        protected IReadOnlyList<Vector2> Curve { get; private set; }

        private ICollection<Tuple<Vector2, Vector2, Color, float>> _originalSegments;
        private ICollection<Tuple<Vector2, Vector2, Color, float>> _solutionSegments;
        private ICollection<Tuple<Vector2, Color, float>> _points;

        public BaseVisualizationStep(IReadOnlyList<Vector2> curve)
        {
            Curve = curve;
            _originalSegments = new List<Tuple<Vector2, Vector2, Color, float>>();
            for (int i = 0; i < curve.Count - 1; ++i)
            {
                _originalSegments.Add(Tuple.Create(curve[i], curve[i + 1], Color.LightSlateGray, 2f));
            }
            _solutionSegments = new List<Tuple<Vector2, Vector2, Color, float>>();
            _points = Curve.Select(p => Tuple.Create(p, Color.LightSlateGray, 2f)).ToList();
        }
        public BaseVisualizationStep(IReadOnlyList<Vector2> curve, IEnumerable<Tuple<int, int>> solutionSegments, IEnumerable<int> solutionPoints)
            : this(curve)
        {
            SetSolution(solutionSegments, solutionPoints);
        }

        public void SetSolution(IEnumerable<Tuple<int, int>> solutionSegments, IEnumerable<int> solutionPoints)
        {
            _solutionSegments = solutionSegments
                .Select(s => Tuple.Create(Curve[s.Item1], Curve[s.Item2], Color.Black, 2f))
                .ToList();

            var set = solutionPoints as ISet<int> ?? new HashSet<int>(solutionPoints);
            _points = Curve.Select((p, i) => Tuple.Create(p, set.Contains(i) ? Color.Black : Color.LightSlateGray, 2f)).ToList();
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

        public virtual IEnumerable<Tuple<Vector2, Color, float>> GetCircles()
        {
            return Enumerable.Empty<Tuple<Vector2, Color, float>>();
        }

        public virtual IEnumerable<Tuple<Vector2, Color, float>> GetPoints()
        {
            return _points;
        }

        public virtual IEnumerable<Polygon> GetPolygons()
        {
            return Enumerable.Empty<Polygon>();
        }


        protected Tuple<Vector2, Vector2, Color, float> Segment(int start, int end, Color color, float width)
        {
            return Tuple.Create(Curve[start], Curve[end], color, width);
        }

        protected Tuple<Vector2, Vector2, Color, float> Highlight(int start, int end, Color color, float width)
        {
            var s = Curve[start];
            var e = Curve[end];
            var slope = s.X != e.X
                ? (e.Y - s.Y) / (e.X - s.X)
                : 10000f; //TODO: maybe a better way to handle this?
            var intersect = s.Y - (slope * s.X);
            var newStart = new Vector2(-1000f, (slope * -1000f) + intersect);
            var newEnd = new Vector2(2000f, (slope * 2000f) + intersect);
            return Tuple.Create(newStart, newEnd, color, width);
        }

        protected Tuple<Vector2, Color, float> Point(int index, Color color, float radius = 2)
        {
            return Tuple.Create(Curve[index], color, radius);
        }

    }
}
