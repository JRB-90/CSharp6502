using CS6502.UIConsole.Models;

namespace CS6502.UIConsole.ViewModels
{
    internal class CS6502ViewModel : ViewModelBase
    {
        const double UPDATE_INTERVAL = 50;

        readonly CS6502Model cpu;

        public CS6502ViewModel()
        {
            cpu = new CS6502Model();

            Console = 
                new ConsoleViewModel(
                    cpu,
                    UPDATE_INTERVAL,
                    320,
                    256,
                    8,
                    8
                );

            CpuState =
                new CpuStateViewModel(
                    cpu, 
                    UPDATE_INTERVAL
                );

            CycleControl = new CycleControlViewModel(cpu);
        }

        public ConsoleViewModel Console { get; }

        public CpuStateViewModel CpuState { get; }

        public CycleControlViewModel CycleControl { get; }
    }
}
