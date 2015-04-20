using SharpDX;
using SharpDX.Toolkit;
using SharpDX.Toolkit.Content;
using SharpDX.Toolkit.Graphics;
using SharpDX.Toolkit.Input;

namespace LimeJelly.CurveSimplifier.State
{
    class MainMenuScreenState : ScreenState
    {
        public MainMenuScreenState(IContentManager cm) : base(cm)
        {
        }

        public override void Update(GameTime gameTime, KeyboardState keyboard, MouseState mouse)
        {
            base.Update(gameTime, keyboard, mouse);
            if (IsPaused) return;

            if (keyboard.IsKeyPressed(Keys.A))
            {
                PushState(new AboutScreenState(ContentManager));
            }
            else if (keyboard.IsKeyPressed(Keys.D))
            {
                PushState(new CurveDrawerScreenState(ContentManager));
            }
        }

        public override void Draw(GameTime gameTime, SpriteBatch batch)
        {
            base.Draw(gameTime, batch);

            var arial16 = ContentManager.Load<SpriteFont>(@"Font\Arial16");
            batch.DrawString(arial16, "[D]raw curve", new Vector2(10, 50), Color.Black);
            batch.DrawString(arial16, "[A]bout", new Vector2(10, 80), Color.Black);
        }
    }
}
