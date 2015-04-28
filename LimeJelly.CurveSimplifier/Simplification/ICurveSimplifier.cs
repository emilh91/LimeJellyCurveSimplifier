using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using LimeJelly.CurveSimplifier.Visualization;

namespace LimeJelly.CurveSimplifier.Simplification
{
    interface ICurveSimplifier
    {
        /// <summary>
        /// Displays the curve initially passed to this simplifier
        /// </summary>
        IVisualizationStep Initial();

        /// <summary>
        /// Computes another "step" of an algorithm, returning a visualization of that step.
        /// Returns null once the solution has been found.
        /// </summary>
        IVisualizationStep NextStep();
    }
}
