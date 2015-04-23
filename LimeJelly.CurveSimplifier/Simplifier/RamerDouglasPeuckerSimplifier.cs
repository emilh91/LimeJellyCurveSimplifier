using System.Collections.Generic;
using System.Linq;
using SharpDX;

namespace LimeJelly.CurveSimplifier.Simplifier
{
    class RamerDouglasPeuckerSimplifier : MemoizedCurveSimplifier
    {
        public float Epsilon { get; private set; }

        internal RamerDouglasPeuckerSimplifier(IEnumerable<Vector3> points, float epsilon) : base(points)
        {
            Epsilon = epsilon;
            
            var queue = new Queue<BinaryTreeNode<VisualizationStep>>();
            var root = new BinaryTreeNode<VisualizationStep>(InitialVisualizationStep);
            queue.Enqueue(root);

            while (queue.Any())
            {
                var node = queue.Dequeue();
                if (node == null) continue;
                if (Saturate(node, Epsilon))
                {
                    queue.Enqueue(node.Left);
                    queue.Enqueue(node.Right);
                }
                else
                {
                    Steps.Add(node.Data);
                }
            }
        }

        public override void Simplify()
        {
        }

        private static bool Saturate(BinaryTreeNode<VisualizationStep> node, float epsilon)
        {
            var points = node.Data.Points;
            if (points.Count <= 2) return false;

            var farthest = 0;
            var farthestDist = epsilon;
            var start = points.First(); var end = points.Last();

            // Find the point farthest from the line containing both endpoints of the curve.
            for (var i = 1; i < points.Count - 1; ++i)
            {
                var dist = points.ElementAt(i).DistanceToLine(start, end);
                if (dist > farthestDist)
                {
                    farthest = i;
                    farthestDist = dist;
                }
            }

            if (farthestDist <= epsilon)
            {
                // No point in this region is farther than epsilon from the curve,
                // so we discard everything but the endpoints.
                var keptPoints = new[] {start, end};
                var removedPoints = points.Skip(1).Take(points.Count - 1);
                var vs = new VisualizationStep(keptPoints, removedPoints);
                node.Left = new BinaryTreeNode<VisualizationStep>(vs);
            }
            else
            {
                // Recurse on the sections before and after the farthest point,
                // and be sure to keep the farthest point too (which is definitely > epsilon).
                var vsLeft = new VisualizationStep(points.Take(farthest + 1));
                node.Left = new BinaryTreeNode<VisualizationStep>(vsLeft);

                var vsRight = new VisualizationStep(points.Skip(farthest));
                node.Right = new BinaryTreeNode<VisualizationStep>(vsRight);
            }

            return true;
        }
    }
}
