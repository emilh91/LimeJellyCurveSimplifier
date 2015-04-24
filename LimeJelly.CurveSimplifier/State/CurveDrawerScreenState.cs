using System.Collections.Generic;
using System.Linq;
using SharpDX;
using SharpDX.Toolkit;
using SharpDX.Toolkit.Content;
using SharpDX.Toolkit.Graphics;
using SharpDX.Toolkit.Input;

namespace LimeJelly.CurveSimplifier.State
{
    class CurveDrawerScreenState : ScreenState
    {
        private readonly SpriteFont _arial16;
        private readonly List<Vector3> _vertices = new List<Vector3>();

        public CurveDrawerScreenState(IContentManager cm) : base(cm)
        {
            _arial16 = ContentManager.Load<SpriteFont>(@"Font\Arial16");
        }

        public override void Update(GameTime gameTime, KeyboardState keyboard, MouseState mouse)
        {
            base.Update(gameTime, keyboard, mouse);
            if (IsPaused) return;

            if (keyboard.IsKeyPressed(Keys.V))
            {
                if (_vertices.Any())
                {
                    PushState(new VisualizerScreenState(ContentManager, _vertices));
                }
            }
            else if (keyboard.IsKeyPressed(Keys.Z))
            {
                if (_vertices.Any())
                {
                    _vertices.RemoveAt(_vertices.Count - 1);
                }
            }
            else if (mouse.LeftButton.Down)
            {
                var vec3 = new Vector3(mouse.X * Width, mouse.Y * Height, 0);
                if (_vertices.Count == 0 || _vertices.Last() != vec3)
                {
                    _vertices.Add(vec3);
                }
            }
        }

        public override void Draw(GameTime gameTime, SpriteBatch batch)
        {
            base.Draw(gameTime, batch);

            var text = string.Format("{0} point(s)", _vertices.Count);
            batch.DrawString(_arial16, text, Vector2.Zero, Color.Black);
        }

        public override void Draw(GameTime gameTime, PrimitiveBatch<VertexPositionColor> batch)
        {
            base.Draw(gameTime, batch);

            var width = batch.GraphicsDevice.Viewport.Width;
            var height = batch.GraphicsDevice.Viewport.Height;

            if (_vertices.Count >= 1)
            {
                var lineStrip = _vertices.Select(vec3 => new VertexPositionColor(vec3, Color.Black)).ToArray();
                batch.Draw(PrimitiveType.LineStrip, lineStrip);

                var points = _vertices.Select(vec3 => new VertexPositionColor(vec3, Color.Red)).ToArray();
                batch.Draw(PrimitiveType.PointList, points);
            }
        }

        protected override void Reset()
        {
            base.Reset();

            _vertices.Clear();
        }
    }
}
