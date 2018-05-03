namespace Polytech.Common.Telemetron
{
    /// <summary>
    /// A console Operation. This <see cref="IOperation"/> is derived from console logging.
    /// </summary>
    public class ConsoleOperation : OperationBase<byte[], ConsoleTelemetron>, IOperation
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ConsoleOperation"/> class.
        /// </summary>
        /// <param name="provider">The provider that will emit logging.</param>
        /// <param name="operationName">The name of the operation.</param>
        /// <param name="newOperationId">The new Id of the operation.</param>
        /// <param name="correlationContext">The current correlation context.</param>
        internal ConsoleOperation(ConsoleTelemetron provider, string operationName, string newOperationId, string correlationContext)
            : base(provider, operationName, newOperationId, correlationContext)
        {
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
