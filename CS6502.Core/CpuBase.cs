using System;

namespace CS6502.Core
{
    /// <summary>
    /// Abstract base class to share funcitonality amongst all CPU implementations.
    /// </summary>
    public abstract class CpuBase : ICpu
    {
        #region Constants

        const ushort IRQ_VECTOR_HI = 0xFFFF;
        const ushort IRQ_VECTOR_LO = 0xFFFE;
        const ushort RST_VECTOR_HI = 0xFFFD;
        const ushort RST_VECTOR_LO = 0xFFFC;
        const ushort NMI_VECTOR_HI = 0xFFFB;
        const ushort NMI_VECTOR_LO = 0xFFFA;
        
        const int STARTUP_CYCLES = 12;

        #endregion

        #region Constructors

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
            RecalculatePhiOutputs();

            addressBus = new Bus(16);
            addressPins = addressBus.CreateAndConnectPinArray();
            AddressBus = addressBus;

            dataBus = new Bus(8);
            dataPins = dataBus.CreateAndConnectPinArray();
            DataBus = dataBus;

            SetCpuToStartupState();
            TransitionState(CpuState.Startup);
        }

        #endregion

        #region Properties

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
                RecalculatePhiOutputs();
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

        #endregion

        #region Helper Functions

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
                dataPins.SetTo(registers.DataBusBuffer);
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

        protected void ClearAddressBuffer()
        {
            addressBuffer = 0;
        }

        private void RecalculatePhiOutputs()
        {
            phi1o.State = PHI2.State ? TriState.False : TriState.True;
            phi2o.State = PHI2.State ? TriState.True : TriState.False;
        }

        private void TransitionState(CpuState newState)
        {
            state = newState;
        }

        private void SetCpuToStartupState()
        {
            registers = new CpuRegisters();
            startupCycleCount = 0;
            SetRW(RWState.Read);
            SetAddressBus(0x00FF);
        }

        #endregion

        #region EventHandlers

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
            RecalculatePhiOutputs();

