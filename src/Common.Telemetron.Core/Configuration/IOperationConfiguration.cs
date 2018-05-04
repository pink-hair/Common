namespace Polytech.Common.Telemetron.Configuration
{
    /// <summary>
    /// Configuration interface for Operations.
    /// </summary>
    public interface IOperationConfiguration
    {
        /// <summary>
        /// Gets a value indicating whether to emit the metrics generated from an operation directly.
        /// </summary>
        bool EmitOperationMetrics { get; }
    }
}
