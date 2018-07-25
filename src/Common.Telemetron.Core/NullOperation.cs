using System;
using System.Collections.Generic;
using System.Text;

namespace PinkHair.Common.Telemetron
{
    public class NullOperation : IOperation
    {
        public IDictionary<string, string> TelemetryData => new Dictionary<string, string>();

        public OperationResult Result => OperationResult.Success;

        public string OperationName => string.Empty;

        public string OperationId => string.Empty;

        public string CorrelationContext => string.Empty;

        public void Dispose()
        {
            // do nothing
        }

        public void SetOperationResult(OperationResult result, bool soft = false)
        {
            // do nothing
        }

        public void SetOperationResult(Exception ex)
        {
            // do nothing
        }
    }
}
