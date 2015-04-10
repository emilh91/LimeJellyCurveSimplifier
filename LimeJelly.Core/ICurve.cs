using System.Collections.Generic;

namespace LimeJelly.Core
{
    public interface ICurve
    {
        IReadOnlyCollection<IPoint> Points { get; }
    }
}