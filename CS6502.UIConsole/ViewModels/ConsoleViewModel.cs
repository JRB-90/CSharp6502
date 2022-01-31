using Avalonia.Controls;
using Avalonia.Media;
using CS6502.UIConsole.Models;
using CS6502.UIConsole.Shared;

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

        private AssetManager assetManager;
        private PixelCanvasModel pixelCanvasModel;
    }
}
