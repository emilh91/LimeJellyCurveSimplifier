using System.Windows.Forms;
using SharpDX.Direct2D1;

namespace LimeJelly.CurveSimplifier.State
{
    abstract class ScreenState
    {
        protected bool IsPaused { get; private set; }
        protected int FrameCount { get; private set; }

        private ScreenState PreviousState { get; set; }
        public ScreenState NextState { get; private set; }
        public bool ShouldChangeState { get; private set; }

        public virtual void KeyDown(KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Escape)
            {
                PopState();
            }
            else if (e.KeyCode == Keys.F2)
            {
                Reset();
            }
            else if (e.KeyCode == Keys.Pause)
            {
                if (IsPaused)
                {
                    Resume();
                }
                else
                {
                    Pause();
                }
            }
        }

        public virtual void KeyUp(KeyEventArgs e)
        {
        }

        public virtual void KeyPress(KeyPressEventArgs e)
        {
        }

        public virtual void MouseDown(MouseEventArgs e)
        {
        }

        public virtual void MouseUp(MouseEventArgs e)
        {
        }

        public virtual void MouseClick(MouseEventArgs e)
        {
        }

        public virtual void MouseDoubleClick(MouseEventArgs e)
        {
        }

        public virtual void Update()
        {
            if (!IsPaused)
            {
                FrameCount++;
            }
        }

        public abstract void Draw(RenderTarget renderTarget, ResourceFactory rf);

        protected virtual void Reset()
        {
            IsPaused = false;
            FrameCount = 0;
        }

        protected virtual void Pause()
        {
            IsPaused = true;
        }

        protected virtual void Resume()
        {
            IsPaused = false;
        }

        protected void PushState(ScreenState state)
        {
            state.PreviousState = this;
            NextState = state;
            ShouldChangeState = true;
        }

        private void PopState()
        {
            NextState = PreviousState;
            ShouldChangeState = true;

            if (PreviousState == null) return;
            PreviousState.NextState = null;
            PreviousState.ShouldChangeState = false;
        }
    }
}
