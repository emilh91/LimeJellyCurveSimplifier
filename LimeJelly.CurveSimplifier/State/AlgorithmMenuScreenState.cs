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
                "Reumann-Witkam",
                "Opheim",
                "Lang",
            };
        }

        public override void KeyDown(KeyEventArgs e)
        {
            base.KeyDown(e);

            if (e.KeyCode == Keys.D1 || e.KeyCode == Keys.NumPad1)
            {
                var input = "20";
                var validInput = 0;

                do
                {
                    const string text = "Enter a tolerance value below. This is used by the Ramer-Douglas-Peucker algorithm as the epsilon.";
                    input = Microsoft.VisualBasic.Interaction.InputBox(text, "Prompt", input);
                    if (input == "") break;
                } while (!int.TryParse(input, out validInput) || validInput < 0);

                if (input == "") return;

                var simpl = new RdpCurveSimplifier(_points, validInput);
                PushState(new VisualizerScreenState(simpl));
            }
            else if (e.KeyCode == Keys.D2 || e.KeyCode == Keys.NumPad2)
            {
                var input = "1000";
                var validInput = 0;

                do
                {
                    const string text = "Enter a tolerance value below. This is used by the Vasvalingam algorithm as the epsilon.";
                    input = Microsoft.VisualBasic.Interaction.InputBox(text, "Prompt", input);
                    if (input == "") break;
                } while (!int.TryParse(input, out validInput) || validInput < 0);

                if (input == "") return;

                var simpl = new VisvalingamCurveSimplifier(_points, validInput);
                PushState(new VisualizerScreenState(simpl));
            }
            else if (e.KeyCode == Keys.D3 || e.KeyCode == Keys.NumPad3)
            {
                var input = "50";
                var validInput = 0;

                do
                {
                    const string text = "Enter a tolerance value below. This is used by the Reumann Witkam algorithm as the epsilon.";
                    input = Microsoft.VisualBasic.Interaction.InputBox(text, "Prompt", input);
                    if (input == "") break;
                } while (!int.TryParse(input, out validInput) || validInput < 0);

                if (input == "") return;

                var simpl = new ReumannWitkamCurveSimplifier(_points, validInput);
                PushState(new VisualizerScreenState(simpl));
            }
            else if (e.KeyCode == Keys.D4 || e.KeyCode == Keys.NumPad4)
            {
                var input = "20 50";
                string input1 = "", input2 = "";
                int validInput1 = 0, validInput2 = 0;

                do
                {
                    const string text =
                        "Enter two tolerance values below (separated by a space). " +
                        "These are used by the Opheim algorithm as the perpendicular distance epsilon and the radial distance epsilon, respectively.";
                    input = Microsoft.VisualBasic.Interaction.InputBox(text, "Prompt", input);
                    if (input == "") break;

                    var inputs = input.Split(' ');
                    if (inputs.Length != 2) continue;

                    input1 = inputs[0];
                    input2 = inputs[1];
                } while (!int.TryParse(input1, out validInput1) || !int.TryParse(input2, out validInput2) || validInput1 < 0 || validInput2 < 0);

                if (input == "") return;

                var simpl = new OpheimCurveSimplifier(_points, validInput1, validInput2);
                PushState(new VisualizerScreenState(simpl));
            }
            else if (e.KeyCode == Keys.D5 || e.KeyCode == Keys.NumPad5)
            {
                var input = "20";
                var validInput = 0;

                do
                {
                    const string text = "Enter a tolerance value below. This is used by the Lang algorithm as the epsilon.";
                    input = Microsoft.VisualBasic.Interaction.InputBox(text, "Prompt", input);
                    if (input == "") break;
                } while (!int.TryParse(input, out validInput) || validInput < 0);

                if (input == "") return;

                var simpl = new LangCurveSimplifier(_points, validInput);
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
