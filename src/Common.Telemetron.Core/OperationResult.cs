namespace Polytech.Common.Telemetron
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    /// <summary>
    /// The result of an operation.
    /// </summary>
    public enum OperationResult
    {
        NotSet = 0,
        Failed = 1,
        Success = 2,
        Exceptional = 3,
    }
}
