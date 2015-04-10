using System.Collections.Generic;
using System.Linq;
using SharpDX;
using SharpDX.Toolkit;
using SharpDX.Toolkit.Graphics;
using SharpDX.Toolkit.Input;

namespace LimeJelly.Driver
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    class Game1 : Game
    {
        private const int Width = 800;
        private const int Height = 600;

        // Boilerplate properties
        private KeyboardManager KeyboardManager { get; set; }
        private MouseManager MouseManager { get; set; }
        private PrimitiveBatch<VertexPositionColor> PrimitiveBatch { get; set; }
        private Effect Effect { get; set; }

        // Actually useful properties
        private KeyboardState Keyboard { get { return KeyboardManager.GetState(); } }
        private MouseState Mouse { get { return MouseManager.GetState(); } }
        private List<VertexPositionColor> Vertices { get; set; }

        public Game1()
        {
            new GraphicsDeviceManager(this);
            Vertices = new List<VertexPositionColor>();
            KeyboardManager = new KeyboardManager(this);
            MouseManager = new MouseManager(this);
            IsMouseVisible = true;
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

            PrimitiveBatch = new PrimitiveBatch<VertexPositionColor>(GraphicsDevice);

            Effect = new BasicEffect(GraphicsDevice)
            {
                VertexColorEnabled = true,
                Projection = Matrix.OrthoOffCenterRH(0, Width, Height, 0, 0, 1)
            };

            base.Initialize();
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            if (Keyboard.IsKeyDown(Keys.Escape))
            {
                Exit();
            }

            if (Keyboard.IsKeyPressed(Keys.R))
            {
                Vertices.Clear();
            }

            // For undoing
            if (Keyboard.IsKeyPressed(Keys.Z))
            {
                if (Vertices.Any())
                {
                    Vertices.RemoveAt(Vertices.Count - 1);
                }
            }

            if (Mouse.LeftButton.Down)
            {
                var vec3 = new Vector3(Mouse.X * Width, Mouse.Y * Height, 0);
                var vpc = new VertexPositionColor(vec3, Color.Black);
                if (Vertices.Count == 0 || Vertices.Last() != vpc)
                {
                    Vertices.Add(vpc);
                }
            }

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            foreach (var pass in Effect.CurrentTechnique.Passes)
            {
                pass.Apply();
            }

            PrimitiveBatch.Begin();
            if (Vertices.Count == 1)
            {
                PrimitiveBatch.Draw(PrimitiveType.PointList, Vertices.ToArray());
            }
            else if (Vertices.Count > 1)
            {
                PrimitiveBatch.Draw(PrimitiveType.LineStrip, Vertices.ToArray());
            }
            PrimitiveBatch.End();

            base.Draw(gameTime);
        }
    }
}
