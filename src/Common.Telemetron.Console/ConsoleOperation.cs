namespace Polytech.Common.Telemetron
{
    using Polytech.Common.Telemetron.Configuration;

    /// <summary>
    /// A console Operation. This <see cref="IOperation"/> is derived from console logging.
    /// </summary>
    public class ConsoleOperation : CorrelatedOperationBase<byte[], ConsoleTelemetron>, IOperation
    {
        private byte[] capturedContext;

        /// <summary>
        /// Initializes a new instance of the <see cref="ConsoleOperation"/> class.
        /// </summary>
        /// <param name="provider">The provider that will emit logging.</param>
        /// <param name="operationName">The name of the operation.</param>
        /// <param name="newOperationId">The new Id of the operation.</param>
        /// <param name="correlationContext">The current correlation context.</param>
        /// <param name="capturedContext">The captured context to reinstate when the operation has completed.</param>
        public ConsoleOperation(ConsoleTelemetron provider, string operationName, string newOperationId, string correlationContext, byte[] capturedContext = null)
            : base(provider, operationName, newOperationId, correlationContext, capturedContext)
        {
            this.capturedContext = capturedContext;
        }

        /// <summary>
        /// The additional tasks to perform at the end of an operation's lifetime.
        /// </summary>
        protected override void EmitOperationEnd()
        {
            base.EmitOperationEnd();

            this.Provider.Info($"The operation {this.OperationName} with id {this.OperationId} has completed");
        }
    }
}
