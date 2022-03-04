using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;

namespace CS6502.UIConsole.Views
{
    public partial class CpuStateView : UserControl
    {
        public CpuStateView()
        {
            InitializeComponent();
        }

        private void InitializeComponent()
        {
            AvaloniaXamlLoader.Load(this);
        }
    }
}
