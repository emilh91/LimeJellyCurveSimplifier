using System.Collections.Generic;
using System.Linq;
using SharpDX;
using SharpDX.Toolkit;
using SharpDX.Toolkit.Graphics;
using SharpDX.Toolkit.Input;

namespace LimeJelly.CurveSimplifier.State
{
    class CurveDrawerScreenState : ScreenState
    {
        private readonly List<Vector2> _vertices = new List<Vector2>();

        public override void Update(GameTime gameTime, KeyboardState keyboard, MouseState mouse)
        {
            if (keyboard.IsKeyPressed(Keys.Escape))
            {
                PopState();
            }

            if (keyboard.IsKeyPressed(Keys.R))
            {
                _vertices.Clear();
            }

            if (keyboard.IsKeyPressed(Keys.Z))
            {
                if (_vertices.Any())
                {
                    _vertices.RemoveAt(_vertices.Count - 1);
                }
            }

            if (mouse.LeftButton.Down)
            {
                var vec2 = new Vector2(mouse.X, mouse.Y);
                if (_vertices.Count == 0 || _vertices.Last() != vec2)
                {
                    _vertices.Add(vec2);
                }
            }
        }

        public override void Draw(GameTime gameTime, PrimitiveBatch<VertexPositionColor> batch)
        {
            var width = batch.GraphicsDevice.Viewport.Width;
            var height = batch.GraphicsDevice.Viewport.Height;
            var vertices = _vertices
                    .Select(vec2 => new Vector3(vec2.X*width, vec2.Y*height, 0))
                    .Select(vec3 => new VertexPositionColor(vec3, Color.Black))
                    .ToArray();

            if (_vertices.Count == 1)
            {
                batch.Draw(PrimitiveType.PointList, vertices);
            }
            else if (_vertices.Count > 1)
            {
                batch.Draw(PrimitiveType.LineStrip, vertices);
            }
        }
    }
}