            if (e.NewState == true)
            {
                Cycle(SignalEdge.RisingEdge);
            }
            else if (e.NewState == false)
            {
                Cycle(SignalEdge.FallingEdge);
            }
        }

        #endregion

        #region Cycle Handling

        private void Cycle(SignalEdge signalEdge)
        {
            switch (state)
            {
                case CpuState.Startup:
                    HandleStartupCycle(signalEdge);
                    break;
                case CpuState.ReadingOpcode:
                    HandleReadingOpcodeCycle(signalEdge);
                    break;
                case CpuState.HandlingAddressingMode:
                    HandleAddressingModeCycle(signalEdge);
                    break;
                default:
                    throw new InvalidOperationException($"CPU State {state} not supported");
            }
        }

        /// <summary>
        /// Simulate register activity of the CPU during the startup.
        /// </summary>
        private void HandleStartupCycle(SignalEdge signalEdge)
        {
            switch (startupCycleCount)
            {
                case 0:
                    SetRW(RWState.Read);
                    ClearAddressBuffer();
                    break;

                case 6:
                    // Simulate stack activity during startup
                    SetAddressBus(0x01C0);
                    break;

                case 8:
                    // Simulate stack activity during startup
                    SetAddressBus(0x01BF);
                    break;

                case 10:
                    // Simulate stack activity during startup
                    SetAddressBus(0x01BE);
                    break;

                case STARTUP_CYCLES:
                    // Simulate stack activity during startup
                    registers.DecrementStackPointer();
                    registers.DecrementStackPointer();
                    registers.DecrementStackPointer();

                    SetAddressBus(RST_VECTOR_LO);
                    break;

                case STARTUP_CYCLES + 1:
                    registers.SetIRQ();
                    registers.LatchDataBus(ReadFromDataBus());
                    ReadAddressBufferLo();
                    break;

                case STARTUP_CYCLES + 2:
                    registers.SetBRK();
                    SetAddressBus(RST_VECTOR_HI);
                    break;

                case STARTUP_CYCLES + 3:
                    registers.LatchDataBus(ReadFromDataBus());
                    ReadAddressBufferHi();
                    TransitionState(CpuState.ReadingOpcode);
                    break;
            }

            startupCycleCount++;
        }

        /// <summary>
        /// Control reading and decoding of an opcode from memory.
        /// Sets the addressing mode that will determine how the next
        /// cycles are handled.
        /// </summary>
        private void HandleReadingOpcodeCycle(SignalEdge signalEdge)
        {
            if (signalEdge == SignalEdge.FallingEdge)
            {
                registers.PollInternalAluQueue();

                // We have transisiton to next opcode so execute the last instruction
                // if it is a read/internal as this happens on the falling edge..
                if (registers.IR.OperationType != OperationType.Write)
                {
                    registers.IR.Execute(registers);
                }
                else
                {
                    registers.LatchDataBus(registers.InputDataLatch);
                }

                instructionCycleCount = 0;
                SetRW(RWState.Read);
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

        /// <summary>
        /// Control the addressing stage of an instruction lifecycle.
        /// Allows the CPU to handle the logic for supporting each of
        /// the different addressing modes available in the 6502 instruction
        /// set.
        /// </summary>
        private void HandleAddressingModeCycle(SignalEdge signalEdge)
        {
            if (signalEdge == SignalEdge.FallingEdge)
            {
                registers.PollInternalAluQueue();
                if (instructionCycleCount == 1)
                {
                    registers.DecodeOpcode();
                }
            }
            else if (signalEdge == SignalEdge.RisingEdge)
            {
                registers.LatchDataBus(ReadFromDataBus());
            }

            if (registers.IR.AddressingMode == AddressingMode.Implied)
            {
                HandleImpliedAddressingCycle(signalEdge);
            }
            else if (registers.IR.AddressingMode == AddressingMode.Immediate)
            {
                HandleImmediateAddressingCycle(signalEdge);
            }
            else if (registers.IR.AddressingMode == AddressingMode.Relative)
            {
                HandleRelativeAddressingCycle(signalEdge);
            }
            else if (
                registers.IR.AddressingMode == AddressingMode.Absolute ||
                registers.IR.AddressingMode == AddressingMode.AbsoluteX ||
                registers.IR.AddressingMode == AddressingMode.AbsoluteY)
            {
                HandleAbsoluteAddressingCycle(signalEdge);
            }
            else if (
                registers.IR.AddressingMode == AddressingMode.Indirect ||
                registers.IR.AddressingMode == AddressingMode.IndirectX ||
                registers.IR.AddressingMode == AddressingMode.IndirectY)
            {
                HandleIndirectAddressingCycle(signalEdge);
            }
            else if (
                registers.IR.AddressingMode == AddressingMode.ZeroPage ||
                registers.IR.AddressingMode == AddressingMode.ZeroPageX ||
                registers.IR.AddressingMode == AddressingMode.ZeroPageY)
            {
                HandleZeroPageAddressingCycle(signalEdge);
            }
            else
            {
                throw new NotImplementedException(
                    $"Addressing mode {registers.IR.AddressingMode.ToString()} not supported"
                );
            }

            if (signalEdge == SignalEdge.RisingEdge)
            {
                instructionCycleCount++;
            }
        }

        #endregion

        #region Addressing Logic

        /// <summary>
        /// Handles a cycle during implied addressing mode.
        /// During implied addressing there is no addressing logic needed as
        /// the operation is internal.
        /// </summary>
        private void HandleImpliedAddressingCycle(SignalEdge signalEdge)
        {
            if (signalEdge == SignalEdge.FallingEdge)
            {
                if (instructionCycleCount == 1)
                {
                    registers.IncrementProgramCounter();
                    TransferPCToAddressBus();
                }
            }
            else if (signalEdge == SignalEdge.RisingEdge)
            {
                if (instructionCycleCount == 1)
                {
                    addressBuffer = registers.PC;
                    TransitionState(CpuState.ReadingOpcode);
                }
            }
        }

        /// <summary>
        /// Handles a cycle during immediate addressing mode.
        /// During immediate addressing, the data for the operation is supllied
        /// in the operand, rather than at a memory location.
        /// </summary>
        private void HandleImmediateAddressingCycle(SignalEdge signalEdge)
        {
            if (signalEdge == SignalEdge.FallingEdge)
            {
                if (instructionCycleCount == 1)
                {
                    registers.IncrementProgramCounter();
                    TransferPCToAddressBus();
                }
            }
            else if (signalEdge == SignalEdge.RisingEdge)
            {
                if (instructionCycleCount == 1)
                {
                    addressBuffer = (ushort)(registers.PC + 1);
                    TransitionState(CpuState.ReadingOpcode);
                }
            }
        }

        /// <summary>
        /// Handles a cycle during relative addressing mode.
        /// During relative addressing, the desired address is a location relative
        /// to the current PC address, given by a signed byte (-127, 127).
        /// </summary>
        private void HandleRelativeAddressingCycle(SignalEdge signalEdge)
        {
            // TODO
            if (signalEdge == SignalEdge.FallingEdge)
            {
            }
            else if (signalEdge == SignalEdge.RisingEdge)
            {
            }
        }

        /// <summary>
        /// Handles a cycle during absolute addressing mode.
        /// During absolute addressing, the desired address is supplied in the
        /// operand ($HHLL).
        /// </summary>
        private void HandleAbsoluteAddressingCycle(SignalEdge signalEdge)
        {
            if (signalEdge == SignalEdge.FallingEdge)
            {
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
            }
            else if (signalEdge == SignalEdge.RisingEdge)
            {
                if (instructionCycleCount == 1)
                {
                    ReadAddressBufferLo();
                }
                else if (instructionCycleCount == 2)
                {
                    ReadAddressBufferHi();
                    TransitionState(CpuState.ReadingOpcode);
                }
            }
        }

        /// <summary>
        /// Handles a cycle during indirect addressing mode.
        /// During indirect addressing, an address is given with the operand,
        /// which itself stores an address to the data desired. This is
        /// similar to a pointer in higher level languages.
        /// </summary>
        private void HandleIndirectAddressingCycle(SignalEdge signalEdge)
        {
            if (signalEdge == SignalEdge.FallingEdge)
            {
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
                else if (instructionCycleCount == 3)
                {
                    registers.SetProgramCounter(addressBuffer);
                    TransferPCToAddressBus();
                }
                else if (instructionCycleCount == 4)
                {
                    registers.IncrementProgramCounter();
                    TransferPCToAddressBus();
                }
            }
            else if (signalEdge == SignalEdge.RisingEdge)
            {
                if (instructionCycleCount == 1)
                {
                    ReadAddressBufferLo();
                }
                else if (instructionCycleCount == 2)
                {
                    ReadAddressBufferHi();
                }
                else if (instructionCycleCount == 3)
                {
                    ReadAddressBufferLo();
                }
                else if (instructionCycleCount == 4)
                {
                    ReadAddressBufferHi();
                    TransitionState(CpuState.ReadingOpcode);
                }
            }
        }

        /// <summary>
        /// Handles a cycle during zero page addressing mode.
        /// During zero page addressing, the address of a zero page memory is
        /// given in the operand.
        /// </summary>
        private void HandleZeroPageAddressingCycle(SignalEdge signalEdge)
        {
            if (signalEdge == SignalEdge.FallingEdge)
            {
                if (instructionCycleCount == 1)
                {
                    registers.IncrementProgramCounter();
                    TransferPCToAddressBus();
                }
                else if (instructionCycleCount == 2)
                {
                    SetAddressBus(addressBuffer);
                    registers.IncrementProgramCounter();
                    addressBuffer = registers.PC;
                    if (registers.IR.OperationType == OperationType.Write)
                    {
                        registers.LatchInputDataBusBuffer();
                        SetRW(RWState.Write);
                    }
                    else
                    {
                        SetRW(RWState.Read);
                    }
                }
            }
            else if (signalEdge == SignalEdge.RisingEdge)
            {
                if (instructionCycleCount == 1)
                {
                    addressBuffer = registers.DataBusBuffer;
                }
                else if (instructionCycleCount == 2)
                {
                    if (registers.IR.OperationType == OperationType.Write)
                    {
                        // We have transisiton to next opcode so execute the last instruction
                        // if it is a write operation as this happens on the rising edge..
                        registers.IR.Execute(registers);
                        SetRW(RWState.Write);
                    }
                    else
                    {
                        SetRW(RWState.Read);
                    }
                    TransitionState(CpuState.ReadingOpcode);
                }
            }
        }

        #endregion

        private CpuState state;
        private int startupCycleCount;
        private int instructionCycleCount;
        private CpuRegisters registers;
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
