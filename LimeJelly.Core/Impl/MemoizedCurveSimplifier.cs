using System;
using System.Collections.Generic;
using System.Linq;

namespace LimeJelly.Core.Impl
{
    abstract class MemoizedCurveSimplifier : ICurveSimplifier
    {
        private IDictionary<double, ICurve> _simplifications;

        internal MemoizedCurveSimplifier(ICurve curve)
        {
            _simplifications = new Dictionary<double, ICurve> { { 0, curve } };
        }

        public ICurve InitialCurve
        {
            get { return _simplifications[0]; }
        }

        public ICurve Simplify(double epsilon)
        {
            ICurve result;
            if (_simplifications.TryGetValue(epsilon, out result))
                return result;

            result = SimplifyImpl(_simplifications[0], epsilon);
            _simplifications.Add(epsilon, result);
            return result;
            
        }

        protected abstract ICurve SimplifyImpl(ICurve curve, double epsilon);
    }
}
