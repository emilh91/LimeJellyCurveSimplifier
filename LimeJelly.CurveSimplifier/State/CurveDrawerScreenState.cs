using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using SharpDX;
using SharpDX.Direct2D1;

namespace LimeJelly.CurveSimplifier.State
{
    class CurveDrawerScreenState : ScreenState
    {
        private readonly List<Vector2> _points;

        public CurveDrawerScreenState(IEnumerable<Vector2> points = null)
        {
            _points = (points ?? Enumerable.Empty<Vector2>()).ToList();
        }

        public override void KeyUp(KeyEventArgs e)
        {
            base.KeyUp(e);

            if (e.KeyCode == Keys.S)
            {
                var dlg = new SaveFileDialog();
                dlg.Filter = "Text Files | *.txt";
                dlg.DefaultExt = "txt";
                dlg.FileName = "ljcs-" + DateTime.Now.ToString("yyyyMMdd-HHmmss");
                dlg.FileOk += (sender, args) =>
                {
                    var data = _points.Select(p => string.Format("{0},{1}", p.X, p.Y));
                    File.WriteAllLines(dlg.FileName, data);
                };
                dlg.ShowDialog();
            }
            else if (e.KeyCode == Keys.V)
            {
                if (_points.Any())
                {
                    PushState(new AlgorithmMenuScreenState(_points));
                }
            }
            else if (e.KeyCode == Keys.Z)
            {
                if (_points.Any())
                {
                    _points.RemoveAt(_points.Count - 1);
                }
            }
        }

        public override void MouseDown(MouseEventArgs e)
        {
            base.MouseDown(e);

            var vec2 = new Vector2(e.X, e.Y);
            if (_points.Count == 0 || _points.Last() != vec2)
            {
                _points.Add(vec2);
            }
        }

        public override void Draw(RenderTarget renderTarget, ResourceFactory rf)
        {
            renderTarget.Clear(Color.White);

            var brush = rf.GetSolidColorBrush(Color.Black);

            for (var i = 1; i < _points.Count; i++)
            {
                renderTarget.DrawLine(_points[i - 1], _points[i], brush);
            }

            foreach (var vec2 in _points)
            {
                renderTarget.FillEllipse(new Ellipse(vec2, 2, 2), brush);
            }

            var font = rf.GetFont("Arial", 16);
            var rect = new RectangleF(0, 0, renderTarget.Size.Width, 30);
            renderTarget.DrawText(_points.Count + " point(s)", font, rect, brush);
        }

        protected override void Reset()
        {
            base.Reset();

            _points.Clear();
        }
    }
}
