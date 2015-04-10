namespace LimeJelly.Core.Impl
{
    class Point2D : IPoint
    {
        public double X { get; private set; }

        public double Y { get; private set; }

        internal Point2D(double x, double y)
        {
            X = x;
            Y = y;
        }
    }
}
