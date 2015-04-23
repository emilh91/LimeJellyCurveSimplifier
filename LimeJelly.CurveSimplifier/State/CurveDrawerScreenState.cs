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
        private readonly List<Vector2> _vertices = new List<Vector2>();

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
                var points = _vertices.Select(vec2 => (Vector3)vec2);
                PushState(new VisualizerScreenState(ContentManager, points));
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
                var vec2 = new Vector2(mouse.X, mouse.Y);
                if (_vertices.Count == 0 || _vertices.Last() != vec2)
                {
                    _vertices.Add(vec2);
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
            var vertices = _vertices.Select(vec2 => new Vector3(vec2.X*width, vec2.Y*height, 0));

            if (_vertices.Count >= 1)
            {
                var lineStrip = vertices.Select(vec3 => new VertexPositionColor(vec3, Color.White)).ToArray();
                batch.Draw(PrimitiveType.LineStrip, lineStrip);

                var points = vertices.Select(vec3 => new VertexPositionColor(vec3, Color.Red)).ToArray();
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
