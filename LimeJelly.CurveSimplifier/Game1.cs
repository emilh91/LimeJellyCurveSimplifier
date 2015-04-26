using System;
using System.Windows.Forms;
using LimeJelly.CurveSimplifier.State;
using SharpDX.Direct2D1;
using SharpDX.Direct3D;
using SharpDX.Direct3D11;
using SharpDX.DXGI;
using SharpDX.Windows;
using Device = SharpDX.Direct3D11.Device;
using Factory = SharpDX.Direct2D1.Factory;
using FeatureLevel = SharpDX.Direct2D1.FeatureLevel;

namespace LimeJelly.CurveSimplifier
{
    class Game1 : IDisposable
    {
        private readonly RenderForm _form;
        private readonly RenderTarget _renderTarget;
        private readonly Device _device;
        private readonly SwapChain _swapChain;
        private readonly ResourceFactory _resourceFactory;
        private ScreenState CurrentScreenState { get; set; }

        public Game1()
        {
            _form = new RenderForm
            {
                Size = new System.Drawing.Size(800, 600),
                Text = "LimeJelly Curve Simplifier",
            };
            _form.KeyDown += (o, e) =>
            {
                CurrentScreenState.KeyDown(e);
            };
            _form.KeyUp += (o, e) =>
            {
                CurrentScreenState.KeyUp(e);
            };
            _form.MouseDown += (o, e) =>
            {
                CurrentScreenState.MouseDown(e);
            };

            var swapChainDesc = new SwapChainDescription
            {
                BufferCount = 2,
                Usage = Usage.RenderTargetOutput,
                OutputHandle = _form.Handle,
                IsWindowed = true,
                ModeDescription = new ModeDescription(0, 0, new Rational(60, 1), Format.R8G8B8A8_UNorm),
                SampleDescription = new SampleDescription(1, 0),
                Flags = SwapChainFlags.AllowModeSwitch,
                SwapEffect = SwapEffect.Discard
            };
            Device.CreateWithSwapChain(DriverType.Hardware, DeviceCreationFlags.BgraSupport, swapChainDesc, out _device, out _swapChain);

            using (var factory = new Factory())
            {
                var dpi = factory.DesktopDpi;
                var backBuffer = Surface.FromSwapChain(_swapChain, 0);
                var rtp = new RenderTargetProperties
                {
                    DpiX = dpi.Width,
                    DpiY = dpi.Height,
                    MinLevel = FeatureLevel.Level_DEFAULT,
                    PixelFormat = new PixelFormat(Format.Unknown, AlphaMode.Ignore),
                    Type = RenderTargetType.Default,
                    Usage = RenderTargetUsage.None,
                };
                _renderTarget = new RenderTarget(factory, backBuffer, rtp);
            }

            _resourceFactory = new ResourceFactory(_renderTarget);

            CurrentScreenState = new MainMenuScreenState();
        }

        ~Game1()
        {
            Dispose(false);
        }

        public void Run()
        {
            RenderLoop.Run(_form, GameLoop);
        }

        private void GameLoop()
        {
            if (ShouldChangeState())
            {
                CurrentScreenState = CurrentScreenState.NextState;
            }

            if (CurrentScreenState == null)
            {
                Application.Exit();
            }
            else
            {
                CurrentScreenState.Update();

                _renderTarget.BeginDraw();
                CurrentScreenState.Draw(_renderTarget, _resourceFactory);
                _renderTarget.EndDraw();
                _swapChain.Present(1, PresentFlags.None);
            }
        }

        private bool ShouldChangeState()
        {
            return CurrentScreenState != null && CurrentScreenState.ShouldChangeState;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void Dispose(bool disposing)
        {
            if (disposing)
            {
                _resourceFactory.Dispose();
                _renderTarget.Dispose();
                _swapChain.Dispose();
                _device.Dispose();
            }
        }
    }
}
