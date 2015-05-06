using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LimeJelly.CurveSimplifier.Visualization;
using SharpDX;

namespace LimeJelly.CurveSimplifier.Simplification
{
    class VisvalingamCurveSimplifier : BaseCurveSimplifier
    {
        // The same size as Points.
        // Each index contains the triangle whose middle vertex is at that index in Points,
        // or null if no such triangle exists
        private List<Triangle> _triangles; 

        // A sorted set performing the role of a minheap.
        // The top triangle has the smallest area.
        private SortedSet<Triangle> _heap;

        private float _maxAreaToRemove;


        public VisvalingamCurveSimplifier(IEnumerable<Vector2> points, float maxAreaToRemove) : base(points)
        {
            _maxAreaToRemove = maxAreaToRemove;
            // Construct _triangles (being sure to add nulls on either end so that its indices match Points)
            _triangles = new List<Triangle> { null };
            for (int i = 1; i < Points.Count - 1; ++i)
            {
                var tri = new Triangle(i, Points[i - 1], Points[i], Points[i + 1]);
                _triangles.Add(tri);
            }
            _triangles.Add(null);

            // Add every segment in the curve to the initial solution
            AddEntireCurveToSolution();

            // Prepare the heap
            _heap = new SortedSet<Triangle>(_triangles.Where(t => t != null));
        }


        public override IVisualizationStep NextStep()
        {
            // "pop" the triangle with the smallest area from the queue
            var smallest = _heap.FirstOrDefault();
            if (smallest == null || smallest.Area >= _maxAreaToRemove)
                return null; // no more, our work here is done
            _heap.Remove(smallest);

            RemovePointFromSolution(smallest.Index); // remove that triangle's midpoint from the solution...
            _triangles[smallest.Index] = null; // and remove it from the list of active triangles

            //resize the left triangle
            {
                var i = smallest.Index - 1;
                while (i > 0 && _triangles[i] == null) --i;
                var left = _triangles[i];

                if (left != null)
                {
                    // "Changing the sort values of existing items is not supported and may lead to unexpected behavior."
                    // https://msdn.microsoft.com/en-us/library/dd412070(v=vs.110).aspx

                    _heap.Remove(left);
                    left.Resize(left.First, smallest.Last);
                    _heap.Add(left);
                }
            }


            //resize the right triangle
            {
                var i = smallest.Index + 1;
                while (i < Points.Count - 1 && _triangles[i] == null) ++i;
                var right = _triangles[i];

                if (right != null)
                {
                    _heap.Remove(right);
                    right.Resize(smallest.First, right.Last);
                    _heap.Add(right);
                }

            }

            //TODO: prettier visualization
            return Visualize();
        }

        private IVisualizationStep Visualize()
        {
            var smallest = _heap.FirstOrDefault();
            if (smallest == null || smallest.Area > _maxAreaToRemove)
                return new BaseVisualizationStep(Points, SolutionSegments, SolutionPoints);

            return new VisvalingamVisualizationStep(Points, SolutionSegments, SolutionPoints, smallest.Index);
        }


        private class Triangle : IComparable<Triangle>
        {
            public Triangle(int index, Vector2 first, Vector2 middle, Vector2 last)
            {
                Index = index;
                First = first;
                Middle = middle;
                Last = last;
            }
            public int Index { get; private set; }
            public Vector2 First { get; private set; }
            public Vector2 Middle { get; private set; }
            public Vector2 Last { get; private set; }

            public void Resize(Vector2 first, Vector2 last)
            {
                First = first;
                Last = last;
                _area = null;
            }

            public int CompareTo(Triangle other)
            {
                if (other == null) return 1;
                return Area.CompareTo(other.Area);
            }

            private float? _area;
            public float Area
            {
                get
                {
                    if (!_area.HasValue)
                    {
                        _area = Math.Abs(
                            (First.X * (Middle.Y - Last.Y))
                            + (Middle.X * (Last.Y - First.Y))
                            + (Last.X * (First.Y - Middle.Y))) / 2;
                    }
                    return _area.Value;
                }
            }
        }
    }
}
