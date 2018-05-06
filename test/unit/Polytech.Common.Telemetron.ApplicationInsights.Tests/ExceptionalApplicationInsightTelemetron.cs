using System;
using System.Collections.Generic;
using System.Text;
using Polytech.Common.Telemetron.Configuration;
using Telemetron.Core.Unit.Tests;

namespace Polytech.Common.Telemetron.ApplicationInsights.Tests
{
    internal class ExceptionalApplicationInsightTelemetron : ApplicationInsightsTelemetron, ITelemetronProvider<byte[]>
    {
        public ExceptionalApplicationInsightTelemetron(IApplicationInsightsTelemetronConfiguration configuration) : base(configuration)
        {
        }

        protected override IOperation CreateOperationInternalWithContextSet(string operationName, string newOperationIdString, string cc)
        {
            throw new FakeException();
        }
    }
}
