using Avalonia.Controls;
using System.Threading.Tasks;

namespace CS6502.UIConsole.Shared
{
    public class DialogService : IDialogService
    {
        readonly Window window;

        public DialogService(Window window)
        {
            this.window = window;
        }

        public async Task<string> ShowOpenFileDialogAsync( 
            string title, 
            FileDialogFilter filter)
        {
            var openFileDialog = new OpenFileDialog()
            {
                Title = title,
                AllowMultiple = false
            };
            openFileDialog.Filters.Add(filter);
            var result = await openFileDialog.ShowAsync(window);
            return result[0];
        }

        public async Task<string> ShowOpenFolderDialogAsync(
            string title)
        {
            var openFolderDialog = new OpenFolderDialog()
            {
                Title = title
            };
            return await openFolderDialog.ShowAsync(window);
        }

        public async Task<string> ShowSaveFileDialogAsync(
            string title,
            FileDialogFilter filter)
        {
            var saveFileDialog = new SaveFileDialog()
            {
                Title = title
            };
            saveFileDialog.Filters.Add(filter);
            return await saveFileDialog.ShowAsync(window);
        }
    }
}
