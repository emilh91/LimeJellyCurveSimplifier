using System.Collections.Generic;
using SharpDX;

namespace LimeJelly.CurveSimplifier
{
    public interface ICurve
    {
        IReadOnlyCollection<Vector3> Points { get; }
    }
}