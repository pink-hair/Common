namespace Polytech.Common.Telemetron.Configuration
{
    using Common.Telemetron.Configuration;

    /// <summary>
    /// A configuration class for Application Insights Telemetrons.
    /// </summary>
    public class ApplicationInsightsTelemetronConfiguration : TelemetronConfigurationBase, IApplicationInsightsTelemetronConfiguration
    {
        /// <summary>
        /// Gets or sets the instrumentation Key. This is required.
        /// </summary>
        public string InstrumentationKey { get; set; }
    }
}
