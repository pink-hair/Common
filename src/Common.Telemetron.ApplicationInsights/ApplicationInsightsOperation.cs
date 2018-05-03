using System;
using System.Collections.Generic;
using System.Text;

namespace Polytech.Common.Telemetron
{
    using Microsoft.ApplicationInsights;
    using Microsoft.ApplicationInsights.Extensibility;
    public class ApplicationInsightsOperation : OperationBase<byte[], ApplicationInsightsTelemetron>, IOperation, IDisposable
    {
        private readonly ApplicationInsightsOperationTelemetry aiTelemetry;
        private IOperationHolder<ApplicationInsightsOperationTelemetry> operationHandle;

        public ApplicationInsightsOperation(ApplicationInsightsTelemetron provider, string operationName, string newOperationId, string correlationContext)
            : base(provider, operationName, newOperationId, correlationContext)
        {
            this.aiTelemetry = new ApplicationInsightsOperationTelemetry(operationName, newOperationId, correlationContext);
            this.aiTelemetry.Timestamp = DateTimeOffset.UtcNow;

            this.aiTelemetry.SetOperationInfo(provider);
            this.aiTelemetry.CorrelationContext = correlationContext;

            this.operationHandle = this.Provider.TelemetryClient.StartOperation(this.aiTelemetry);
        }

        /// <summary>
        /// Gets the correlation Context property for the telemetry
        /// </summary>
        public override string CorrelationContext => this.aiTelemetry.CorrelationContext;

        /// <summary>
        /// Gets the current Id of the operation.
        /// </summary>
        public override string OperationId => this.aiTelemetry.Id;

        /// <summary>
        /// Gets the current name of the operation.
        /// </summary>
        public override string OperationName => this.aiTelemetry.Name;

        /// <summary>
        /// Gets the current telemetry properties.
        /// </summary>
        public override IDictionary<string, string> TelemetryData => this.aiTelemetry.Properties;

        /// <summary>
        /// Stuff to do when the operation starts.
        /// </summary>
        protected override void EmitOperationStart()
        {
            // do nothing
        }

        /// <summary>
        /// Stuff to do whent the operation ends.
        /// </summary>
        protected override void EmitOperationEnd()
        {
            this.Stopwatch.Stop();
            this.aiTelemetry.Duration = this.Stopwatch.Elapsed;

            if (this.Result == OperationResult.Success)
            {
                this.aiTelemetry.Success = true;
            }
            else
            {
                this.aiTelemetry.Success = false;
            }

            this.Provider.TelemetryClient.StopOperation(this.operationHandle);
        }
    }
}
