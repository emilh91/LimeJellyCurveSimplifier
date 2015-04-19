namespace LimeJelly.CurveSimplifier.Simplifier
{
    public interface ICurveSimplifier
    {
        /// <summary>
        /// Returns the sequence of points initially inputted.
        /// </summary>
        Curve InitialCurve { get; }

        /// <returns>
        /// A simplification of this curve with the specified epsilon value.
        /// </returns>
        Curve Simplify(double epsilon);
    }
}
