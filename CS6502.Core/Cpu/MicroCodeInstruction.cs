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

        // IR
        LatchIRToData,

        // Registers
        LatchDataIntoA,
        LatchDataIntoX,
        LatchDataIntoY,
        IncrementA,
        IncrementX,
        IncrementY,

        // ALU

        // PC
        TransferDataToPCLS,
        TransferDataToPCHS,
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
        TransferZPDataToAB,
        TransferDataToABL,
        TransferDataToABH,

        // Data
        LatchDataBus,
        LatchAIntoData,
        LatchXIntoData,
        LatchYIntoData,
    }
}
