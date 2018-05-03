namespace Polytech.Common.Telemetron.Diagnostics
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
        private static DiagnosticTrace instance = new DiagnosticTrace();

        private TraceSource diagnosticTraceSource;

        public DiagnosticTrace()
        {
            this.diagnosticTraceSource = new TraceSource(Constants.EventConstants.TraceSourceId, SourceLevels.All);

        }

        public static DiagnosticTrace Instance => instance;

        public void ListenWith(TraceListener listener)
        {
            this.diagnosticTraceSource.Listeners.Add(listener);
        }

        public bool Trace(EventSeverity eventSeverity, string message, string codePoint = null, Dictionary<string, string> data = null, [CallerMemberName] string callerMemberName = "", [CallerFilePath] string callerFilePath = "", [CallerLineNumber] int callerLineNumber = -1)
        {
            try
            {

                string msg = CreateTraceMessage(message, null, codePoint, data);

                this.diagnosticTraceSource.TraceEvent(
                    TranslateEvent(eventSeverity),
                    1,
                    msg);

                return true;
            }
            catch
            {
                // we are lost
                return false;
            }
        }

        public bool Trace(EventSeverity eventSeverity, string message, Exception exception, string codePoint = null, Dictionary<string, string> data = null, [CallerMemberName] string callerMemberName = "", [CallerFilePath] string callerFilePath = "", [CallerLineNumber] int callerLineNumber = -1)
        {
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
                    1,
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
