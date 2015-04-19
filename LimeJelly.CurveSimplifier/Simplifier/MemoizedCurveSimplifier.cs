using System.Collections.Generic;

namespace LimeJelly.CurveSimplifier.Simplifier
{
    abstract class MemoizedCurveSimplifier : ICurveSimplifier
    {
        private readonly IDictionary<double, Curve> _simplifications;

        internal MemoizedCurveSimplifier(Curve curve)
        {
            _simplifications = new Dictionary<double, Curve> { { 0, curve } };
        }

        public Curve InitialCurve
        {
            get { return _simplifications[0]; }
        }

        public Curve Simplify(double epsilon)
        {
            Curve result;
            if (_simplifications.TryGetValue(epsilon, out result))
                return result;

            result = SimplifyImpl(_simplifications[0], epsilon);
            _simplifications.Add(epsilon, result);
            return result;
            
        }

        protected abstract Curve SimplifyImpl(Curve curve, double epsilon);
    }
}
