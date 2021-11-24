using System;

namespace CS6502.Core
{
    /// <summary>
    /// Abstract base class to share funcitonality amongst all CPU implementations.
    /// </summary>
    public abstract class CpuBase : ICpu
    {
        const ushort IRQ_VECTOR_HI = 0xFFFF;
        const ushort IRQ_VECTOR_LO = 0xFFFE;
        const ushort RST_VECTOR_HI = 0xFFFD;
        const ushort RST_VECTOR_LO = 0xFFFC;
        const ushort NMI_VECTOR_HI = 0xFFFB;
        const ushort NMI_VECTOR_LO = 0xFFFA;
        
        const int STARTUP_CYCLES = 12;

        public CpuBase(string name)
        {
            Name = name;
            state = CpuState.Init;

            IRQ_N = new Wire(WirePull.PullUp);
            NMI_N = new Wire(WirePull.PullUp);
            RES_N = new Wire(WirePull.PullUp);
            RDY_N = new Wire(WirePull.PullUp);

            RW_N = new Wire(WirePull.PullUp);
            rw_n = new Pin();
            RW_N.ConnectPin(rw_n);

            SYNC_N = new Wire(WirePull.PullUp);
            sync_n = new Pin();
            SYNC_N.ConnectPin(sync_n);

            PHI1O = new Wire(WirePull.PullDown);
            phi1o = new Pin();
            PHI1O.ConnectPin(phi1o);

            PHI2O = new Wire(WirePull.PullDown);
            phi2o = new Pin();
            PHI2O.ConnectPin(phi2o);

            PHI2 = new Wire(WirePull.PullDown);
            SetPhiOutput();

            addressBus = new Bus(16);
            addressPins = addressBus.CreateAndConnectPinArray();
            AddressBus = addressBus;

            dataBus = new Bus(8);
            dataPins = dataBus.CreateAndConnectPinArray();
            DataBus = dataBus;

            TransitionState(CpuState.Startup);
        }

        public string Name { get; }

        public Wire IRQ_N
        {
            get => irq_n;
            set
            {
                irq_n = value;
                irq_n.StateChanged += Irq_n_StateChanged;
            }
        }

        public Wire NMI_N
        {
            get => nmi_n;
            set
            {
                nmi_n = value;
                nmi_n.StateChanged += Nmi_n_StateChanged;
            }
        }

        public Wire RES_N
        {
            get => res_n;
            set
            {
                res_n = value;
                res_n.StateChanged += Res_n_StateChanged;
            }
        }

        public Wire RDY_N
        {
            get => rdy_n;
            set
            {
                rdy_n = value;
                rdy_n.StateChanged += Rdy_n_StateChanged;
            }
        }

        public Wire RW_N { get; }

        public Wire SYNC_N { get; }

        public Wire PHI2
        {
            get => phi2;
            set
            {
                phi2 = value;
                SetPhiOutput();
                phi2.StateChanged += Phi2_StateChanged;
            }
        }

        public Wire PHI1O { get; }

        public Wire PHI2O { get; }

        public Bus AddressBus
        {
            get => addressBus;
            set
            {
                addressBus.DisconnectPins(addressPins);
                addressBus = value;
                addressBus.ConnectPins(addressPins);
            }
        }

        public Bus DataBus
        {
            get => dataBus;
            set
            {
                dataBus.DisconnectPins(dataPins);
                dataBus = value;
                dataBus.ConnectPins(dataPins);
            }
        }

        public override string ToString()
        {
            return Name;
        }

        public string GetCurrentStateString(char delimiter)
        {
            return
                $"{RW_N.State.ToNumStr()}{delimiter}" +
                $"{registers.A.ToHexString()}{delimiter}" +
                $"{registers.X.ToHexString()}{delimiter}" +
                $"{registers.Y.ToHexString()}{delimiter}" +
                $"{registers.IR.Opcode.ToHexString()}{delimiter}" +
                $"{registers.P.Value.ToHexString()}{delimiter}" +
                $"{registers.SP.ToHexString()}{delimiter}" +
                $"{registers.PC.ToHexString()}{delimiter}" +
                $"{AddressBus.ToUshort().ToHexString()}{delimiter}" +
                $"{registers.DataBusBuffer.ToHexString()}";
        }

        public CycleState GetCurrentCycleState(int cycleID)
        {
            return
                new CycleState(
                    cycleID,
                    Convert.ToByte(RW_N.State),
                    registers.A,
                    registers.X,
                    registers.Y,
                    registers.IR.Opcode,
                    registers.P.Value,
                    registers.SP,
                    registers.PC,
                    AddressBus.ToUshort(),
                    registers.DataBusBuffer
                );
        }

        #region Helper Functions

        protected RWState GetRW()
        {
            return RW_N.State ? RWState.Read : RWState.Write;
        }

        protected void SetRW(RWState rwState)
        {
            if (rwState == RWState.Read)
            {
                dataPins.SetAllTo(TriState.HighImpedance);
                rw_n.State = TriState.True;
            }
            else if (rwState == RWState.Write)
            {
                rw_n.State = TriState.False;
            }
        }

