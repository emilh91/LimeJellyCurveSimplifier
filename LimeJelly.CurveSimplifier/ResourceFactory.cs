using System;
using System.Collections.Generic;
using SharpDX;
using SharpDX.Direct2D1;
using SharpDX.DirectWrite;
using FontFactory = SharpDX.DirectWrite.Factory;

namespace LimeJelly.CurveSimplifier
{
    class ResourceFactory : IDisposable
    {
        private readonly RenderTarget _renderTarget;
        private readonly FontFactory _fontFactory;
        private readonly Dictionary<string, TextFormat> _textFormats;
        private readonly Dictionary<Color, SolidColorBrush> _brushes;

        public ResourceFactory(RenderTarget renderTarget)
        {
            _renderTarget = renderTarget;
            _fontFactory = new FontFactory();
            _textFormats = new Dictionary<string, TextFormat>();
            _brushes = new Dictionary<Color, SolidColorBrush>();
        }

        ~ResourceFactory()
        {
            Dispose(false);
        }

        public TextFormat GetFont(string familyName, float size, FontWeight weight = FontWeight.Normal, FontStyle style = FontStyle.Normal)
        {
            var key = string.Format("{0}-{1}-{2}-{3}", familyName, size, weight, style);
            TextFormat font;
            if (!_textFormats.TryGetValue(key, out font))
            {
                font = new TextFormat(_fontFactory, familyName, weight, style, size);
                _textFormats.Add(key, font);
            }
            font.TextAlignment = TextAlignment.Leading;
            return font;
        }

        public SolidColorBrush GetSolidColorBrush(Color color)
        {
            SolidColorBrush brush;
            if (!_brushes.TryGetValue(color, out brush))
            {
                brush = new SolidColorBrush(_renderTarget, color);
                _brushes.Add(color, brush);
            }
            return brush;
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
                foreach (var kvp in _textFormats)
                {
                    kvp.Value.Dispose();
                }
                _fontFactory.Dispose();

                foreach (var kvp in _brushes)
                {
                    kvp.Value.Dispose();
                }
            }
        }
    }
}
