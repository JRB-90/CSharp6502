namespace CS6502.Core
{
    /// <summary>
    /// Enum to contain the default states a wire can be in.
    /// Used to resolve the state of a wire when no pins are connected.
    /// </summary>
    public enum WirePull
    {
        PullDown,
        PullUp
    }
}
