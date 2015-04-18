using System.Collections.Generic;
using System.Linq;
using SharpDX;

namespace LimeJelly.CurveSimplifier.Impl
{
    class CurveImpl : ICurve
    {
        public IReadOnlyCollection<Vector3> Points { get; private set; }

        internal CurveImpl(IEnumerable<Vector3> points)
        {
            Points = points.ToList().AsReadOnly();
        }
    }
}