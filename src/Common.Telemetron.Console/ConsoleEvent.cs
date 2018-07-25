namespace PinkHair.Common.Telemetron
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using PinkHair.Common.Telemetron;

    /// <summary>
    /// Data Package to hold a console event for thread aggregation.
    /// </summary>
    internal class ConsoleEvent
    {
        internal DateTime EventTime { get; set; }

        internal EventSeverity EventSeverity { get; set; }

        internal string Message { get; set; }

        internal string CodePoint { get; set; }

        internal Dictionary<string, string> Data { get; set; }

        internal string CallerMemberName { get; set; }

        internal string CallerFilePath { get; set; }

        internal int CallerLineNumber { get; set; }

        internal string CorrelationContext { get; set; }
    }
}
