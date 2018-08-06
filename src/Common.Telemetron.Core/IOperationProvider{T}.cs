namespace PinkHair.Common.Telemetron
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using PinkHair.Common.Telemetron.Configuration;

    /// <summary>
    /// A provider that supports the concept of operations. 
    /// </summary>
    /// <typeparam name="T">The type used to restore context manually.</typeparam>
    public interface IOperationProvider<T> : IOperationProvider
    {
        /// <summary>
        /// Creates an operation with a correlation context imported from outside of a boundary where the correlation context will follow.
        /// </summary>
        /// <param name="operationName">The name of the operation to cretae.</param>
        /// <param name="correlationContext">The data representation of the correlation context state.</param>
        /// <returns>An operation handle.</returns>
        IOperation CreateOperation(string operationName, T correlationContext);
    }
}
