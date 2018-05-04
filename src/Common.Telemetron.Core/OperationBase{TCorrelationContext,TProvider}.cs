namespace Polytech.Common.Telemetron
{
    using Polytech.Common.Telemetron.Configuration;
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Text;

    using static Polytech.Common.Telemetron.Diagnostics.DiagnosticTrace;

    public abstract class OperationBase<TCorrelationContext, TProvider> : IOperation
        where TProvider : IOperationProvider<TCorrelationContext>, ITraceProvider, IMetricProvider, ICorrelatedProvider
    {
        private byte[] capturedContext;
        private readonly string operationName;
        private readonly string operationId;
        private readonly string correlationContext;
        private readonly string diagnosticOperationName;

        private readonly Dictionary<string, string> telemetryData;

        private readonly Stopwatch stopwatch;

        private readonly TProvider provider;

        private OperationResult result;

        private Exception capturedException;

        private bool emitOperationMetricsDirectly;

        public OperationBase(TProvider provider, string operationName, string newOperationId, string correlationContext)
        {
            if (string.IsNullOrWhiteSpace(operationName))
            {
                throw new ArgumentNullException(nameof(operationName));
            }

            if (string.IsNullOrWhiteSpace(newOperationId))
            {
                throw new ArgumentNullException(nameof(newOperationId));
            }

            this.correlationContext = correlationContext ?? throw new ArgumentNullException(nameof(correlationContext));

            this.operationId = newOperationId;
            this.operationName = operationName;

            // for tracing, and england
            this.diagnosticOperationName = string.Concat(this.operationId, '/', this.operationName);

            this.telemetryData = new Dictionary<string, string>();

            this.provider = provider;
            this.emitOperationMetricsDirectly = provider.OperationConfiguration.EmitOperationMetrics;

            this.telemetryData[nameof(this.operationName)] = this.operationName;
            this.telemetryData[nameof(this.operationId)] = this.operationId;
            this.telemetryData[nameof(this.correlationContext)] = this.correlationContext;

            this.stopwatch = Stopwatch.StartNew();

            this.EmitOperationStart();
            Diag($"Operation created '{this.diagnosticOperationName}");


        }

        /// <summary>
        /// Gets the diagnostic Operation Name used in tracing.
        /// </summary>
        protected internal string DiagnosticOperationName => this.diagnosticOperationName;

        /// <summary>
        /// Gets the stopwatch object started when the operation was created.
        /// </summary>
        protected virtual Stopwatch Stopwatch => this.stopwatch;

        protected virtual void EmitOperationStart()
        {
            // base do nothing.
        }

        protected virtual void EmitOperationEnd()
        {
            this.stopwatch.Stop();

            if (this.telemetryData.Count > 0)
            {
                Diag($"Operation contains telemetry Data. ## {nameof(this.TelemetryData)}.{nameof(this.TelemetryData.Count)} {this.telemetryData.Count} OPB/DRz430M");
            }

            if (this.emitOperationMetricsDirectly)
            {
                Diag($"Emitting Operation Complete metrics for {this.diagnosticOperationName}");
                this.provider.Metric($"{this.operationName}/metrics/duration", this.stopwatch.ElapsedMilliseconds, this.telemetryData);
                this.provider.Metric($"{this.operationName}/metrics/outcome", (int)this.result, this.telemetryData);
            }
        }

        /// <summary>
        /// Gets the telemetry data that is collected for this operation.
        /// </summary>
        public virtual IDictionary<string, string> TelemetryData => this.telemetryData;

        /// <summary>
        /// Gets the name of the operation.
        /// </summary>
        public virtual string OperationName => this.operationName;

        /// <summary>
        /// Gets the id of the operation.
        /// </summary>
        public virtual string OperationId => this.operationId;

        /// <summary>
        /// Gets the correlation context 
        /// </summary>
        public virtual string CorrelationContext => this.correlationContext;

        /// <summary>
        /// Gets the result of the operation.
        /// </summary>
        public OperationResult Result => this.result;

        /// <summary>
        /// Gets the provider that created the operation.
        /// </summary>
        protected TProvider Provider => this.provider;

        /// <summary>
        /// Disposes of this operation. The operation itself does not implement any unmanaged resources, this simply enables the <see langword="using" /> syntax.
        /// </summary>
        /// <remarks>
        /// This <see cref="OperationBase{TCorrelationContext, TProvider}"/> does not implement any unmanaged resources to dispose of, but it is not to say that an implementer will not. Please be sure to Dispose.</remarks>
        public void Dispose()
        {
            ICorrelationContext ctx = this.provider.CorrelationContext;
            long removed = ctx.RemoveOperation();
            this.provider.CorrelationContext = ctx;

            this.EmitOperationEnd();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="result"></param>
        /// <param name="soft"></param>
        public void SetOperationResult(OperationResult result, bool soft = false)
        {
            if (soft && this.result == OperationResult.NotSet)
            {
                this.result = result;
            }
            else
            {
                this.result = result;
            }
        }

        public void SetOperationResult(Exception ex)
        {
            this.result = OperationResult.Exceptional;
            this.capturedException = ex;
        }
    }
}
