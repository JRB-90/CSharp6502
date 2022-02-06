using CS6502.Core;
using ReactiveUI;
using System;

namespace CS6502.UIConsole.ViewModels
{
    internal class CpuStateViewModel : ViewModelBase
    {
        public CpuStateViewModel(
            CycleState state,
            Action startAction,
            Action stopAction,
            Action resetAction)
        {
            this.state = state;
            this.startAction = startAction;
            this.stopAction = stopAction;
            this.resetAction = resetAction;
        }

        public CycleState State
        {
            get => state;
            set => this.RaiseAndSetIfChanged(ref state, value, "");
        }

        public string AHex => state.A.ToHexString();

        public string XHex => state.X.ToHexString();

        public string YHex => state.Y.ToHexString();

        public string IRHex => state.IR.ToHexString();

        public string SPHex => state.SP.ToHexString();

        public string AddrHex => state.Address.ToHexString();

        public string PCHex => state.PC.ToHexString();

        public void Start()
        {
            startAction.Invoke();
        }

        public void Stop()
        {
            stopAction.Invoke();
        }

        public void Reset()
        {
            resetAction.Invoke();
        }

        private CycleState state;
        private Action startAction;
        private Action stopAction;
        private Action resetAction;
    }
}
