using Avalonia.Controls;
using CS6502.UIConsole.ViewModels;
using System.Threading.Tasks;

namespace CS6502.UIConsole.Shared
{
    public interface IDialogService
    {
        Task<string> ShowOpenFileDialogAsync(string title, FileDialogFilter filter);

        Task<string> ShowOpenFolderDialogAsync(string title);

        Task<string> ShowSaveFileDialogAsync(string title, FileDialogFilter filter);
    }
}
