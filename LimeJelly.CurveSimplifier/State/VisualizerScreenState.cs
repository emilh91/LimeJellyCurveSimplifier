using System.Collections.Generic;
using System.Linq;
using LimeJelly.CurveSimplifier.Simplifier;
using SharpDX;
using SharpDX.Toolkit;
using SharpDX.Toolkit.Content;
using SharpDX.Toolkit.Graphics;
using SharpDX.Toolkit.Input;

namespace LimeJelly.CurveSimplifier.State
{
    class VisualizerScreenState : ScreenState
    {
        private readonly SpriteFont _arial16;
        private readonly RamerDouglasPeuckerSimplifier _simplifier;
        private int _currentStepIndex;
        
        public VisualizerScreenState(IContentManager cm, IEnumerable<Vector3> points)
            : base(cm)
        {
            _arial16 = ContentManager.Load<SpriteFont>(@"Font\Arial16");
            _simplifier = new RamerDouglasPeuckerSimplifier(points, 0.1f);
        }

        public override void Update(GameTime gameTime, KeyboardState keyboard, MouseState mouse)
        {
            base.Update(gameTime, keyboard, mouse);

            if (IsPaused)
            {
                if (keyboard.IsKeyPressed(Keys.N) && _currentStepIndex < _simplifier.NumSteps)
                {
                    _currentStepIndex++;
                }
            }
            else if (FrameCount%60==0 && _currentStepIndex < _simplifier.NumSteps)
            {
                _currentStepIndex++;
            }
        }

        public override void Draw(GameTime gameTime, SpriteBatch batch)
        {
            base.Draw(gameTime, batch);

            var text = string.Format("{0} point(s); step {1}/{2}",
                _simplifier.NumSteps+1, _currentStepIndex, _simplifier.NumSteps);
            batch.DrawString(_arial16, text, Vector2.Zero, Color.Black);
        }

        public override void Draw(GameTime gameTime, PrimitiveBatch<VertexPositionColor> batch)
        {
            base.Draw(gameTime, batch);

            var width = batch.GraphicsDevice.Viewport.Width;
            var height = batch.GraphicsDevice.Viewport.Height;

            var ils = _simplifier.InitialVisualizationStep.Points
                    .Select(vec3 => new Vector3(vec3.X * width, vec3.Y * height, 0))
                    .Select(vec3 => new VertexPositionColor(vec3, Color.White))
                    .ToArray();
            batch.Draw(PrimitiveType.LineStrip, ils);

            for (var i = 1; i <= _currentStepIndex; i++)
            {
                var ls = _simplifier.VisualizationAtStep(i).Points
                    .Select(vec3 => new Vector3(vec3.X*width, vec3.Y*height, 0))
                    .Select(vec3 => new VertexPositionColor(vec3, Color.Black))
                    .ToArray();
                batch.Draw(PrimitiveType.LineStrip, ls);
            }
        }

        protected override void Reset()
        {
            base.Reset();

            _currentStepIndex = 0;
        }
    }
}
