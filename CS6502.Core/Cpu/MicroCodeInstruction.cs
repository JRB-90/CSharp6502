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
        TransferDataIntoP,

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
