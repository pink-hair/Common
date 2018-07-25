﻿namespace PinkHair.Common.Telemetron
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using PinkHair.Common.Telemetron.Configuration;

    /// <summary>
    /// A provider that supports the concept of operations. 
    /// </summary>
    /// <typeparam name="T">The type used to restore context manually.</typeparam>
    public interface IOperationProvider<T>
    {
        Guid RuntimeId { get; }

        IOperation CreateOperation(string operationName);

        IOperation CreateOperation(string operationName, T correlationContext);

        IOperationConfiguration OperationConfiguration { get; }
    }
}
