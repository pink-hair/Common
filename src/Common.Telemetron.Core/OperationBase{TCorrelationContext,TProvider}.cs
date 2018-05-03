namespace Polytech.Common.Telemetron
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Text;

    public abstract class OperationBase<TCorrelationContext, TProvider> : IOperation
        where TProvider : IOperationProvider<TCorrelationContext>, ITraceProvider, IMetricProvider, ICorrelatedProvider
    {
        private readonly string operationName;
        private readonly string operationId;
        private readonly string correlationContext;

        private readonly Dictionary<string, string> telemetryData;

        private readonly Stopwatch stopwatch;

        private readonly TProvider provider;

        private OperationResult result;

        private Exception capturedException;

        public OperationBase(TProvider provider, string operationName, string newOperationId, string correlationContext)
        {
            this.operationName = operationName ?? throw new ArgumentNullException(nameof(operationName));
            this.operationId = newOperationId ?? throw new ArgumentNullException(nameof(newOperationId));
            this.correlationContext = correlationContext ?? throw new ArgumentNullException(nameof(correlationContext));

            this.telemetryData = new Dictionary<string, string>();

            this.provider = provider;

            this.telemetryData[nameof(this.operationName)] = this.operationName;
            this.telemetryData[nameof(this.operationId)] = this.operationId;
            this.telemetryData[nameof(this.correlationContext)] = this.correlationContext;

            this.stopwatch = Stopwatch.StartNew();

            this.EmitOperationStart();
        }

        protected virtual Stopwatch Stopwatch => this.stopwatch;

        protected virtual void EmitOperationStart()
        {
            // do nothing.
        }

        protected virtual void EmitOperationEnd()
        {
            this.stopwatch.Stop();

            this.provider.Metric($"{this.operationName}/metrics/duration", this.stopwatch.ElapsedMilliseconds, this.telemetryData);
            this.provider.Metric($"{this.operationName}/metrics/outcome", (int)this.result, this.telemetryData);
        }

        public virtual IDictionary<string, string> TelemetryData => this.telemetryData;

        public virtual string OperationName => this.operationName;

        public virtual string OperationId => this.operationId;

        public virtual string CorrelationContext => this.correlationContext;

        public OperationResult Result => this.result;

        protected TProvider Provider => this.provider;

        public void Dispose()
        {
            ICorrelationContext ctx = this.provider.CorrelationContext;
            long removed = ctx.RemoveOperation();
            this.provider.CorrelationContext = ctx;
           
            this.EmitOperationEnd();
        }

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
