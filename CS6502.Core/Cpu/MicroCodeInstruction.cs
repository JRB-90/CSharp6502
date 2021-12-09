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

        // Status
        ClearCarry,
        SetCarry,
        ClearIRQ,
        SetIRQ,
        ClearDecimal,
        SetDecimal,
        ClearOverflow,

        // IR
        LatchIRToData,

        // Registers
        LatchDILIntoA,
        LatchDILIntoX,
        LatchDILIntoY,
        IncrementA,
        IncrementX,
        IncrementY,
        DecrementA,
        DecrementX,
        DecrementY,
        TransferXToSP,
        TransferAToX,
        TransferAToY,
        TransferSPToX,
        TransferXToA,
        TransferYToA,

        // ALU
        TransferHoldToA,
        TransferHoldToX,
        TransferHoldToY,

        // PC
        TransferDILToPCLS,
        TransferDILToPCHS,
        TransferPCLToPCLS,
        TransferPCHToPCHS,
        TransferPCToPCS,
        TransferPCLSToPCL,
        TransferPCHSToPCH,
        TransferPCSToPC_NoIncrement,
        TransferPCSToPC_WithCarryIncrement,
        IncrementPC,

        // Address
        TransferPCToAddressBus,
        TransferPCSToAddressBus,
        TransferZPDataToAB,
        TransferDILToABL,
        TransferDILToABH,
        IncrementAB_NoCarry,
        IncrementABByX,
        IncrementABByY,
        IncrementABByY_WithCarry,

        // Data
        LatchDataIntoDIL,
        LatchDILIntoDOR,
        LatchAIntoDOR,
        LatchXIntoDOR,
        LatchYIntoDOR,
    }
}
