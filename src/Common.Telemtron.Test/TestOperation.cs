namespace Polytech.Common.Telemetron
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using Polytech.Common.Telemetron;


    public class TestOperation : OperationBase<byte[], TestTelemetron>, IOperation
    {
        internal TestOperation(TestTelemetron provider, string operationName, string newOperationId, string correlationContext)
            : base(provider, operationName, newOperationId, correlationContext)
        {
        }

        protected override void EmitOperationEnd()
        {
            base.EmitOperationEnd();

            this.Provider.Info($"The operation {this.OperationName} with id {this.OperationId} has completed");
        }
    }
}