        protected EnableState GetSync()
        {
            return SYNC_N.State ? EnableState.Disabled : EnableState.Enabled;
        }

        protected void SetSync(EnableState enableState)
        {
            if (enableState == EnableState.Enabled)
            {
                sync_n.State = TriState.False;
            }
            else if (enableState == EnableState.Disabled)
            {
                sync_n.State = TriState.True;
            }
        }

        protected void SetAddressBus(ushort address)
        {
            addressPins.SetTo(address);
        }

        protected void TransferPCToAddressBus()
        {
            addressPins.SetTo(registers.PC);
        }

        protected byte ReadFromDataBus()
        {
            SetRW(RWState.Read);

            return dataBus.ToByte();
        }

        protected void WriteToDataBus(byte value)
        {
            dataPins.SetTo(value);
            SetRW(RWState.Write);
        }

        protected void ReadAddressBufferLo()
        {
            addressBuffer &= 0xFF00;
            addressBuffer |= registers.DataBusBuffer;
        }

        protected void ReadAddressBufferHi()
        {
            addressBuffer &= 0x00FF;
            addressBuffer |= (ushort)(registers.DataBusBuffer << 8);
        }

        #endregion

        private void Irq_n_StateChanged(object sender, WireStateChangedEventArgs e)
        {
            // TODO - Handle IRQ
        }

        private void Nmi_n_StateChanged(object sender, WireStateChangedEventArgs e)
        {
            // TODO - Handle NMI
        }

        private void Res_n_StateChanged(object sender, WireStateChangedEventArgs e)
        {
            // TODO - Handle RESET
        }

        private void Rdy_n_StateChanged(object sender, WireStateChangedEventArgs e)
        {
            // TODO - Handle RDY
        }

        private void Phi2_StateChanged(object sender, WireStateChangedEventArgs e)
        {
            SetPhiOutput();

            if (e.NewState == true)
            {
                Cycle(SignalEdge.RisingEdge);
            }
            else if (e.NewState == false)
            {
                Cycle(SignalEdge.FallingEdge);
            }
        }

        private void SetPhiOutput()
        {
            phi1o.State = PHI2.State ? TriState.False : TriState.True;
            phi2o.State = PHI2.State ? TriState.True : TriState.False;
        }

        #region Cycle Handling

        private void Cycle(SignalEdge signalEdge)
        {
            switch (state)
            {
                case CpuState.ResetActive:
                    HandleResetActive();
                    break;
                case CpuState.IrqActive:
                    HandleIrqActive();
                    break;
                case CpuState.NmiActive:
                    HandleNmiActive();
                    break;
                case CpuState.BrkActive:
                    HandlerBrkActive();
                    break;
                case CpuState.Startup:
                    HandleStartupCycle(signalEdge);
                    break;
                case CpuState.ReadingOpcode:
                    HandleReadingOpcodeCycle(signalEdge);
                    break;
                case CpuState.HandlingAddressingMode:
                    HandleAddressingModeCycle(signalEdge);
                    break;
                case CpuState.ExecutingInstruction:
                    HandleExecutingInstructionCycle(signalEdge);
                    break;
                default:
                    throw new InvalidOperationException($"CPU State {state} not supported");
            }
        }

        private void HandleResetActive()
        {
            // TODO
        }

        private void HandleIrqActive()
        {
            // TODO
        }

        private void HandleNmiActive()
        {
            // TODO
        }

        private void HandlerBrkActive()
        {
            // TODO
        }

        private void HandleStartupCycle(SignalEdge signalEdge)
        {
            if (startupCycleCount == 6)
            {
                // Simulate the push off of CPU params from stack
                SetAddressBus(0x01C0);
            }
            else if (startupCycleCount == 8)
            {
                // Simulate the push off of CPU params from stack
                SetAddressBus(0x01BF);
            }
            else if (startupCycleCount == 10)
            {
                // Simulate the push off of CPU params from stack
                SetAddressBus(0x01BE);
            }

            if (startupCycleCount >= STARTUP_CYCLES)
            {
                if (startupCycleCount == 12)
                {
                    // Simulate the push off of CPU params from stack
                    registers.DecrementStackPointer();
                    registers.DecrementStackPointer();
                    registers.DecrementStackPointer();

                    SetAddressBus(RST_VECTOR_LO);
                    SetRW(RWState.Read);
                    addressBuffer = 0;
                    addressReadingState = AddressReadingState.ReadingLoByte;
                }
                else if (startupCycleCount == 13)
                {
                    registers.SetIRQ();
                    registers.LatchDataBus(ReadFromDataBus());
                    ReadAddressBufferLo();
                }
                else if (startupCycleCount == 14)
                {
                    registers.SetBRK();
                    SetAddressBus(RST_VECTOR_HI);
                    SetRW(RWState.Read);
                    addressReadingState = AddressReadingState.ReadingLoByte;
                }
                else if (startupCycleCount == 15)
                {
                    registers.LatchDataBus(ReadFromDataBus());
                    ReadAddressBufferHi();
                    TransitionState(CpuState.ReadingOpcode);
                }
            }
            startupCycleCount++;
        }

