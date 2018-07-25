namespace PinkHair.Common.Telemetron
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using static PinkHair.Common.Telemetron.Diagnostics.DiagnosticTrace;

    /// <summary>
    /// A multi targeted operation. Useful when using two providers.
    /// </summary>
    public class MultiOperation : IOperation
    {
        private IDictionary<Guid, IOperation> operations;
        private MultiDictionary telemetryData;
        private IOperation first;

        /// <summary>
        /// Initializes a new instance of the <see cref="MultiOperation"/> class.
        /// </summary>
        /// <param name="operations">The already created operations that will be tracked with this operation.</param>
        public MultiOperation(IDictionary<Guid, IOperation> operations)
        {
            if (operations == null)
            {
                throw new ArgumentNullException(nameof(operations));
            }

            if (operations.Count == 0)
            {
                throw new ArgumentException("You must provide one or more operations to track", nameof(operations));
            }

            this.operations = operations;
            this.telemetryData = new MultiDictionary(this);
            this.first = this.operations.FirstOrDefault().Value;
        }

        /// <summary>
        /// Gets the currently tracked operations.
        /// </summary>
        public IDictionary<Guid, IOperation> Operations
        {
            get => this.operations;
            internal set => this.operations = value;
        }

        /// <summary>
        /// Gets the telemetry data;
        /// </summary>
        public IDictionary<string, string> TelemetryData => this.telemetryData;

        /// <summary>
        /// Gets the current known outcome of the operation.
        /// </summary>
        public OperationResult Result => this.first.Result;

        /// <summary>
        /// Gets the name of the operation.
        /// </summary>
        public string OperationName => this.first.OperationName;

        /// <summary>
        /// Gets the id of the Operation.
        /// </summary>
        public string OperationId => this.first.OperationId;

        /// <summary>
        /// Gets the correlation context for the operation.
        /// </summary>
        public string CorrelationContext => this.first.CorrelationContext;

        /// <summary>
        /// Dispose of this operation and all children.
        /// </summary>
        public void Dispose()
        {
            foreach (IOperation operation in this.operations.Values)
            {
                try
                {
                    operation.Dispose();
                }
                catch (Exception ex)
                when (Filter($"An unexpected expected occurred when attempting to dispose of an operation.", ex))
                {
                }
            }
        }

        /// <summary>
        /// Sets the result of the operation.
        /// </summary>
        /// <param name="result">The result of the operation.</param>
        /// <param name="soft">Only set the result of the operation if it is not already set.</param>
        public void SetOperationResult(OperationResult result, bool soft = false)
        {
            foreach (IOperation operation in this.operations.Values)
            {
                try
                {
                    operation.SetOperationResult(result, soft);
                }
                catch (Exception ex)
                when (Filter($"An unexpected expected occurred when attempting to set the result of an operation.", ex))
                {
                }
            }
        }

        /// <summary>
        /// Set the result of the operation as the result of an exception.
        /// </summary>
        /// <param name="ex">The exception.</param>
        public void SetOperationResult(Exception ex)
        {
            foreach (IOperation operation in this.operations.Values)
            {
                try
                {
                    operation.SetOperationResult(ex);
                }
                catch (Exception iex)
                when (Filter($"An unexpected expected occurred when attempting to set the result of an operation.", iex))
                {
                }
            }
        }
    }
}
