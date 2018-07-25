namespace PinkHair.Common.Telemetron
{
    using System.Threading;
    using Common.Telemetron.Configuration;

    /// <summary>
    /// Base class for a provider that does not implement correlation by itself. 
    /// </summary>
    public abstract class CorrelatedProviderBase : ProviderBase
    {
        private AsyncLocal<ICorrelationContext> correlationContext;

        /// <summary>
        /// Initializes a new instance of the <see cref="CorrelatedProviderBase"/> class.
        /// </summary>
        /// <param name="configuration">The inherited configuration from the implementing class to pass to the base class.</param>
        protected CorrelatedProviderBase(ITelemetronConfigurationBase configuration) 
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
