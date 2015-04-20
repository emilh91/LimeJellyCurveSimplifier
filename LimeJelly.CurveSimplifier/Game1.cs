using LimeJelly.CurveSimplifier.State;
using SharpDX;
using SharpDX.Toolkit;
using SharpDX.Toolkit.Graphics;
using SharpDX.Toolkit.Input;

namespace LimeJelly.CurveSimplifier
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    class Game1 : Game
    {
        private const int Width = 800;
        private const int Height = 600;

        private GraphicsDeviceManager GraphicsDeviceManager { get; set; }
        private KeyboardManager KeyboardManager { get; set; }
        private MouseManager MouseManager { get; set; }
        private SpriteBatch SpriteBatch { get; set; }
        private PrimitiveBatch<VertexPositionColor> PrimitiveBatch { get; set; }
        private Effect Effect { get; set; }
        private ScreenState CurrentScreenState { get; set; }

        public Game1()
        {
            GraphicsDeviceManager = new GraphicsDeviceManager(this)
            {
                PreferredBackBufferWidth = Width,
                PreferredBackBufferHeight = Height
            };
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            Window.Title = "LimeJelly Curve Simplifier";
            IsMouseVisible = true;

            KeyboardManager = new KeyboardManager(this);
            MouseManager = new MouseManager(this);

            SpriteBatch = new SpriteBatch(GraphicsDevice);
            PrimitiveBatch = new PrimitiveBatch<VertexPositionColor>(GraphicsDevice);

            Effect = new BasicEffect(GraphicsDevice)
            {
                VertexColorEnabled = true,
                Projection = Matrix.OrthoOffCenterRH(0, Width, Height, 0, 0, 1)
            };

            CurrentScreenState = new MainMenuScreenState(Content);

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            Content.Load<SpriteFont>(@"Font\Arial16");
            base.LoadContent();
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            if (ShouldChangeState())
            {
                CurrentScreenState = CurrentScreenState.NextState;
            }

            if (CurrentScreenState == null)
            {
                Exit();
            }
            else
            {
                var keyboard = KeyboardManager.GetState();
                var mouse = MouseManager.GetState();
                CurrentScreenState.Update(gameTime, keyboard, mouse);
            }

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(CurrentScreenState.ClearColor);

            foreach (var pass in Effect.CurrentTechnique.Passes)
            {
                pass.Apply();
            }

            SpriteBatch.Begin();
            PrimitiveBatch.Begin();
            
            CurrentScreenState.Draw(gameTime, SpriteBatch);
            CurrentScreenState.Draw(gameTime, PrimitiveBatch);

            PrimitiveBatch.End();
            SpriteBatch.End();

            base.Draw(gameTime);
        }

        private bool ShouldChangeState()
        {
            return CurrentScreenState != null && CurrentScreenState.ShouldChangeState;
        }
    }
}
