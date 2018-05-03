namespace Polytech.Common.Telemetron
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;
    using Common.Extension.Core;
    using Common.Telemetron.Configuration;
    using Common.Telemetron.Diagnostics;
    using Microsoft.ApplicationInsights;
    using Microsoft.ApplicationInsights.DataContracts;
    using Microsoft.ApplicationInsights.Extensibility;

    /// <summary>
    /// 
    /// </summary>
    public class ApplicationInsightsTelemetron : CorrelatedProviderBase, ITelemetronProvider<byte[]>
    {
        TelemetryClient internalTelemetryClient;

        public ApplicationInsightsTelemetron(IApplicationInsightsTelemetronConfiguration configuration)
            : base(configuration)
        {
            if (configuration == null)
            {
                throw new ArgumentNullException(nameof(configuration));
            }

            if (string.IsNullOrWhiteSpace(configuration.InstrumentationKey))
            {
                throw new ArgumentNullException($"{nameof(configuration)}.{nameof(configuration.InstrumentationKey)}");
            }

            this.CorrelationContext = new CorrelationContext(16713); // 4149 in hex or 'A' 'I' in Ascii

            TelemetryConfiguration tc = new TelemetryConfiguration(configuration.InstrumentationKey);
            this.internalTelemetryClient = new TelemetryClient(tc);
        }

        protected internal TelemetryClient TelemetryClient => this.internalTelemetryClient;

        public IOperation CreateOperation(string operationName)
        {
            try
            {
                ICorrelationContext localCorrelationcontext = this.CorrelationContext;
                long newOperationId = localCorrelationcontext.AddOperation();
                string cc = localCorrelationcontext.ToString();

                this.CorrelationContext = localCorrelationcontext;

                IOperation createdOperation = new ApplicationInsightsOperation(this, operationName, newOperationId.GetBase64String(), cc);

                return createdOperation;
            }
            catch (Exception ex)
            {
                DiagnosticTrace.Instance.Error("An unexpected error occurred when attempting to create an operation", ex, "FHnm64k800M");

                return new NullOperation();
            }
        }

        public IOperation CreateOperation(string operationName, byte[] correlationContext)
        {
            try
            {
                byte[] capturedCorrelationContext = this.CorrelationContext.Capture();

                try
                {
                    CorrelationContext localCorrelationcontext = new CorrelationContext(correlationContext);

                    long newOperationId = localCorrelationcontext.AddOperation();
                    string cc = localCorrelationcontext.ToString();

                    this.CorrelationContext = localCorrelationcontext;

                    IOperation createdOperation = new ApplicationInsightsOperation(this, operationName, newOperationId.GetBase64String(), cc);

                    return createdOperation;
                }
                catch (Exception ex)
                {
                    DiagnosticTrace.Instance.Error("An unexpected error occurred when attempting to create an operation", ex, "7AKlKXaBwkM");

                    return new NullOperation();
                }
                finally
                {
                    this.CorrelationContext = new CorrelationContext(capturedCorrelationContext);
                }
            }
            catch (Exception ex)
            {
                DiagnosticTrace.Instance.Error("An unexpected error occurred when attempting to reinstate the correlation context", ex, "t/30e9t+2kM");

                return new NullOperation();
            }
        }

        public bool Event(string name, Dictionary<string, string> data = null)
        {
            try
            {
                EventTelemetry telemetry = new EventTelemetry(name);
                telemetry.SetOperationInfo(this);
                telemetry.MergeProperties(data.Safe());

                this.internalTelemetryClient.TrackEvent(telemetry);

                return true;
            }
            catch (Exception ex)
            {
                DiagnosticTrace.Instance.Error("An unexpected error occured when attempting to track an event", ex, "Y6MjRLHRwUM");

                return false;
            }
        }

        public bool Metric(string name, double value = 1, Dictionary<string, string> data = null)
        {
            try
            {
                MetricTelemetry telemetry = new MetricTelemetry(name, value);
                telemetry.SetOperationInfo(this);
                telemetry.MergeProperties(data.Safe());

                this.internalTelemetryClient.TrackMetric(telemetry);

                return true;
            }
            catch (Exception ex)
            {
                DiagnosticTrace.Instance.Error("An unexpected error occured when attempting to track a metric", ex, "L7i4RxdczEM");

                return false;
            }
        }

        public bool Trace(EventSeverity eventSeverity, string message, string codePoint = null, Dictionary<string, string> data = null, [CallerMemberName] string callerMemberName = "", [CallerFilePath] string callerFilePath = "", [CallerLineNumber] int callerLineNumber = -1)
        {
            return TraceInternal(eventSeverity, message, codePoint, data, callerMemberName, callerFilePath, callerLineNumber);
        }

        private bool TraceInternal(EventSeverity eventSeverity, string message, string codePoint, Dictionary<string, string> data, string callerMemberName, string callerFilePath, int callerLineNumber)
        {
            try
            {
                SeverityLevel sl;

                switch (eventSeverity)
                {
                    case EventSeverity.Verbose:
                    case EventSeverity.Debug:
                        sl = SeverityLevel.Verbose;
                        break;
                    case EventSeverity.Metric:
                    case EventSeverity.OperationInfo:
                    case EventSeverity.Event:
                    case EventSeverity.Info:
                        sl = SeverityLevel.Information;
                        break;
                    case EventSeverity.Warning:
                        sl = SeverityLevel.Warning;
                        break;
                    case EventSeverity.Error:
                    case EventSeverity.OperationError:
                        sl = SeverityLevel.Error;
                        break;
                    case EventSeverity.Fatal:
                        sl = SeverityLevel.Critical;
                        break;
                    default:
                        sl = SeverityLevel.Verbose;
                        break;
                }

                Dictionary<string, string> props = data.Safe();

                TraceTelemetry tt = new TraceTelemetry()
                {
                    Message = message,
                    Timestamp = DateTimeOffset.UtcNow,
                    SeverityLevel = sl
                };

                this.AddCodepoint(message, codePoint, callerMemberName, callerFilePath, callerLineNumber, props);
                this.AddCallerFilePath(callerFilePath, props);
                this.AddCallerLineNumber(callerLineNumber, props);
                this.AddCallerMemberName(callerMemberName, props);

                if (this.EmitAdditionalData)
                {
                    tt.MergeProperties(props);
                }

                // set operational properties 
                tt.SetOperationInfo(this);

                this.internalTelemetryClient.TrackTrace(tt);

                return true;
            }
            catch (Exception ex)
            {
                DiagnosticTrace.Instance.Error("Unexpected error occured when emitting trace.", ex, "7eI+iHZxj0M");

                return true;
            }
        }

        public bool Trace(EventSeverity eventSeverity, string message, Exception exception, string codePoint = null, Dictionary<string, string> data = null, [CallerMemberName] string callerMemberName = "", [CallerFilePath] string callerFilePath = "", [CallerLineNumber] int callerLineNumber = -1)
        {
            return this.TraceInternal(eventSeverity, message, codePoint, data.SafeCombine(exception), callerMemberName, callerFilePath, callerLineNumber);
        }
    }
}
