namespace PinkHair.Common.Telemetron
{
    /// <summary>
    /// The result of an operation.
    /// </summary>
    public enum OperationResult
    {
        /// <summary>
        /// The result of the operation has not been set.
        /// </summary>
        NotSet = 0,

        /// <summary>
        /// The result of the operation has been marked as failed by the consumer, but did not supply and exception.
        /// </summary>
        Failed = 1,

        /// <summary>
        /// The result of the operation is success.
        /// </summary>
        Success = 2,

        /// <summary>
        /// The Result of the operation is failed by Exception.
        /// </summary>
        Exceptional = 3,
    }
}
