using PinkHair.Common.Telemetron.Configuration;
using System;
using System.Collections.Generic;

namespace PinkHair.Common.Telemetron
{
    using static PinkHair.Common.Telemetron.Diagnostics.DiagnosticTrace;

    public class MultiTelemetron<T> : CorrelatedProviderBase,
        IOperationProvider<T>
    {
        private HashSet<ITraceProvider> traceProviders;
        private HashSet<IMetricProvider> metricProviders;
        private HashSet<IEventProvider> eventProviders;
        private Dictionary<Guid, IOperationProvider<T>> operationProviders;

        public MultiTelemetron(IMultiTelemetronConfiguration configuration)
            : base(configuration)
        {
            this.traceProviders = new HashSet<ITraceProvider>();
            this.metricProviders = new HashSet<IMetricProvider>();
            this.eventProviders = new HashSet<IEventProvider>();
            this.operationProviders = new Dictionary<Guid, IOperationProvider<T>>();
        }

        public void AddTraceProvider(ITraceProvider provider)
        {
            this.traceProviders.Add(provider);
        }

        public void AddMetricProvider(IMetricProvider provider)
        {
            this.metricProviders.Add(provider);
        }

        public void AddEventProvider(IEventProvider provider)
        {
            this.eventProviders.Add(provider);
        }

        public void AddOperationProvider(IOperationProvider<T> provider)
        {
            this.operationProviders[provider.RuntimeId] = provider;
        }

        public IOperation CreateOperation(string operationName)
        {
            try
            {
                // allocate operations per
                Dictionary<Guid, IOperation> createdOperations = new Dictionary<Guid, IOperation>();

                foreach (KeyValuePair<Guid, IOperationProvider<T>> operationProvider in this.operationProviders)
                {
                    IOperation createdOperation = operationProvider.Value.CreateOperation(operationName);
                    createdOperations[operationProvider.Key] = createdOperation;
                }

                MultiOperation multiOperation = new MultiOperation(createdOperations);

                return multiOperation;
            }
            catch (Exception ex)
            when (Filter("An unexpected exception occurred when creatign a multiOperation", ex))
            {
                return new NullOperation();
            }
        }

        public IOperation CreateOperation(string operationName, T correlationContext)
        {
            try
            {
                // allocate operations per
                Dictionary<Guid, IOperation> createdOperations = new Dictionary<Guid, IOperation>();

                foreach (KeyValuePair<Guid, IOperationProvider<T>> operationProvider in this.operationProviders)
                {
                    IOperation createdOperation = operationProvider.Value.CreateOperation(operationName, correlationContext);
                    createdOperations[operationProvider.Key] = createdOperation;
                }

                MultiOperation multiOperation = new MultiOperation(createdOperations);

                return multiOperation;
            }
            catch (Exception ex)
            when (Filter("An unexpected exception occurred when creatign a multiOperation", ex))
            {
                return new NullOperation();
            }
        }
    }
}
