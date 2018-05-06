using System;
using System.Collections.Generic;
using System.Text;

namespace Polytech.Common.Telemetron.ApplicationInsights.Tests
{
    using Polytech.Common.Telemetron.Configuration;

    internal static class Common
    {
        internal static ApplicationInsightsTelemetron Get()
        {
            ApplicationInsightsTelemetronConfiguration aitc = GetConfiguration();

            ApplicationInsightsTelemetron ai = new ApplicationInsightsTelemetron(aitc);

            return ai;
        }

        internal static ApplicationInsightsTelemetronConfiguration GetConfiguration()
        {
            return new ApplicationInsightsTelemetronConfiguration()
            {
                InstrumentationKey = "3ed73856-b500-460f-9ff5-dfdb3749cb20",
                OperationConfiguration = new OperationConfiguration()
                {
                    EmitOperationMetrics = true
                }
            };
        }
    }
}
