namespace Polytech.Common.Telemetron.Configuration
{
    /// <summary>
    /// Base class for Telemetron Configurations
    /// </summary>
    public abstract class TelemetronConfigurationBase : ITelemetronConfigurationBase
    {
        /// <summary>
        /// Gets or sets the Delegate that will be ran when no codepoint is provided to the logging methods. fd
        /// </summary>
        /// <value>The null codepoint action.</value>
        public TelemetronNullCodePointDelegte NullCodepointAction { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to emit the CodePoint as part of the emitted event.
        /// </summary>
        public bool EmitCodePoint { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to emit the callerMemberName as part of the event.
        /// </summary>
        public bool EmitCallerMemberName { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to emit the callerFilePath as part of the event.
        /// </summary>
        public bool EmitCallerFilePath { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to emit the callerLineNumber as part of the event.
        /// </summary>
        public bool EmitCallerLineNumber { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to emit any of the additional data collected with the event.
        /// </summary>
        public bool EmitAdditionalData { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether 
        /// </summary>
        public bool EmitCorrelationContext { get; set; }

        /// <summary>
        /// Gets or sets the configuration used for Operations.
        /// </summary>
        public OperationConfiguration OperationConfiguration { get; set; }

        /// <summary>
        /// Gets the configuration used for Operations.
        /// </summary>
        IOperationConfiguration ITelemetronConfigurationBase.OperationConfiguration => this.OperationConfiguration;
    }
}
