namespace PinkHair.Common.Telemetron
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    /// <summary>
    /// An interface for a non strongly typed operation.
    /// </summary>
    public interface IOperation : IOperationBase
    {
        /// <summary>
        /// Gets the telemetry data;
        /// </summary>
        IDictionary<string, string> TelemetryData { get; }
    }

    /// <summary>
    /// A base interface for operations.
    /// </summary>
    public interface IOperationBase : IDisposable
    {
        /// <summary>
        /// Gets the current known outcome of the operation.
        /// </summary>
        OperationResult Result { get; }

        /// <summary>
        /// Gets the name of the operation.
        /// </summary>
        string OperationName { get; }

        /// <summary>
        /// Gets the id of the Operation.
        /// </summary>
        string OperationId { get; }

        /// <summary>
        /// Gets the correlation context for the operation.
        /// </summary>
        string CorrelationContext { get; }

        /// <summary>
        /// Sets the result of the operation.
        /// </summary>
        /// <param name="result">The result of the operation.</param>
        /// <param name="soft">Only set the result of the operation if it is not already set.</param>
        void SetOperationResult(OperationResult result, bool soft = false);

        /// <summary>
        /// Set the result of the operation as the result of an exception.
        /// </summary>
        /// <param name="ex">The exception.</param>
        void SetOperationResult(Exception ex);
    }
}
