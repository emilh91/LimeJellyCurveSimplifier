using System;
using System.Threading;
using SharpDX;
using SharpDX.Toolkit;
using SharpDX.Toolkit.Content;
using SharpDX.Toolkit.Graphics;
using SharpDX.Toolkit.Input;

namespace LimeJelly.CurveSimplifier.State
{
    class AboutScreenState : ScreenState
    {
        private readonly SpriteFont _arial16;
        private readonly string[] _lines;

        public AboutScreenState(IContentManager cm) : base(cm)
        {
            _arial16 = ContentManager.Load<SpriteFont>(@"Font\Arial16");

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

        public override void Update(GameTime gameTime, KeyboardState keyboard, MouseState mouse)
        {
            if (keyboard.IsKeyPressed(Keys.Escape))
            {
                PopState();
            }

            if (gameTime.FrameCount%90 == 0)
            {
                const int i = 9, j = 10;
                var tmp = _lines[i];
                _lines[i] = _lines[j];
                _lines[j] = tmp;
                
            }
        }

        public override void Draw(GameTime gameTime, SpriteBatch batch)
        {
            for (var i = 0; i < _lines.Length; i++)
            {
                batch.DrawString(_arial16, _lines[i], new Vector2(10, 50+i*30), Color.Black);
            }
        }
    }
}
