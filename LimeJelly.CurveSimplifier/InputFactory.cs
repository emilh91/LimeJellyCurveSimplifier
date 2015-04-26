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

        public static IEnumerable<Vector2> RandomPoints()
        {
            var rand = new Random();

            var x = rand.NextFloat(0, 15);
            while (true)
            {
                x = rand.NextFloat(x + 15, x + 30);
                var y = rand.NextFloat(100, 500);
                yield return new Vector2(x, y);
            }
        }
    }
}
