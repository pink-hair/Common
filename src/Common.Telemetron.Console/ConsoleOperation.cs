namespace Polytech.Common.Telemetron
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using Polytech.Common.Telemetron;

    public class ConsoleOperation : OperationBase<byte[], ConsoleTelemetron>, IOperation
    {
        internal ConsoleOperation(ConsoleTelemetron provider, string operationName, string newOperationId, string correlationContext)
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
