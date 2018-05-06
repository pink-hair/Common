namespace Polytech.Common.Telemetron
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics.CodeAnalysis;
    using System.Text;
    using Microsoft.ApplicationInsights.Channel;
    using Microsoft.ApplicationInsights.DataContracts;
    using Microsoft.ApplicationInsights.Extensibility.Implementation;
    using Polytech.Ide;

    public class ApplicationInsightsOperationTelemetry : OperationTelemetry, ITelemetry, ISupportProperties, ISupportMetrics, ISupportSampling
    {
        private readonly Dictionary<string, double> metrics;
        private string id;
        private string name;
        private bool? success;
        private TimeSpan duration;
        private string sequence;
        private DateTimeOffset timestamp;
        private TelemetryContext telemetryContext;
        private double? samplingPercentage;

        /// <summary>
        /// Initializes a new instance of the <see cref="ApplicationInsightsOperationTelemetry"/> class.
        /// </summary>
        /// <param name="operationName">The name of the operation.</param>
        /// <param name="operationId">The Id of the newly created operation.</param>
        /// <param name="correlationContext">The correlation Context for the operation.</param>
        public ApplicationInsightsOperationTelemetry(string operationName, string operationId, string correlationContext)
        {
            if (string.IsNullOrEmpty(operationName))
            {
                throw new ArgumentNullException(nameof(operationName));
            }

            if (string.IsNullOrEmpty(operationId))
            {
                throw new ArgumentNullException(nameof(operationId));
            }

            if (string.IsNullOrEmpty(correlationContext))
            {
                throw new ArgumentNullException(nameof(correlationContext));
            }

            this.metrics = new Dictionary<string, double>();
            this.telemetryContext = new TelemetryContext();

            this.name = operationName;
            this.id = operationId;
            this.CorrelationContext = correlationContext;
        }

        /// <summary>
        /// Gets or sets the Id of the event or operation.
        /// </summary>
        public override string Id
        {
            get => this.id;
            set => this.id = value;
        }

        /// <summary>
        /// Gets or sets the name of the operation.
        /// </summary>
        public override string Name
        {
            get => this.name;
            set => this.name = value;
        }

        /// <summary>
        /// Gets or sets a value indicating success.
        /// </summary>
        public override bool? Success
        {
            get => this.success;
            set => this.success = value;
        }

        /// <summary>
        /// Gets or sets a value indicating the duration of the operation.
        /// </summary>
        public override TimeSpan Duration
        {
            get => this.duration;
            set => this.duration = value;
        }

        public string CorrelationContext
        {
            get
            {
                if (this.Properties.TryGetValue(nameof(this.CorrelationContext), out string correlationContext))
                {
                    return correlationContext;
                }
                else
                {
                    return null;
                }
            }

            set
            {
                this.Properties[nameof(this.CorrelationContext)] = value;
            }
        }

        /// <summary>
        /// Gets or sets any metrics associated with the operation.
        /// </summary>
        public override IDictionary<string, double> Metrics => this.metrics;

        /// <summary>
        /// Gets or sets any properties associated with the operation.
        /// </summary>
        public override IDictionary<string, string> Properties => this.Context.Properties;

        /// <summary>
        /// Gets or sets the Timestamp of when the telemetry event occurred.
        /// </summary>
        public override DateTimeOffset Timestamp
        {
            get => this.timestamp;
            set => this.timestamp = value;
        }

        /// <summary>
        /// Gets the telemetry context for this event.
        /// </summary>
        public override TelemetryContext Context => this.telemetryContext;

        /// <summary>
        /// Gets or sets the sequence for this event.
        /// </summary>
        [ExcludeFromCodeCoverage]
        [Justification(nameof(ExcludeFromCodeCoverageAttribute), "This property is used by AI Internally.")]
        public override string Sequence
        {
            get => this.sequence;
            set => this.sequence = value;
        }

        [ExcludeFromCodeCoverage]
        [Justification(nameof(ExcludeFromCodeCoverageAttribute), "This property is used by AI Internally.")]
        public double? SamplingPercentage
        {
            get => this.samplingPercentage;
            set => this.samplingPercentage = value;
        }

        public override ITelemetry DeepClone()
        {
            ApplicationInsightsOperationTelemetry t = new ApplicationInsightsOperationTelemetry(this.name, this.id, this.CorrelationContext)
            {
                duration = this.duration,
                sequence = this.sequence,
                success = this.success,
                timestamp = this.timestamp
            };

            foreach (KeyValuePair<string, double> metric in this.metrics)
            {
                t.metrics[metric.Key] = metric.Value;
            }

            foreach (KeyValuePair<string, string> property in this.Properties)
            {
                t.Properties[property.Key] = property.Value;
            }

            return t;
        }
    }
}
