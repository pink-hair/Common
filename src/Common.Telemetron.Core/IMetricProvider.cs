namespace PinkHair.Common.Telemetron
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    /// <summary>
    /// A provider that emits metrics
    /// </summary>
    public interface IMetricProvider
    {
        /// <summary>
        /// Emits a metric with the specified <paramref name="name"/> and <paramref name="value"/>. Optional data.
        /// </summary>
        /// <param name="name">The name or Id of the metric.</param>
        /// <param name="value">The value of the Metric</param>
        /// <param name="data">Additional Dimensional Data.</param>
        /// <returns>a value indicating if emiting the metric was successful.</returns>
        bool Metric(string name, double value = 1, Dictionary<string, string> data = null);
    }
}
