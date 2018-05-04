namespace Polytech.Common.Telemetron
{
    /// <summary>
    /// The enumeration for the action to take when a codepoint object is null. Used by implementing providers.
    /// </summary>
    public enum EmptyCodepointAction
    {
        /// <summary>
        /// Pass an empty string / do not emit the object.
        /// </summary>
        DoNothing = 0,

        /// <summary>
        /// Use the delegate method provided to compute the value of the codepoint.
        /// </summary>
        UseDelegateMethod = 1
    }
}
