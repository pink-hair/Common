namespace PinkHair.Common.Telemetron
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;

    /// <summary>
    /// An interface for a trace provider.
    /// </summary>
    public interface ITraceProvider
    {
        /// <summary>
        /// Trace the specified message. Optionall include additional data to suppliment log stream.
        /// </summary>
        /// <param name="eventSeverity">The severity of the event.</param>
        /// <param name="message">Message.</param>
        /// <param name="codePoint">Code point.</param>
        /// <param name="data">Data.</param>
        /// <param name="callerMemberName">Caller member name.</param>
        /// <param name="callerFilePath">Caller file path.</param>
        /// <param name="callerLineNumber">Caller line number.</param>
        /// <returns>A value indicating if the emission of the trace event was successful.</returns>
        bool Trace(
            EventSeverity eventSeverity,
            string message,
            string codePoint = null,
            Dictionary<string, string> data = null,
            [CallerMemberName]string callerMemberName = "",
            [CallerFilePath]string callerFilePath = "",
            [CallerLineNumber]int callerLineNumber = -1);

        /// <summary>
        /// Trace the specified message. Optionall include additional data to suppliment log stream.
        /// </summary>
        /// <param name="eventSeverity">The severity of the event.</param>
        /// <param name="message">Message.</param>
        /// <param name="exception">The exception that generated this event.</param>
        /// <param name="codePoint">Code point.</param>
        /// <param name="data">Data.</param>
        /// <param name="callerMemberName">Caller member name.</param>
        /// <param name="callerFilePath">Caller file path.</param>
        /// <param name="callerLineNumber">Caller line number.</param>
        /// <returns>A value indicating if the emission of the trace event was successful.</returns>
        bool Trace(
        EventSeverity eventSeverity,
            string message,
            Exception exception,
            string codePoint = null,
            Dictionary<string, string> data = null,
            [CallerMemberName]string callerMemberName = "",
            [CallerFilePath]string callerFilePath = "",
            [CallerLineNumber]int callerLineNumber = -1);


    }
}
