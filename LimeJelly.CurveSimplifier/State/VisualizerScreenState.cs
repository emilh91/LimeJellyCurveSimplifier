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
        private readonly MemoizedCurveSimplifier _simplifier;
        private Curve _currentCurve;

        public VisualizerScreenState(IContentManager cm, Curve inputCurve)
            : base(cm)
        {
            _arial16 = ContentManager.Load<SpriteFont>(@"Font\Arial16");
            _simplifier = new RamerDouglasPeuckerSimplifier(inputCurve);
            _currentCurve = inputCurve;
        }

        public override void Update(GameTime gameTime, KeyboardState keyboard, MouseState mouse)
        {
            base.Update(gameTime, keyboard, mouse);
            if (IsPaused) return;

            if (gameTime.FrameCount%60 == 0)
            {
                _currentCurve = _simplifier.Simplify(0.25);
            }
        }

        public override void Draw(GameTime gameTime, SpriteBatch batch)
        {
            base.Draw(gameTime, batch);

            var text = string.Format("{0} point(s)", _currentCurve.Points.Count);
            batch.DrawString(_arial16, text, Vector2.Zero, Color.Black);
        }

        public override void Draw(GameTime gameTime, PrimitiveBatch<VertexPositionColor> batch)
        {
            base.Draw(gameTime, batch);

            var width = batch.GraphicsDevice.Viewport.Width;
            var height = batch.GraphicsDevice.Viewport.Height;
            var vertices = _currentCurve.Points.Select(vec3 => new Vector3(vec3.X*width, vec3.Y*height, 0));

            var lineStrip = vertices.Select(vec3 => new VertexPositionColor(vec3, Color.Black)).ToArray();
            batch.Draw(PrimitiveType.LineStrip, lineStrip);

            var points = vertices.Select(vec3 => new VertexPositionColor(vec3, Color.Red)).ToArray();
            batch.Draw(PrimitiveType.PointList, points);
        }
    }
}
