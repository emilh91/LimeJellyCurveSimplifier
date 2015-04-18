namespace LimeJelly.CurveSimplifier
{
    public interface ICurveSimplifier
    {
        /// <summary>
        /// Returns the sequence of points initially inputted.
        /// </summary>
        ICurve InitialCurve { get; }

        /// <returns>
        /// A simplification of this curve with the specified epsilon value.
        /// </returns>
        ICurve Simplify(double epsilon);
    }
}
