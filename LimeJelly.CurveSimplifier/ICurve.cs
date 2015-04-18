using System.Collections.Generic;

namespace LimeJelly.CurveSimplifier
{
    public interface ICurve
    {
        IReadOnlyCollection<IPoint> Points { get; }
    }
}