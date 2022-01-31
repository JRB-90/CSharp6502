using CS6502.UIConsole.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
        }

        public int Width { get; }

        public int Height { get; }

        public int CharsPerRow { get; }

        public int CharsPerCol { get; }

        public double AspectRatio { get; }

        private AssetManager assetManager;
    }
}
