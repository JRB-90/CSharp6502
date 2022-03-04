using Avalonia;
using Avalonia.Controls.ApplicationLifetimes;
using Avalonia.Markup.Xaml;
using CS6502.UIConsole.Shared;
using CS6502.UIConsole.ViewModels;
using CS6502.UIConsole.Views;

namespace CS6502.UIConsole
{
    public class App : Application
    {
        public override void Initialize()
        {
            AvaloniaXamlLoader.Load(this);
        }

        public override void OnFrameworkInitializationCompleted()
        {
            if (ApplicationLifetime is IClassicDesktopStyleApplicationLifetime desktop)
            {
                desktop.MainWindow = new MainWindow();
                DialogService dialogService = new DialogService(desktop.MainWindow);
                desktop.MainWindow.DataContext = new MainWindowViewModel(dialogService);
            }

            base.OnFrameworkInitializationCompleted();
        }
    }
}
