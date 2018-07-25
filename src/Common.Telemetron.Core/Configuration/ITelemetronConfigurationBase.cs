namespace PinkHair.Common.Telemetron.Configuration
{
    /// <summary>
    /// Base configuration interface for all telemetrons.
    /// </summary>
    public interface ITelemetronConfigurationBase
    {
        /// <summary>
        /// Gets or sets the Delegate that will be ran when no codepoint is provided to the logging methods. fd
        /// </summary>
        /// <value>The null codepoint action.</value>
        TelemetronNullCodePointDelegte NullCodepointAction { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to emit the CodePoint as part of the emitted event.
        /// </summary>
        bool EmitCodePoint { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to emit the callerMemberName as part of the event.
        /// </summary>
        bool EmitCallerMemberName { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to emit the callerFilePath as part of the event.
        /// </summary>
        bool EmitCallerFilePath { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to emit the callerLineNumber as part of the event.
        /// </summary>
        bool EmitCallerLineNumber { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to emit any of the additional data collected with the event.
        /// </summary>
        bool EmitAdditionalData { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether 
        /// </summary>
        bool EmitCorrelationContext { get; set; }

        /// <summary>
        /// Gets the configuration used for Operations.
        /// </summary>
        IOperationConfiguration OperationConfiguration { get; }
    }
}