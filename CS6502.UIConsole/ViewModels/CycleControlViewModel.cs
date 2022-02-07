using CS6502.UIConsole.Models;
using ReactiveUI;

namespace CS6502.UIConsole.ViewModels
{
    internal class CycleControlViewModel : ViewModelBase
    {
        readonly CpuModel cpu;

        public CycleControlViewModel(CpuModel cpu)
        {
            this.cpu = cpu;
            cpu.RunStateChanged += Cpu_RunStateChanged;
        }

        public string RunStatus =>
            cpu.IsRunning
                ? "Running"
                : "Halted";

        public void Start()
        {
            cpu.Start();
        }

        public void Stop()
        {
            cpu.Stop();
        }

        public void Reset()
        {
            cpu.Reset();
        }

        private void Cpu_RunStateChanged(object? sender, System.EventArgs e)
        {
            this.RaisePropertyChanged(nameof(RunStatus));
        }
    }
}
