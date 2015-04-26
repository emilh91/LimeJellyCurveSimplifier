using System;
using System.Collections.Generic;
using SharpDX;

namespace LimeJelly.CurveSimplifier.Visualization
{
    interface IVisualizationStep
    {
        IEnumerable<Tuple<Vector2, Vector2, Color, float>> GetSegments();
        
        IEnumerable<Tuple<Vector2, Color>> GetPoints();
        
        IVisualizationStep NextStep();
    }
}
