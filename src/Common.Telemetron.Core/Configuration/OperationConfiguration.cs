namespace Polytech.Common.Telemetron.Configuration
{
    /// <summary>
    /// Configuration Class for Operations.
    /// </summary>
    public class OperationConfiguration : IOperationConfiguration
    {
        /// <summary>
        /// Gets or sets a value indicating whether to emit the metrics generated from an operation directly.
        /// </summary>
        public bool EmitOperationMetrics { get; set; }
    }
}
