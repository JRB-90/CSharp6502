namespace CS6502.Core
{
    /// <summary>
    /// Enum to hold all the different microcode instructions.
    /// </summary>
    internal enum MicroCodeInstruction
    {
        // CPU State
        SetToRead,
        SetToWrite,
        DecrementSP,
        IncrementSP,
        TransferSPIntoPCHS,

        // Status
        ClearCarry,
        SetCarry,
        ClearIRQ,
        SetIRQ,
        ClearDecimal,
        SetDecimal,
        ClearOverflow,
        ClearZero,
        SetZero,
        TransferDataIntoP,
        ShiftLowABitIntoCarry,
        ShiftHighABitIntoCarry,
        ShiftLowDILBitIntoCarry,
        ShiftHighDILBitIntoCarry,

        // IR
        LatchIRToData,

        // Registers
        LatchDILIntoA,
        LatchDILIntoX,
        LatchDILIntoY,
        TransferXToSP,
        TransferAToX,
        TransferAToY,
        TransferSPToX,
        TransferXToA,
        TransferYToA,

        // ALU
        UpdateFlagsOnHold,
        TransferHoldToDOR,
        TransferHoldToA,
        TransferHoldToX,
        TransferHoldToY,
        IncrementA,
        IncrementX,
        IncrementY,
        DecrementA,
        DecrementX,
        DecrementY,
        INC,
        DEC,
        ADC,
        SBC,
        AND,
        ORA,
        EOR,
        ASL,
        ASL_A,
        LSR,
        LSR_A,
        ROL,
        ROL_A,
        ROR,
        ROR_A,
        BIT,

        // PC
        TransferDILToPCLS,
        TransferDILToPCHS,
        TransferPCLToPCLS,
        TransferPCHToPCHS,
        TransferPCToPCS,
        TransferPCLSToPCL,
        TransferPCHSToPCH,
        TransferPCHSToSP,
        TransferPCSToPC_NoIncrement,
        TransferPCSToPC_WithCarryIncrement,
        IncrementPC,
        IncrementPCLS,
        IncrementPCHS,
        DecrementPCLS,
        DecrementPCHS,

        // Address
        TransferPCToAddressBus,
        TransferPCSToAddressBus,
        TransferPCHSToABL,
        TransferZPDataToAB,
        TransferDILToABL,
        TransferDILToABH,
        IncrementAB_NoCarry,
        IncrementABByX,
        IncrementABByY,
        IncrementABByY_WithCarry,
        TransferSPToAB,
        TransferABLToSP,

        // Data
        LatchDataIntoDIL,
        LatchDILIntoDOR,
        LatchAIntoDOR,
        LatchXIntoDOR,
        LatchYIntoDOR,
        LatchPIntoDOR,
        LatchPCLIntoDOR,
        LatchPCHIntoDOR,
        LatchDILIntoSP
    }
}
