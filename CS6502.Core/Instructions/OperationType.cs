namespace CS6502.Core
{
    /// <summary>
    /// Enum to collate the different types of operations an instruction
    /// can do, eg read, internal, write.
    /// This allows a higher level to understand what to do on the last
    /// instruction cycle.
    /// </summary>
    public enum OperationType
    {
        Internal,
        Read,
        Write
    }
}
