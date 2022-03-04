using Avalonia;
using Avalonia.Logging;
using CS6502.UIConsole.Shared;
using CS6502.UIConsole.Windows;

namespace CS6502.UIConsole.ViewModels
{
    internal class MainWindowViewModel : ViewModelBase
    {
        readonly DialogService dialogService;

        public MainWindowViewModel(DialogService dialogService)
        {
            this.dialogService = dialogService;

            AppBuilder.Configure<App>()
                .UsePlatformDetect()
                .LogToTrace(LogEventLevel.Information);

            CS6502 = new CS6502ViewModel(dialogService);

            memoryView = new MemoryViewWindow();
            memoryView.DataContext = CS6502.MemoryView;
            memoryView.Show();
        }

        public CS6502ViewModel CS6502 { get; }

        private MemoryViewWindow memoryView;
    }
}
