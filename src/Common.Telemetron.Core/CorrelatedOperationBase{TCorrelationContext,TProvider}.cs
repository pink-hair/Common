namespace PinkHair.Common.Telemetron
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using PinkHair.Common.Telemetron.Configuration;

    /// <summary>
    /// A correlated Operation.
    /// </summary>
    /// <typeparam name="TCorrelationContext">The type of correlation context that is used.</typeparam>
    /// <typeparam name="TProvider">The type of provider generating this operation.</typeparam>
    public class CorrelatedOperationBase<TCorrelationContext, TProvider>
        : OperationBase<TCorrelationContext, TProvider>, IOperation
        where TProvider : IOperationProvider<TCorrelationContext>, ITraceProvider, IMetricProvider, ICorrelatedProvider
    {
        private byte[] capturedOriginContext;

        /// <summary>
        /// Initializes a new instance of the <see cref="CorrelatedOperationBase{TCorrelationContext, TProvider}"/> class.
        /// </summary>
        /// <param name="provider">The operation provider. Used to hook additional telemetry and emit.</param>
        /// <param name="operationName">The name of the created operation. Be purposeful.</param>
        /// <param name="newOperationId">The id of the newly created Operation.</param>
        /// <param name="correlationContext">The captured human readable correlation context.</param>
        /// <param name="capturedOriginContext">The captured origin context to reset.</param>
        public CorrelatedOperationBase(
            TProvider provider,
            string operationName,
            string newOperationId,
            string correlationContext,
            byte[] capturedOriginContext)
            : base(
                  provider,
                  operationName,
                  newOperationId,
                  correlationContext)
        {
            this.capturedOriginContext = capturedOriginContext;
        }

        /// <summary>
        /// Activities to perform after the operation has completed.
        /// </summary>
        protected override void EmitOperationEnd()
        {
            base.EmitOperationEnd();

            if (this.capturedOriginContext != null)
            {
                this.Provider.ReapplyOriginContext(this.capturedOriginContext);
            }
        }
    }
}
