using Avalonia;
using Avalonia.Logging;

namespace CS6502.UIConsole.ViewModels
{
    internal class MainWindowViewModel : ViewModelBase
    {
        public MainWindowViewModel()
        {
            AppBuilder.Configure<App>()
                .UsePlatformDetect()
                .LogToTrace(LogEventLevel.Information);

            CS6502 = new CS6502ViewModel();
        }

        public CS6502ViewModel CS6502 { get; }
    }
}
