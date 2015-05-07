using System;
using System.Collections.Generic;
using LimeJelly.CurveSimplifier.Geometry;
using SharpDX;

namespace LimeJelly.CurveSimplifier.Visualization
{
    interface IVisualizationStep
    {
        IEnumerable<Tuple<Vector2, Color, float>> GetCircles();

        IEnumerable<Tuple<Vector2, Vector2, Color, float>> GetSegments();
        
        IEnumerable<Tuple<Vector2, Color, float>> GetPoints();

        IEnumerable<Polygon> GetPolygons();
    }
}
