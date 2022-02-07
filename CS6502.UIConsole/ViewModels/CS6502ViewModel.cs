using CS6502.UIConsole.Models;

namespace CS6502.UIConsole.ViewModels
{
    internal class CS6502ViewModel : ViewModelBase
    {
        public CS6502ViewModel()
        {
            cpu = new CpuModel();

            Console = 
                new ConsoleViewModel(
                    cpu,
                    320,
                    256,
                    8,
                    8
                );

            CpuState = new CpuStateViewModel(cpu);
            CycleControl = new CycleControlViewModel(cpu);
        }

        public ConsoleViewModel Console { get; }

        public CpuStateViewModel CpuState { get; }

        public CycleControlViewModel CycleControl { get; }

        private CpuModel cpu;
    }
}
