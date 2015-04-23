using System.Collections.Generic;
using System.Linq;
using SharpDX;

namespace LimeJelly.CurveSimplifier.Simplifier
{
    abstract class MemoizedCurveSimplifier
    {
        protected readonly IList<VisualizationStep> Steps;

        public int NumSteps { get { return Steps.Count - 1; } }

        public VisualizationStep InitialVisualizationStep { get { return Steps[0]; } }

        public VisualizationStep LastVisualizationStep { get { return Steps[NumSteps]; } }

        internal MemoizedCurveSimplifier(IEnumerable<Vector3> points)
        {
            Steps = new List<VisualizationStep>
            {
                new VisualizationStep(points, Enumerable.Empty<Vector3>())
            };
        }

        public VisualizationStep VisualizationAtStep(int step)
        {
            return Steps[step];
        }

        public abstract void Simplify();
    }
}
