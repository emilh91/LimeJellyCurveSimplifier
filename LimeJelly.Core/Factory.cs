using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using LimeJelly.Core.Impl;

namespace LimeJelly.Core
{
    public static class Factory
    {
        public static IEnumerable<IPoint> PointsFromFile(string filePath, char delimiter = ',')
        {
            return File.ReadLines(filePath)
                .Select(ln => ln.Split(delimiter).Select(double.Parse).ToArray())
                .Select(sa => new Point2D(sa[0], sa[1]));
        }

        public static IEnumerable<IPoint> RandomPoints(double xMax=1000, double yMax=1000)
        {
            var rand = new Random();
            while (true)
            {
                var x = rand.NextDouble()*xMax;
                var y = rand.NextDouble()*yMax;
                yield return new Point2D(x, y);
            }
        }

        public static ICurve ToCurve(this IEnumerable<IPoint> source)
        {
            return new CurveImpl(source);
        }

        public static ICurveSimplifier DefaultSimplifier(ICurve curve)
        {
            return new DefaultCurveSimplifier(curve);
        }

        public static ICurveSimplifier MemoizedSimplifier(ICurve curve)
        {
            return new MemoizedCurveSimplifier(curve);
        }
    }
}
