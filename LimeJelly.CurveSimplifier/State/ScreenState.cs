using SharpDX;
using SharpDX.Toolkit;
using SharpDX.Toolkit.Graphics;
using SharpDX.Toolkit.Input;

namespace LimeJelly.CurveSimplifier.State
{
    class ScreenState
    {
        public Color ClearColor { get; protected set; }

        private ScreenState PreviousState { get; set; }
        public ScreenState NextState { get; private set; }
        public bool ShouldChangeState { get; private set; }

        public ScreenState()
        {
            ClearColor = Color.CornflowerBlue;
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