        private void HandleReadingOpcodeCycle(SignalEdge signalEdge)
        {
            if (signalEdge == SignalEdge.FallingEdge)
            {
                // We have transisiton to next opcode so execute the last instruction
                // as this happens on the falling edge..
                registers.IR.Execute(registers);

                instructionCycleCount = 0;
                registers.SetProgramCounter(addressBuffer);
                TransferPCToAddressBus();
            }
            else if (signalEdge == SignalEdge.RisingEdge)
            {
                registers.LatchDataBus(ReadFromDataBus());
                TransitionState(CpuState.HandlingAddressingMode);
                instructionCycleCount++;
            }
        }

        private void HandleAddressingModeCycle(SignalEdge signalEdge)
        {
            if (signalEdge == SignalEdge.FallingEdge)
            {
                if (instructionCycleCount == 1)
                {
                    registers.DecodeOpcode();
                }

                switch (registers.IR.AddressingMode)
                {
                    case AddressingMode.Implied:
                        if (instructionCycleCount == 1)
                        {
                            registers.IncrementProgramCounter();
                            TransferPCToAddressBus();
                        }
                        break;

                    case AddressingMode.Absolute:
                        if (instructionCycleCount == 1)
                        {
                            registers.IncrementProgramCounter();
                            TransferPCToAddressBus();
                        }
                        else if (instructionCycleCount == 2)
                        {
                            registers.IncrementProgramCounter();
                            TransferPCToAddressBus();
                        }
                        break;

                    default:
                        throw new NotImplementedException("Addressign mode not supported");
                }
            }
            else if (signalEdge == SignalEdge.RisingEdge)
            {
                registers.LatchDataBus(ReadFromDataBus());

                switch (registers.IR.AddressingMode)
                {
                    case AddressingMode.Implied:
                        if (instructionCycleCount == 1)
                        {
                            addressBuffer = registers.PC;
                            TransitionState(CpuState.ReadingOpcode);
                        }
                        break;

                    case AddressingMode.Absolute:
                        if (instructionCycleCount == 1)
                        {
                            ReadAddressBufferLo();
                        }
                        else if (instructionCycleCount == 2)
                        {
                            ReadAddressBufferHi();
                            TransitionState(CpuState.ReadingOpcode); 
                        }
                        break;

                    default:
                        throw new NotImplementedException($"Addressing mode {registers.IR.AddressingMode.ToString()} not supported");
                }

                instructionCycleCount++;
            }
        }

        private void HandleExecutingInstructionCycle(SignalEdge signalEdge)
        {
            if (signalEdge == SignalEdge.FallingEdge)
            {
                // TODO
                registers.IncrementProgramCounter();
                TransferPCToAddressBus();
            }
            else if (signalEdge == SignalEdge.RisingEdge)
            {
                // TODO
                registers.LatchDataBus(ReadFromDataBus());
            }

            instructionCycleCount++;
        }

        #endregion

        #region State Transitions

        private void TransitionState(CpuState newState)
        {
            if (state == CpuState.Init && newState == CpuState.Startup)
            {
                SetCpuToStartupState();
                state = newState;
            }
            else if (state == CpuState.Startup && newState == CpuState.ReadingOpcode)
            {
                ExitStartup();
                state = newState;
            }
            else if (state == CpuState.ReadingOpcode && newState == CpuState.HandlingAddressingMode)
            {
                ExitReadingOpcode();
                state = newState;
            }
            else if (state == CpuState.HandlingAddressingMode && newState == CpuState.ReadingOpcode)
            {
                ExitExecutingInstruction();
                state = newState;
            }
            else if (state == CpuState.ExecutingInstruction && newState == CpuState.ReadingOpcode)
            {
                ExitExecutingInstruction();
                state = newState;
            }
            else
            {
                // This exception flags any non-defined transitions from occuring
                throw new InvalidOperationException($"Cannot transition states from {state.ToString()} to {newState.ToString()}");
            }
        }

        private void SetCpuToStartupState()
        {
            // TODO

            registers = new CpuRegisters();
            startupCycleCount = 0;
            SetRW(RWState.Read);
            SetAddressBus(0x00FF);
        }

        private void ExitStartup()
        {
            // TODO
            startupCycleCount = 0;
        }

        private void ExitReadingOpcode()
        {
            // TODO
        }

        private void ExitExecutingInstruction()
        {
            // TODO
        }

        #endregion

        private CpuState state;
        private int startupCycleCount;
        private int instructionCycleCount;
        private CpuRegisters registers;
        private AddressReadingState addressReadingState;
        private ushort addressBuffer;

        private Wire irq_n;
        private Wire nmi_n;
        private Wire res_n;
        private Wire rdy_n;
        private Wire phi2;
        private Pin rw_n;
        private Pin sync_n;
        private Pin phi1o;
        private Pin phi2o;
        private Bus addressBus;
        private Bus dataBus;
        private Pin[] addressPins;
        private Pin[] dataPins;
    }
}
