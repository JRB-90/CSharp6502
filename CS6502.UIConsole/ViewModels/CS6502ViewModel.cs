using CS6502.UIConsole.Shared;

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

            cpuManager = new CpuManager();

            var charData = cpuManager.GetVRAMCharData();
            Console.SetCharData(charData);
        }

        public ConsoleViewModel Console { get; }

        private CpuManager cpuManager;
    }
}
