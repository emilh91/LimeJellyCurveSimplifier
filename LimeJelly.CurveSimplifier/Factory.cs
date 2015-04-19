using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using LimeJelly.CurveSimplifier.Simplifier;
using SharpDX;

namespace LimeJelly.CurveSimplifier
{
    public static class Factory
    {
        public static IEnumerable<Vector3> PointsFromFile(string filePath, char delimiter = ',')
        {
            return File.ReadLines(filePath)
                .Select(ln => ln.Split(delimiter).Select(Single.Parse).ToArray())
                .Select(sa => new Vector3(sa[0], sa[1], 0));
        }

        public static IEnumerable<Vector3> RandomPoints(float xMax = 1000, float yMax = 1000)
        {
            var rand = new Random();
            while (true)
            {
                var x = rand.NextFloat(0, xMax);
                var y = rand.NextFloat(0, yMax);
                yield return new Vector3(x, y, 0);
            }
        }

        public static Curve ToCurve(this IEnumerable<Vector3> source)
        {
            return new Curve(source);
        }

        public static ICurveSimplifier DefaultSimplifier(Curve curve)
        {
            return new RamerDouglasPeuckerSimplifier(curve);
        }

        public static ICurveSimplifier MemoizedSimplifier(Curve curve)
        {
            return new RamerDouglasPeuckerSimplifier(curve);
        }
    }
}
