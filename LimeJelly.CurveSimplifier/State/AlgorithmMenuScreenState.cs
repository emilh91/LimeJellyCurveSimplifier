using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using LimeJelly.CurveSimplifier.Simplification;
using SharpDX;
using SharpDX.Direct2D1;

namespace LimeJelly.CurveSimplifier.State
{
    class AlgorithmMenuScreenState : ScreenState
    {
        private readonly List<Vector2> _points;
        private readonly string[] _lines;

        public AlgorithmMenuScreenState(IEnumerable<Vector2> points)
        {
            _points = points.ToList();

            _lines = new[]
            {
                "Ramer-Douglas-Peucker",
                "Visvalingam",
                "Reumann-Witkam"
            };
        }

        public override void KeyDown(KeyEventArgs e)
        {
            base.KeyDown(e);

            if (e.KeyCode == Keys.D1 || e.KeyCode == Keys.NumPad1)
            {
                var simpl = new RdpCurveSimplifier(_points, 20);
                PushState(new VisualizerScreenState(simpl));
            }
            else if (e.KeyCode == Keys.D2 || e.KeyCode == Keys.NumPad2)
            {
                var simpl = new VisvalingamCurveSimplifier(_points, 1000);
                PushState(new VisualizerScreenState(simpl));
            }
            else if (e.KeyCode == Keys.D3 || e.KeyCode == Keys.NumPad3)
            {
                var simpl = new ReumannWitkamCurveSimplifier(_points, 50f);
                PushState(new VisualizerScreenState(simpl));
            }
        }

        public override void Draw(RenderTarget renderTarget, ResourceFactory rf)
        {
            renderTarget.Clear(Color.White);

            var font = rf.GetFont("Arial", 16);
            var brush = rf.GetSolidColorBrush(Color.Black);

            var rect = new RectangleF(0, 0, renderTarget.Size.Width, 30);
            renderTarget.DrawText(_points.Count + " point(s)", font, rect, brush);

            for (var i = 0; i < _lines.Length; i++)
            {
                const int leftMargin = 10;
                rect = new RectangleF(leftMargin, 50 + i * 30, renderTarget.Size.Width - leftMargin, 30);
                var line = string.Format("{0}. {1}", i + 1, _lines[i]);
                renderTarget.DrawText(line, font, rect, brush);
            }
        }
    }
}
