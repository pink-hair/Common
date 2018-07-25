namespace PinkHair.Common.Telemetron
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;
    using System.Text;
    using PinkHair.Common.Extension;
    using PinkHair.Common.Telemetron.Configuration;

    public abstract class TextWriterTelemetronBase : CorrelatedProviderBase, ITraceProvider
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TextWriterTelemetronBase"/> class.
        /// </summary>
        /// <param name="configuration">The configuration for this provider.</param>
        protected TextWriterTelemetronBase(ITelemetronConfigurationBase configuration)
            : base(configuration)
        {
        }

        public bool Event(string name, Dictionary<string, string> data = null)
        {
            return this.Trace(EventSeverity.Event, this.CreateEventString(name), null, data, "", "", -1);
        }

        public bool Metric(string name, double value = 1, Dictionary<string, string> data = null)
        {
            return this.Trace(EventSeverity.Metric, this.CreateMetricString(name, value), null, data, "", "", -1);
        }

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
        public abstract bool Trace(EventSeverity eventSeverity, string message, string codePoint = null, Dictionary<string, string> data = null, [CallerMemberName] string callerMemberName = "", [CallerFilePath] string callerFilePath = "", [CallerLineNumber] int callerLineNumber = -1);

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
        public abstract bool Trace(EventSeverity eventSeverity, string message, Exception exception, string codePoint = null, Dictionary<string, string> data = null, [CallerMemberName] string callerMemberName = "", [CallerFilePath] string callerFilePath = "", [CallerLineNumber] int callerLineNumber = -1);

        /// <summary>
        /// Creates the string that will be displayed as the message when an event is fired.
        /// </summary>
        /// <param name="eventName">The name of the event.</param>
        /// <returns>The event string.</returns>
        protected virtual string CreateEventString(string eventName)
        {
            return '{' + eventName + '}';
        }

        /// <summary>
        /// Creates the string that will be displayed as the message when a metric is fired.
        /// </summary>
        /// <param name="metricName">The name of the metric.</param>
        /// <param name="metricValue">The value of the metric.</param>
        /// <returns>The metric string.</returns>
        protected virtual string CreateMetricString(string metricName, double metricValue)
        {
            return '{' + metricName + ':' + metricValue.ToString() + '}';
        }

        protected virtual string CreateLogMessage(EventSeverity eventSeverity, string message, string codePoint, Dictionary<string, string> data, string callerMemberName, string callerFilePath, int callerLineNumber, DateTime now)
        {
            Dictionary<string, string> props = data.Safe();

            if (!this.EmitAdditionalData)
            {
                props = new Dictionary<string, string>();
            }

            this.AddCodepoint(message, codePoint, callerMemberName, callerFilePath, callerLineNumber, props);
            this.AddCallerFilePath(callerFilePath, props);
            this.AddCallerLineNumber(callerLineNumber, props);
            this.AddCallerMemberName(callerMemberName, props);

            if (this.EmitCorrelationContext)
            {
                props[nameof(this.CorrelationContext)] = this.CorrelationContext.Get();
            }

            this.CreateLogMessageAppendSpecificTelemetry(props);

            string json = Newtonsoft.Json.JsonConvert.SerializeObject(props);

            string log = $"[{GetTimeString(now)}][{eventSeverity.ToString()}]{message}|{json}";
            return log;
        }

        protected virtual void CreateLogMessageAppendSpecificTelemetry(Dictionary<string, string> props)
        {
            // doing nothing here is OK.
            // doesnt NEED to be implemented
        }
    }
}
