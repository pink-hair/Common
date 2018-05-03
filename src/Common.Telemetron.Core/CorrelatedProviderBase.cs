namespace Polytech.Common.Telemetron
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using System.Threading;
    using Common.Telemetron.Configuration;

    /// <summary>
    /// Base class for a provider that does not implement correlation by itself. 
    /// </summary>
    public abstract class CorrelatedProviderBase : ProviderBase
    {
        private AsyncLocal<ICorrelationContext> correlationContext;

        public CorrelatedProviderBase(ITelemetronConfigurationBase configuration) 
            : base(configuration)
        {
            this.correlationContext = new AsyncLocal<ICorrelationContext>();
        }

        /// <summary>
        /// Gets or sets the correlaation context for the provider. 
        /// Please remember that structs are immutable and copy on access.
        /// So please copy the struct to a local variable before attempting access.
        /// </summary>
        public ICorrelationContext CorrelationContext
        {
            get => this.correlationContext.Value;
            set => this.correlationContext.Value = value;
        }
    }
}
