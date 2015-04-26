using SharpDX;
using SharpDX.Direct2D1;

namespace LimeJelly.CurveSimplifier.State
{
    class AboutScreenState : ScreenState
    {
        private readonly string[] _lines;

        public AboutScreenState()
        {
            _lines = new []
            {
                "LimeJelly Curve Simplifier",
                "", "",
                "NYU Polytechnic School of Engineering",
                "CS6703 - Computational Geometry",
                "Spring 2015",
                "", "",
                "Brought to you by:",
                "- Emil \"Lime\" Huseynaliev",
                "- Simon \"Jelly\" Gellis",
            };
        }

        public override void Update()
        {
            base.Update();

            if (FrameCount%90 == 0)
            {
                const int i = 9, j = 10;
                var tmp = _lines[i];
                _lines[i] = _lines[j];
                _lines[j] = tmp;
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
                var rect = new RectangleF(leftMargin, 50+i*30, renderTarget.Size.Width-leftMargin, 30);
                renderTarget.DrawText(_lines[i], font, rect, brush);
            }
        }
    }
}
