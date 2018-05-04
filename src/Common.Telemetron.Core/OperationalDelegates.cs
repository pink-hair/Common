namespace Polytech.Common.Telemetron
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Threading.Tasks;

    public delegate void RunOperationDelegate(IOperation operation);

    public delegate T RunOperationDelegate<T>(IOperation operation);

    public delegate Task RunOperationAsyncDelegate(IOperation operation);

    public delegate Task<T> RunOperationAsyncDelegate<T>(IOperation operation);
}
