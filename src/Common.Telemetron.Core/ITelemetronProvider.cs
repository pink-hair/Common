namespace PinkHair.Common.Telemetron
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    /// <summary>
    /// A Telemetron Provider. 
    /// </summary>
    /// <typeparam name="T">The Correlation Context type that will be used when restoring Operation Correlation.</typeparam>
    public interface ITelemetronProvider<T> : ITraceProvider, IOperationProvider<T>, IMetricProvider, IEventProvider, ICorrelatedProvider
    {
        // summary Interface 
    }
}
