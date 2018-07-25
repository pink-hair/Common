namespace PinkHair.Common.Telemetron
{
    using System;
    using System.Collections.Concurrent;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;
    using System.Threading;
    using System.Threading.Tasks;
    using Newtonsoft.Json;
    using PinkHair.Common.Extension;
    using PinkHair.Common.Telemetron;
    using PinkHair.Common.Telemetron.Configuration;
    using PinkHair.Common.Telemetron.Diagnostics;
    using static PinkHair.Common.Telemetron.Diagnostics.DiagnosticTrace;

    /// <summary>
    /// A Telemetron for console output.
    /// </summary>
    public partial class ConsoleTelemetron : CorrelatedProviderBase, ITelemetronProvider<byte[]>, IDisposable
    {
        private ConcurrentQueue<ConsoleEvent> eventQueue;
        private Task queueTask;
        private CancellationTokenSource cancellationTokenSource;
        private CancellationToken cancellationToken;

        /// <summary>
        /// Initializes a new instance of the <see cref="ConsoleTelemetron"/> class.
        /// </summary>
        /// <param name="configuration"></param>
        public ConsoleTelemetron(IConsoleConfiguration configuration)
    : base(configuration)
        {
            this.CopyConfigLocal(configuration);

            this.CorrelationContext = new CorrelationContext(1337);

            this.cancellationTokenSource = new CancellationTokenSource();
            this.cancellationToken = this.cancellationTokenSource.Token;

            this.eventQueue = new ConcurrentQueue<ConsoleEvent>();

            this.queueTask = this.QueueJob(this.cancellationToken);
        }

        /// <summary>
        /// Creates a new operation.
        /// </summary>
        /// <param name="operationName">The name of the operation to create.</param>
        /// <returns>A correlated Operation.</returns>
        public IOperation CreateOperation(string operationName)
        {
            try
            {
                ICorrelationContext localCorrelationcontext = this.CorrelationContext;
                long newOperationId = localCorrelationcontext.AddOperation();

                string newOperationIdString = newOperationId.GetBase64String();
                if (string.IsNullOrWhiteSpace(operationName))
                {
                    Diag("Attempting to create operation with null name. Resetting to randomized value. FTOsTooZ1kM");
                    operationName = "ERR_NO_OPERATION_NAME " + newOperationIdString;
                }

                string cc = localCorrelationcontext.ToString();

                this.CorrelationContext = localCorrelationcontext;

                IOperation createdOperation = new ConsoleOperation(this, operationName, newOperationIdString, cc);

                return createdOperation;
            }
            catch (Exception ex)
            {
                DiagnosticTrace.Instance.Error("An unexpected error occurred when attempting to create an operation", ex, "1f3803c8-5a8c-4562-a96b-7069520d8e32");

                return new NullOperation();
            }
        }

        /// <summary>
        /// Creates a new operation.
        /// </summary>
        /// <param name="operationName">The name of the operation to create.</param>
        /// <param name="parentContext">Create an operation using another context for the duration of the operation.</param>
        /// <returns>A correlated Operation.</returns>

        public IOperation CreateOperation(string operationName, byte[] parentContext)
        {
            try
            {
                byte[] capturedCorrelationContext = this.CorrelationContext.Capture();

                CorrelationContext localCorrelationcontext = new CorrelationContext(parentContext);

                long newOperationId = localCorrelationcontext.AddOperation();

                string newOperationIdString = newOperationId.GetBase64String();
                if (string.IsNullOrWhiteSpace(operationName))
                {
                    Diag("Attempting to create operation with null name. Resetting to randomized value. m7x/mk3e30M");
                    operationName = "ERR_NO_OPERATION_NAME " + newOperationIdString;
                }

                string cc = localCorrelationcontext.ToString();

                this.CorrelationContext = localCorrelationcontext;

                IOperation createdOperation = new ConsoleOperation(this, operationName, newOperationIdString, cc, capturedCorrelationContext);

                return createdOperation;
            }
            catch (Exception ex)
            {
                DiagnosticTrace.Instance.Error("An unexpected error occurred when attempting to create an operation", ex, "cd11de1d-c4b6-406c-937e-37bc85eb4370");

                return new NullOperation();
            }
        }

        public bool Event(string name, Dictionary<string, string> data = null)
        {
            try
            {
                this.Trace(EventSeverity.Event, name, "event", data, string.Empty, string.Empty, -1);
                return true;
            }
            catch (Exception ex)
            {
                DiagnosticTrace.Instance.Error("An unexpected error occurred when attempting emit an event", ex, "0df8b1e6-38b5-4790-ad92-6f5e7da9ce3a");
                return false;
            }
        }


        public bool Metric(string name, double value = 1, Dictionary<string, string> data = null)
        {
            try
            {
                this.Trace(EventSeverity.Metric, $"{name}:{value}", "metric", data, string.Empty, string.Empty, -1);
                return true;
            }
            catch (Exception ex)
            {
                DiagnosticTrace.Instance.Error("An unexpected error occurred when attempting emit a metric", ex, "70280d83-876d-4cd8-86ff-95e639e63b94");
                return false;
            }
        }


        public bool Trace(EventSeverity eventSeverity, string message, Exception exception, string codePoint = null, Dictionary<string, string> data = null, [CallerMemberName] string callerMemberName = "", [CallerFilePath] string callerFilePath = "", [CallerLineNumber] int callerLineNumber = -1)
        {
            // safe bridge
            return this.Trace(eventSeverity, message, codePoint, data.SafeCombine(exception), callerMemberName, callerFilePath, callerLineNumber);
        }

        public bool Trace(EventSeverity eventSeverity, string message, string codePoint = null, Dictionary<string, string> data = null, [CallerMemberName] string callerMemberName = "", [CallerFilePath] string callerFilePath = "", [CallerLineNumber] int callerLineNumber = -1)
        {
            try
            {
                DateTime eventTime = DateTime.UtcNow;

                ConsoleEvent evt = new ConsoleEvent()
                {
                    CallerFilePath = callerFilePath,
                    CallerLineNumber = callerLineNumber,
                    CallerMemberName = callerMemberName,
                    CodePoint = codePoint,
                    Data = data,
                    EventSeverity = eventSeverity,
                    EventTime = eventTime,
                    Message = message,
                    CorrelationContext = this.CorrelationContext.ToString()
                };

                this.eventQueue.Enqueue(evt);
                this.OnEventQueued(evt);



                return true;
            }
            catch (Exception ex)
            {
                DiagnosticTrace.Instance.Error("An unexpected exception has occured when attemtping to enqueue a log event, the event has been lost. ", ex, "DSA7WAaQrUM");
                return false;
            }
        }

        internal virtual void OnEventQueued(ConsoleEvent ce)
        {
            Diag($"EVENT QUEUED {ce.EventSeverity.ToString()}/{ce.Message}/{ce.CodePoint ?? "ncp"}");
        }

        private async Task QueueJob(CancellationToken cancellationToken)
        {
            while (cancellationToken.IsCancellationRequested == false)
            {
                if (this.eventQueue.IsEmpty)
                {
                    await Task.Delay(100);
                }
                else
                {
                    if (this.eventQueue.TryDequeue(out ConsoleEvent evt))
                    {
                        this.TraceToConsole(evt.EventSeverity, evt.Message, evt.CodePoint, evt.Data, evt.CallerMemberName, evt.CallerFilePath, evt.CallerLineNumber, evt.EventTime, evt.CorrelationContext);
                    }
                }
            }
        }

        private void TraceToConsole(EventSeverity eventSeverity, string message, string codePoint, Dictionary<string, string> data, string callerMemberName, string callerFilePath, int callerLineNumber, DateTime eventTime, string correlationContext)
        {
            try
            {
                if (data == null || !this.EmitAdditionalData)
                {
                    data = new Dictionary<string, string>();
                }

                if (this.EmitCodePoint)
                {
                    if (codePoint == null)
                    {
                        if (this.NullCodepointAction == EmptyCodepointAction.DoNothing)
                        {
                            // dont add to dictionary as it does not exist.
                        }
                        else
                        {
                            data[nameof(codePoint)] = this.NullCodepointActionDelegate(message, data, callerMemberName, callerFilePath, callerLineNumber);
                        }
                    }
                    else
                    {
                        data[nameof(codePoint)] = codePoint;
                    }
                }

                if (this.EmitCallerMemberName)
                {
                    data[nameof(callerMemberName)] = callerMemberName;
                }

                if (this.EmitCallerFilePath)
                {
                    data[nameof(callerFilePath)] = callerFilePath;
                }

                if (this.EmitCallerLineNumber)
                {
                    data[nameof(callerLineNumber)] = callerLineNumber.ToString();
                }

                if (this.EmitCorrelationContext)
                {
                    data[nameof(CorrelationContext)] = correlationContext;
                }

                // write event
                ConsoleColor foregroundColor = this.GetForegroundColor(eventSeverity);
                ConsoleColor backgroundColor = this.GetBackgroundColor(eventSeverity);

                Console.ResetColor();
                Console.Write('[');
                Write(GetTimeString(eventTime), foregroundColor, backgroundColor);
                Console.Write("][");
                Write(eventSeverity.ToString(), foregroundColor, backgroundColor);
                Console.Write(']');
                WriteLine(message, foregroundColor, backgroundColor);

                string json;
                if (data.Count > 0)
                {
                    json = JsonConvert.SerializeObject(data, Formatting.Indented);
                    Write("Additional Data: ", foregroundColor, backgroundColor);
                    WriteLine(json, ConsoleColor.DarkGray, ConsoleColor.Black);
                    Console.WriteLine();
                }
                else
                {
                    json = null;
                }

                this.TraceDiagnostic(eventSeverity, message, codePoint, json);
            }
            catch (Exception ex)
            {
                DiagnosticTrace.Instance.Error("An unexpected exception has occurred when attempting to send data to the console. See the Details for more information.", ex, "sOe8HdhzzkM");
            }
        }

        private static void Write(string message, ConsoleColor foreground, ConsoleColor background)
        {
            Console.ForegroundColor = foreground;
            Console.BackgroundColor = background;
            Console.Write(message);
            Console.ResetColor();
        }

        private static void WriteLine(string message, ConsoleColor foreground, ConsoleColor background)
        {
            Console.ForegroundColor = foreground;
            Console.BackgroundColor = background;
            Console.WriteLine(message);
            Console.ResetColor();
        }

        public void Dispose()
        {
            this.cancellationTokenSource.CancelAfter(300);

            Thread.Sleep(300);
        }

        /// <summary>
        /// Repply the origin context captured as part of an operation.
        /// </summary>
        /// <param name="capturedContext">the context to reapply.</param>
        void ICorrelatedProvider.ReapplyOriginContext(byte[] capturedContext)
        {
            this.CorrelationContext = new CorrelationContext(capturedContext);
        }
    }
}
