using Avalonia.Controls;
using Avalonia.Media;
using CS6502.UIConsole.Models;
using CS6502.UIConsole.Shared;
using System;
using System.Reactive.Linq;

namespace CS6502.UIConsole.ViewModels
{
    internal class ConsoleViewModel : ViewModelBase
    {
        readonly CS6502Model cpu;

        public ConsoleViewModel(
            CS6502Model cpu,
            double updateInterval,
            int width,
            int height,
            int charWidth,
            int charHeight)
        {
            this.cpu = cpu;
            Width = width;
            Height = height;
            CharsPerRow = width / charWidth;
            CharsPerCol = height / charHeight;
            AspectRatio = (double)width / (double)height;

            cpu.ConsoleChars
                .Sample(TimeSpan.FromMilliseconds(updateInterval))
                .Subscribe(charData => RenderChars(charData));

            assetManager = 
                new AssetManager(
                    "dogica.png",
                    charWidth,
                    charHeight
                );

            pixelCanvasModel =
                new PixelCanvasModel(
                    width,
                    height,
                    charWidth,
                    charHeight,
                    assetManager.Font
                );

            ImageControl =
                new Image()
                {
                    Source = pixelCanvasModel.Bitmap,
                    Stretch = Stretch.Uniform,
                };
        }

        public int Width { get; }

        public int Height { get; }

        public int CharsPerRow { get; }

        public int CharsPerCol { get; }

        public double AspectRatio { get; }

        public Image ImageControl { get; }

        private void RenderChars(byte[] charData)
        {
            pixelCanvasModel.DrawCharData(charData);
            ImageControl.InvalidateVisual();
        }

        private AssetManager assetManager;
        private PixelCanvasModel pixelCanvasModel;

        private static object renderLock = new object();
    }
}
