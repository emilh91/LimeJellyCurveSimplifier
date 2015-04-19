using System.Collections.Generic;
using System.Linq;
using SharpDX;

namespace LimeJelly.CurveSimplifier
{
    public class Curve
    {
        public IReadOnlyCollection<Vector3> Points { get; private set; }

        internal Curve(IEnumerable<Vector3> points)
        {
            Points = points.ToList().AsReadOnly();
        }
    }
}