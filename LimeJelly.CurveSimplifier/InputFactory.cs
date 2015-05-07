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
            var y = 300f;
            while (true)
            {
                x = rand.NextFloat(x + 15, x + 30);
                y = NormallyDistributedValue(rand, y, 75f);
                yield return new Vector2(x, y);
            }
        }

        public static float NormallyDistributedValue(Random rand, float mean, float stdist)
        {
            var result = -1f;
            for(var i = 0; i < 6; ++i)
            {
                result += rand.NextFloat(0f, 1f/3f);
            }
            return (result * stdist) + mean;
        }
    }
}
