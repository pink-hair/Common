namespace Polytech.Common.Telemetron
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    [Flags]
    public enum EventSeverity
    {
        Silent = 0,
        Debug = 1 << 0,
        Verbose = 1 << 1,
        Info = 1 << 2,
        Warning = 1 << 3,
        Error = 1 << 4,
        Fatal = 1 << 5,
        OperationInfo = 1 << 6,
        OperationError = 1 << 7,
        Event = 1 << 8,
        Metric = 1 << 9
    }
}
