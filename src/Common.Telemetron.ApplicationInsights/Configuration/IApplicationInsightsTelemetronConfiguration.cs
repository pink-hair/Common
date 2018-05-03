namespace Polytech.Common.Telemetron.Configuration
{
    /// <summary>
    /// Interface for Application Insights Telemetron Configuration
    /// </summary>
    public interface IApplicationInsightsTelemetronConfiguration : ITelemetronConfigurationBase
    {
        /// <summary>
        /// Gets the Instrumentation Key, this is how AI routes telemetry
        /// </summary>
        string InstrumentationKey { get; }
    }
}