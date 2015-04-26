using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using SharpDX;

namespace LimeJelly.CurveSimplifier
{
    static class InputFactory
    {
        public static IEnumerable<Vector2> PointsFromFile(string filePath, char delimiter = ',')
        {
            return File.ReadLines(filePath)
                .Select(ln => ln.Split(delimiter).Select(Single.Parse).ToArray())
                .Select(sa => new Vector2(sa[0], sa[1]));
        }

        public static IEnumerable<Vector2> RandomPoints(float xMax = 1000, float yMax = 1000)
        {
            var rand = new Random();
            while (true)
            {
                var x = rand.NextFloat(0, xMax);
                var y = rand.NextFloat(0, yMax);
                yield return new Vector2(x, y);
            }
        }
    }
}
