using Avalonia.Controls;
using Avalonia.Media;
using CS6502.UIConsole.Models;
using CS6502.UIConsole.Shared;
using System.Timers;

namespace CS6502.UIConsole.ViewModels
{
    public class ConsoleViewModel : ViewModelBase
    {
        public ConsoleViewModel(
            int width,
            int height,
            int charWidth,
            int charHeight)
        {
            Width = width;
            Height = height;
            CharsPerRow = width / charWidth;
            CharsPerCol = height / charHeight;
            AspectRatio = (double)width / (double)height;

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

            charData = new byte[CharsPerRow * CharsPerCol];

            timer = new Timer(100);
            timer.AutoReset = true;
            timer.Elapsed += Timer_Elapsed;
            timer.Start();
        }

        public int Width { get; }

        public int Height { get; }

        public int CharsPerRow { get; }

        public int CharsPerCol { get; }

        public double AspectRatio { get; }

        public Image ImageControl { get; }

        public void SetCharData(byte[] charData)
        {
            lock (renderLock)
            {
                this.charData = charData;
            }
        }

        private void Timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            UpdateChars();
        }

        private void UpdateChars()
        {
            lock (renderLock)
            {
                pixelCanvasModel.DrawCharData(charData);
                ImageControl.InvalidateVisual();
            }
        }

        private Timer timer;
        private AssetManager assetManager;
        private PixelCanvasModel pixelCanvasModel;
        private byte[] charData;

        private static object renderLock = new object();
    }
}
