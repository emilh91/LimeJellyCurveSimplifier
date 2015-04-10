using System;
using System.Collections.Generic;
using System.Linq;

namespace LimeJelly.Core.Impl
{
    class MemoizedCurveSimplifier : ICurveSimplifier
    {
        private IList<ICurve> _simplifications;

        public ICurve InitialCurve { get { return _simplifications[0]; } }

        public int NumIterations { get { return _simplifications.Count - 1; } }

        public ICurve CurrentIteration { get { return _simplifications.Last(); } }

        internal MemoizedCurveSimplifier(ICurve curve)
        {
            _simplifications = new List<ICurve> {curve};
        }

        public ICurve CurveAtIteration(int iter)
        {
            return _simplifications[iter];
        }

        public bool Simplify()
        {
            throw new NotImplementedException();
        }
    }
}
