using System;

namespace LimeJelly.Core.Impl
{
    class DefaultCurveSimplifier : ICurveSimplifier
    {
        public ICurve InitialCurve { get; private set; }

        public int NumIterations { get; private set; }

        public ICurve CurrentIteration { get; private set; }

        internal DefaultCurveSimplifier(ICurve curve)
        {
            InitialCurve = curve;
            CurrentIteration = curve;
        }

        public ICurve CurveAtIteration(int iter)
        {
            throw new NotSupportedException();
        }

        public bool Simplify()
        {
            throw new NotImplementedException();
        }
    }
}
