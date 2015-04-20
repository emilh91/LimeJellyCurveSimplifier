using SharpDX;
using SharpDX.Toolkit;
using SharpDX.Toolkit.Content;
using SharpDX.Toolkit.Graphics;
using SharpDX.Toolkit.Input;

namespace LimeJelly.CurveSimplifier.State
{
    class ScreenState
    {
        public Color ClearColor { get; protected set; }

        protected IContentManager ContentManager { get; private set; }

        private ScreenState PreviousState { get; set; }
        public ScreenState NextState { get; private set; }
        public bool ShouldChangeState { get; private set; }

        public ScreenState(IContentManager cm)
        {
            ClearColor = Color.CornflowerBlue;
            ContentManager = cm;
        }

        public virtual void Update(GameTime gameTime, KeyboardState keyboard, MouseState mouse)
        {
        }

        public virtual void Draw(GameTime gameTime, SpriteBatch batch)
        {
        }

        public virtual void Draw(GameTime gameTime, PrimitiveBatch<VertexPositionColor> batch)
        {
        }

        protected void PushState(ScreenState state)
        {
            state.PreviousState = this;
            NextState = state;
            ShouldChangeState = true;
        }

        protected void PopState()
        {
            NextState = PreviousState;
            ShouldChangeState = true;

            if (PreviousState == null) return;
            PreviousState.NextState = null;
            PreviousState.ShouldChangeState = false;
        }
    }
}
