using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace CS6502.UIConsole.Views
{
    public partial class MemoryView : UserControl
    {
        public MemoryView()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
