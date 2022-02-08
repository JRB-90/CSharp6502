using CS6502.UIConsole.Models;
using CS6502.UIConsole.Shared;

namespace CS6502.UIConsole.ViewModels
{
    internal class CS6502ViewModel : ViewModelBase
    {
        const double UPDATE_INTERVAL = 50;

        readonly DialogService dialogService;
        readonly CS6502Model cpu;

        public CS6502ViewModel(DialogService dialogService)
        {
            this.dialogService = dialogService;
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

            CycleControl =
                new CycleControlViewModel(
                    dialogService,
                    cpu
                );
        }

        public ConsoleViewModel Console { get; }

        public CpuStateViewModel CpuState { get; }

        public CycleControlViewModel CycleControl { get; }
    }
}
