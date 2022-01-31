namespace CS6502.UIConsole.ViewModels
{
    public class CS6502ViewModel : ViewModelBase
    {
        public CS6502ViewModel()
        {
            Console = 
                new ConsoleViewModel(
                    320,
                    256,
                    8,
                    8
                );
        }

        public ConsoleViewModel Console { get; }
    }
}
