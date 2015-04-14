using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LimeJelly.Core.Impl
{
    class RamerDouglasPeuckerSimplifier : MemoizedCurveSimplifier
    {
        internal RamerDouglasPeuckerSimplifier(ICurve curve) : base(curve) { }

        protected override ICurve SimplifyImpl(ICurve curve, double epsilon)
        {
            return curve; //wgas
        }
    }
}
