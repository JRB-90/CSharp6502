using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace CS6502.UIConsole.Views
{
    public partial class CS6502View : UserControl
    {
        public CS6502View()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
