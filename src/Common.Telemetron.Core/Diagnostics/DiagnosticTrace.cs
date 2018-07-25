namespace PinkHair.Common.Telemetron.Diagnostics
{
    using Newtonsoft.Json;
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Runtime.CompilerServices;
    using System.Text;

    public class DiagnosticTrace :
        ITraceProvider
    {
        private static bool TelemetryEnabled = true;

        private static DiagnosticTrace instance = new DiagnosticTrace();

        private TraceSource diagnosticTraceSource;

        public DiagnosticTrace()
        {
            this.diagnosticTraceSource = new TraceSource(Constants.EventConstants.TraceSourceId, SourceLevels.All);

        }

        public static DiagnosticTrace Instance => instance;

        internal static TraceSource Source => instance.diagnosticTraceSource;

        public void ListenWith(TraceListener listener)
        {
            this.diagnosticTraceSource.Listeners.Add(listener);
        }

        public bool Trace(EventSeverity eventSeverity, string message, string codePoint = null, Dictionary<string, string> data = null, [CallerMemberName] string callerMemberName = "", [CallerFilePath] string callerFilePath = "", [CallerLineNumber] int callerLineNumber = -1)
        {
            return this.Trace(eventSeverity, message, null, codePoint, data, callerMemberName, callerFilePath, callerLineNumber);
        }

        public bool Trace(EventSeverity eventSeverity, string message, Exception exception, string codePoint = null, Dictionary<string, string> data = null, [CallerMemberName] string callerMemberName = "", [CallerFilePath] string callerFilePath = "", [CallerLineNumber] int callerLineNumber = -1)
        {
            if (!TelemetryEnabled)
            {
                return true;
            }

            try
            {
                string msg = CreateTraceMessage(message, exception, codePoint, data);

                int randoId;
                unchecked
                {
                    int hash = (int)2166136261;

                    hash = (hash * 16777619) ^ (int)eventSeverity;
                    hash = (hash * 16777619) ^ (int)msg.GetHashCode();

                    randoId = Math.Abs(hash) % 65535;
                }

                this.diagnosticTraceSource.TraceEvent(
                    TranslateEvent(eventSeverity),
                    randoId,
                    msg);

                return true;
            }
            catch
            {
                // we are lost
                return false;
            }
        }

        public static bool Filter(string message, Exception ex)
        {
            Instance.Trace(EventSeverity.Error, message, ex);
            return false;
        }

        public static bool Diag(string message) => Instance.Info(message);

        public static bool Diag(string message, Exception exception) => Instance.Error(message, exception);

        public static void DisableTelemetry()
        {
            TelemetryEnabled = false;
        }

        private static string CreateTraceMessage(string message, Exception exception, string codePoint, Dictionary<string, string> data)
        {
            var combinedData = data.SafeCombine(exception);
            string newMessage;

            if (combinedData.Count > 0)
            {
                newMessage = $"{message} | cp: {codePoint ?? "unknown"} | { JsonConvert.SerializeObject(data, Formatting.Indented) }";
            }
            else
            {
                newMessage = $"{message} | cp: {codePoint ?? "unknown"}";
            }

            return newMessage;
        }

        private static TraceEventType TranslateEvent(EventSeverity severity)
        {
            switch (severity)
            {
                case EventSeverity.Debug:
                    return TraceEventType.Verbose;
                case EventSeverity.Verbose:
                    return TraceEventType.Verbose;
                case EventSeverity.Info:
                case EventSeverity.Event:
                case EventSeverity.Metric:
                case EventSeverity.OperationInfo:
                case EventSeverity.Silent:
                    return TraceEventType.Information;
                case EventSeverity.Warning:
                    return TraceEventType.Warning;
                case EventSeverity.Error:
                case EventSeverity.OperationError:
                    return TraceEventType.Error;
                case EventSeverity.Fatal:
                    return TraceEventType.Critical;
                default:
                    return TraceEventType.Information;
            }
        }
    }
}
