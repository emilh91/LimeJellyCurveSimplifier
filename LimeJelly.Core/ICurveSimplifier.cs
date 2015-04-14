using System;

namespace LimeJelly.Core
{
    public interface ICurveSimplifier
    {
        /// <summary>
        /// Returns the sequence of points initially inputted.
        /// </summary>
        ICurve InitialCurve { get; }

        /// <returns>
        ///     The sequence of points at the specified iteration:
        ///     - the initial iteration if iter == 0
        ///     - the current iteration if iter == NumIterations
        /// </returns>
        /// <exception cref="ArgumentOutOfRangeException">
        ///     Thrown when iter &lt; 0 || iter &gt; NumIterations
        /// </exception>
        ICurve Simplify(double epsilon);
    }
}
