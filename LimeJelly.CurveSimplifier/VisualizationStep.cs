using System.Collections.Generic;
using System.Linq;
using SharpDX;

namespace LimeJelly.CurveSimplifier
{
    public class VisualizationStep
    {
        public IReadOnlyCollection<Vector3> Points { get; private set; }

        public IReadOnlyCollection<Vector3> RemovedPoints { get; private set; }

        internal VisualizationStep(IEnumerable<Vector3> points, IEnumerable<Vector3> removedPoints=null)
        {
            Points = points.ToList().AsReadOnly();
            RemovedPoints = (removedPoints ?? Enumerable.Empty<Vector3>()).ToList().AsReadOnly();
        }
    }
}