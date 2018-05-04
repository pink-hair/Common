namespace Polytech.Common.Telemetron
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    [Flags]
    public enum EventSeverity
    {
        /// <summary>
        /// Silent. This event should not be recorded.
        /// </summary>
        Silent = 0,
        
        /// <summary>
        /// Debug level event. 
        /// </summary>
        Debug = 1 << 0,

        /// <summary>
        /// Verbuse level event.
        /// </summary>
        Verbose = 1 << 1,

        /// <summary>
        /// Informational. This level is relevant to the every day health of the app.
        /// </summary>
        Info = 1 << 2,

        /// <summary>
        /// A Warning. This is an indicator that a codition is anomolous.
        /// </summary>
        Warning = 1 << 3,

        /// <summary>
        /// An error has occured, the application can recover from this error, but the transaction containing it has failed.
        /// </summary>
        Error = 1 << 4,

        /// <summary>
        /// An error has occured, and the application cannot recover.
        /// </summary>
        Fatal = 1 << 5,

        /// <summary>
        /// Informational level about an operation.
        /// </summary>
        OperationInfo = 1 << 6,

        /// <summary>
        /// Recoverable Error from an operation.
        /// </summary>
        OperationError = 1 << 7,

        /// <summary>
        /// This is an analytics event.
        /// </summary>
        Event = 1 << 8,

        /// <summary>
        /// This is a metric.
        /// </summary>
        Metric = 1 << 9
    }
}
