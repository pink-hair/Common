namespace PinkHair.Common.Telemetron
{
    using System;
    using PinkHair.Common.Telemetron.Configuration;

    /// <summary>
    /// Interface for Telemetrons that provide Operations.
    /// </summary>
    public interface IOperationProvider
    {
        /// <summary>
        /// Gets the runtime Id of this provider. Used to correlate multi-provider operations.
        /// </summary>
        Guid RuntimeId { get; }

        /// <summary>
        /// Gets the current operation configuration from the underlying provider.
        /// </summary>
        IOperationConfiguration OperationConfiguration { get; }

        /// <summary>
        /// Starts a new operation using the underlying provider.
        /// </summary>
        /// <param name="operationName">The name of the provider to instantiate.</param>
        /// <returns>A handle to the operation.</returns>
        IOperation CreateOperation(string operationName);
    }
}
