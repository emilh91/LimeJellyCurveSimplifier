using System.Collections.Generic;
using System.Linq;
using SharpDX;
using SharpDX.Direct2D1;

namespace LimeJelly.CurveSimplifier.Geometry
{
    public class Polygon
    {
        public Polygon(IEnumerable<Vector2> points, Color color)
        {
            _points = points.ToArray();
            Color = color;
        }

        public Polygon(Color color, params Vector2[] points)
        {
            _points = points;
            Color = color;
        }

        private Vector2[] _points;
        public IEnumerable<Vector2> Points { get { return _points; } }
        public Color Color { get; private set; }

        private PathGeometry _geometry;
        public SharpDX.Direct2D1.Geometry GetGeometry(Factory factory)
        {
            if (_geometry != null)
                return _geometry;

            _geometry = new PathGeometry(factory);
            using (var sink = _geometry.Open())
            {
                sink.BeginFigure(_points.Last(), FigureBegin.Filled);
                sink.AddLines(_points);
                sink.EndFigure(FigureEnd.Closed);
                sink.Close();
            }
            return _geometry;
        }
    }
}
