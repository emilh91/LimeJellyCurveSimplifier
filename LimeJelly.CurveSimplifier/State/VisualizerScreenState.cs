﻿using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;
using LimeJelly.CurveSimplifier.Visualization;
using SharpDX;
using SharpDX.Direct2D1;
using SharpDX.DirectWrite;

namespace LimeJelly.CurveSimplifier.State
{
    class VisualizerScreenState : ScreenState
    {
        private readonly List<IVisualizationStep> _steps;
        private int _currentStepIndex;
        private bool _visualizationFinished;
        
        public VisualizerScreenState(IVisualizationStep vs)
        {
            _steps = new List<IVisualizationStep> {vs};
        }

        public override void KeyUp(KeyEventArgs e)
        {
            base.KeyUp(e);
            if (!IsPaused) return;

            if (e.KeyCode == Keys.Left)
            {
                if (_currentStepIndex > 0)
                {
                    _currentStepIndex--;
                }
            }
            else if (e.KeyCode == Keys.Right)
            {
                GoForward();
            }
        }

        public override void Update()
        {
            base.Update();
            if (IsPaused) return;

            if (FrameCount % 60 == 0)
            {
                GoForward();
            }
        }

        public override void Draw(RenderTarget renderTarget, ResourceFactory rf)
        {
            renderTarget.Clear(Color.White);

            var step = _steps[_currentStepIndex];

            foreach (var segment in step.GetSegments())
            {
                var brush = rf.GetSolidColorBrush(segment.Item3);
                renderTarget.DrawLine(segment.Item1, segment.Item2, brush, segment.Item4);
            }

            var points = step.GetPoints().ToList();
            foreach (var point in points)
            {
                var brush = rf.GetSolidColorBrush(point.Item2);
                renderTarget.FillEllipse(new Ellipse(point.Item1, 2, 2), brush);
            }

            var font = rf.GetFont("Arial", 16);
            var rect = new RectangleF(0, 0, renderTarget.Size.Width, 100);

            var text1 = string.Format("Initially: {0} point(s)", _steps.First().GetPoints().Count());
            font.TextAlignment = TextAlignment.Leading;
            renderTarget.DrawText(text1, font, rect, rf.GetSolidColorBrush(Color.Black));

            var text2 = string.Format("Step {0}: {1} point(s)", _currentStepIndex, points.Count(p => p.Item2==Color.Black));
            font.TextAlignment = TextAlignment.Center;
            renderTarget.DrawText(text2, font, rect, rf.GetSolidColorBrush(Color.Black));

            if (_visualizationFinished)
            {
                var text3 = string.Format("Step {0}: {1} point(s)", _steps.Count - 1, _steps.Last().GetPoints().Count(p => p.Item2==Color.Black));
                font.TextAlignment = TextAlignment.Trailing;
                renderTarget.DrawText(text3, font, rect, rf.GetSolidColorBrush(Color.Black));
            }
            else if (IsPaused)
            {
                var text3 = string.Format("Paused; computed {0} step(s) so far", _steps.Count - 1);
                font.TextAlignment = TextAlignment.Trailing;
                renderTarget.DrawText(text3, font, rect, rf.GetSolidColorBrush(Color.Black));
            }
        }

        protected override void Reset()
        {
            base.Reset();

            _currentStepIndex = 0;
        }

        private void GoForward()
        {
            if (_currentStepIndex == _steps.Count - 1)
            {
                var nextStep = _steps.Last().NextStep();
                _visualizationFinished = nextStep == null;
                if (!_visualizationFinished)
                {
                    _steps.Add(nextStep);
                    _currentStepIndex++;
                }
            }
            else
            {
                _currentStepIndex++;
            }
        }
    }
}