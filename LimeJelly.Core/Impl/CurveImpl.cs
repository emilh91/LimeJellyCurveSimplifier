using System.Collections.Generic;
using System.Linq;

namespace LimeJelly.Core.Impl
{
    class CurveImpl : ICurve
    {
        public IReadOnlyCollection<IPoint> Points { get; private set; }

        internal CurveImpl(IEnumerable<IPoint> points)
        {
            Points = points.ToList().AsReadOnly();
        }
    }
}