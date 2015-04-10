using System;

namespace LimeJelly.Core
{
    public interface ICurveSimplifier
    {
        /// <summary>
        ///     Returns the sequence of points initially inputted.
        /// </summary>
        ICurve InitialCurve { get; }

        /// <summary>
        ///     Returns the number of simplifications that were performed on this object.
        /// </summary>
        int NumIterations { get; }

        /// <summary>
        ///     Returns the sequence of points that was the result of the last simplification iteration
        ///     or, if no simplifications have been performed - the initial sequence of points.
        /// </summary>
        ICurve CurrentIteration { get; }

        /// <summary>
        ///     <returns>
        ///         The sequence of points at the specified iteration:
        ///         - the initial iteration if iter == 0
        ///         - the current iteration if iter == NumIterations
        ///     </returns>
        ///     <exception cref="ArgumentOutOfRangeException">
        ///         Thrown when iter &lt; 0 || iter &gt; NumIterations
        ///     </exception>
        ///     <exception cref="NotSupportedException">
        ///         Thrown when the implementation does not support memoization.
        ///     </exception>
        /// </summary>
        ICurve CurveAtIteration(int iter);

        /// <summary>
        ///     Simplifies the current sequence of points to form a new curve
        ///     or does nothing if there are no further simplifications that can be made.
        /// </summary>
        /// <returns>
        ///     true if a new simplification was generated; false otherwise
        /// </returns>
        bool Simplify();
    }
}
