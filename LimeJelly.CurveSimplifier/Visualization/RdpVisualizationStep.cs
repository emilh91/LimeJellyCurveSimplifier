using System;
using System.Collections.Generic;
using System.Linq;
using SharpDX;

namespace LimeJelly.CurveSimplifier.Visualization
{
    class RdpVisualizationStep : IVisualizationStep
    {
        private readonly float _epsilon;
        private readonly IReadOnlyList<Vector2> _points;
        private readonly LinkedList<int> _segments;
        private readonly LinkedListNode<int> _current;

        internal RdpVisualizationStep(IEnumerable<Vector2> points, float epsilon)
        {
            _epsilon = epsilon;
            _points = points.ToList().AsReadOnly();
            _segments = new LinkedList<int>(new[] {0, _points.Count - 1});
            _current = _segments.First;
        }

        private RdpVisualizationStep(IReadOnlyList<Vector2> points, LinkedList<int> segments, LinkedListNode<int> current, float epsilon)
        {
            _epsilon = epsilon;
            _points = points;
            _segments = segments;
            _current = current;
        }

        public IEnumerable<Tuple<Vector2, Vector2, Color, float>> GetSegments()
        {
            for (var node = _segments.First; node.Next != null; node = node.Next)
            {
                for (var i = node.Value; i < node.Next.Value; i++)
                {
                    var a = _points[i];
                    var b = _points[i + 1];
                    yield return Tuple.Create(a, b, Color.LightSlateGray, 1f);
                }

                {
                    var a = _points[node.Value];
                    var b = _points[node.Next.Value];
                    yield return Tuple.Create(a, b, Color.Black, 2f);
                }
            }

            if (_current.Next != null)
            {
                var a = _points[_current.Value];
                var b = _points[_current.Next.Value];
                yield return Tuple.Create(a, b, Color.LightBlue, _epsilon * 2);
                yield return Tuple.Create(a, b, Color.Blue, 1f);
            }
        }

        public IEnumerable<Tuple<Vector2, Color>> GetPoints()
        {
            var node = _segments.First;

            for (var i = 0; i < _points.Count; i++)
            {
                var pt = _points[i];
                if (i == node.Value)
                {
                    node = node.Next;
                    yield return Tuple.Create(pt, Color.Black);
                }
                else
                {
                    yield return Tuple.Create(pt, Color.LightSlateGray);
                }
            }
        }

        public IVisualizationStep NextStep()
        {
            if (_current.Next == null) return null;

            var farthestPointIndex = 0;
            var farthestDist = _epsilon;
            var pt1 = _points[_current.Value];
            var pt2 = _points[_current.Next.Value];

            for (var i = _current.Value + 1; i < _current.Next.Value; i++)
            {
                var dist = _points[i].DistanceToLine(pt1, pt2);
                if (dist > farthestDist)
                {
                    farthestPointIndex = i;
                    farthestDist = dist;
                }
            }

            if (farthestDist <= _epsilon)
            {
                return new RdpVisualizationStep(_points, _segments, _current.Next, _epsilon);
            }

            var listCopy = new LinkedList<int>(_segments);
            var currentCopy = listCopy.Find(_current.Value);
            var nodeToInsert = new LinkedListNode<int>(farthestPointIndex);
            listCopy.AddAfter(currentCopy, nodeToInsert);
            return new RdpVisualizationStep(_points, listCopy, currentCopy, _epsilon);
        }
    }
}
