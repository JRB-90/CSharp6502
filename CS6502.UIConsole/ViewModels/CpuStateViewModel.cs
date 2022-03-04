using CS6502.Core;
using CS6502.UIConsole.Models;
using ReactiveUI;
using System;
using System.Reactive.Linq;

namespace CS6502.UIConsole.ViewModels
{
    internal class CpuStateViewModel : ViewModelBase
    {
        readonly CS6502Model cpu;

        public CpuStateViewModel(
            CS6502Model cpu,
            double updateInterval)
        {
            this.cpu = cpu;
            currentState = new CycleState(0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0);
            cpu.CycleState
                .Sample(TimeSpan.FromMilliseconds(updateInterval))
                .Subscribe(state => UpdateUI(state));
        }

        public string CycleCount => currentState.CycleID.ToString();

        public string AHex => currentState.A.ToHexString();

        public string XHex => currentState.X.ToHexString();

        public string YHex => currentState.Y.ToHexString();

        public string IRHex => currentState.IR.ToHexString();

        public string SPHex => currentState.SP.ToHexString();

        public string AddrHex => currentState.Address.ToHexString();

        public string PCHex => currentState.PC.ToHexString();

        public string DataHex => currentState.Data.ToHexString();

        private void UpdateUI(CycleState currentState)
        {
            this.currentState = currentState;
            this.RaisePropertyChanged("");
        }

        private CycleState currentState;
    }
}
