using System.Linq;
using System.Windows.Forms;
using SharpDX;
using SharpDX.Direct2D1;

namespace LimeJelly.CurveSimplifier.State
{
    class MainMenuScreenState : ScreenState
    {
        private readonly string[] _lines;

        public MainMenuScreenState()
        {
            _lines = new []
            {
                "[D]raw curve",
                "[R]andomly generate curve",
                "[A]bout",
            };
        }

        public override void KeyDown(KeyEventArgs e)
        {
            base.KeyDown(e);

            if (e.KeyCode == Keys.A)
            {
                PushState(new AboutScreenState());
            }
            else if (e.KeyCode == Keys.D)
            {
                PushState(new CurveDrawerScreenState());
            }
            else if (e.KeyCode == Keys.R)
            {
                var points = InputFactory.RandomPoints().TakeWhile(p => p.X < 760);
                PushState(new CurveDrawerScreenState(points));
            }
        }

        public override void Draw(RenderTarget renderTarget, ResourceFactory rf)
        {
            renderTarget.Clear(Color.White);

            var font = rf.GetFont("Arial", 16);
            var brush = rf.GetSolidColorBrush(Color.Black);

            for (var i = 0; i < _lines.Length; i++)
            {
                const int leftMargin = 10;
                var rect = new RectangleF(leftMargin, 50 + i * 30, renderTarget.Size.Width - leftMargin, 30);
                renderTarget.DrawText(_lines[i], font, rect, brush);
            }
        }
    }
}
