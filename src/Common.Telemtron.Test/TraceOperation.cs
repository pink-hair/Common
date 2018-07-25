namespace PinkHair.Common.Telemetron
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using PinkHair.Common.Telemetron;

    /// <summary>
    /// An operation class used by <see cref="TraceTelemetron"/>
    /// </summary>
    public class TraceOperation : OperationBase<byte[], TraceTelemetron>, IOperation
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TraceOperation"/> class.
        /// </summary>
        /// <param name="provider"></param>
        /// <param name="operationName"></param>
        /// <param name="newOperationId"></param>
        /// <param name="correlationContext"></param>
        internal TraceOperation(TraceTelemetron provider, string operationName, string newOperationId, string correlationContext)
            : base(provider, operationName, newOperationId, correlationContext)
        {
        }

        protected override void EmitOperationStart()
        {
            System.Diagnostics.Trace.Indent();

            base.EmitOperationStart();
        }

        protected override void EmitOperationEnd()
        {
            base.EmitOperationEnd();

            this.Provider.Info($"The operation {this.OperationName} with id {this.OperationId} has completed");

            global::System.Diagnostics.Trace.Unindent();
        }
    }
}
