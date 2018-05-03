using System;
namespace Polytech.Common.Telemetron
{
    using Common.Telemetron.Configuration;
    using System.Collections.Generic;

    public abstract class ProviderBase
    {
        private Guid runtimeId;
        private EmptyCodepointAction nullCodepointAction;
        private TelemetronNullCodePointDelegte nullCodepointActionDelegate;

        private bool emitCodePoint;

        private bool emitCallerMemberName;

        private bool emitCallerFilePath;

        private bool emitCallerLineNumber;

        private bool emitAdditionalData;

        private bool emitCorrelationContext;

        public ProviderBase(ITelemetronConfigurationBase configuration)
        {
            if (configuration == null)
            {
                throw new ArgumentNullException(nameof(configuration));
            }

            // configuration is not null
            // delegate can be null
            if (configuration.NullCodepointAction == null)
            {
                this.nullCodepointAction = EmptyCodepointAction.DoNothing;
            }
            else
            {
                this.nullCodepointAction = EmptyCodepointAction.UseDelegateMethod;
                this.nullCodepointActionDelegate = configuration.NullCodepointAction;
            }

            // copy config local.
            this.emitCodePoint = configuration.EmitCodePoint;
            this.emitCallerMemberName = configuration.EmitCallerMemberName;
            this.emitCallerFilePath = configuration.EmitCallerFilePath;
            this.emitCallerLineNumber = configuration.EmitCallerLineNumber;
            this.emitAdditionalData = configuration.EmitAdditionalData;
            this.emitCorrelationContext = configuration.EmitCorrelationContext;

            // dont save configuration reference object
            // setup runtime id
            this.runtimeId = Guid.NewGuid();
        }

        public Guid RuntimeId => this.runtimeId;

        protected EmptyCodepointAction NullCodepointAction => this.nullCodepointAction;

        protected bool EmitCodePoint => this.emitCodePoint;

        protected bool EmitCallerMemberName => this.emitCallerMemberName;

        protected bool EmitCallerFilePath => this.emitCallerFilePath;

        protected bool EmitCallerLineNumber => this.emitCallerLineNumber;

        protected bool EmitAdditionalData => this.emitAdditionalData;

        protected bool EmitCorrelationContext => this.emitCorrelationContext;

        protected TelemetronNullCodePointDelegte NullCodepointActionDelegate => this.nullCodepointActionDelegate;

        protected void AddCodepoint(string message, string codePoint, string callerMemberName, string callerFilePath, int callerLineNumber, Dictionary<string, string> props)
        {
            string codepoint;
            if (codePoint == null && this.NullCodepointAction == EmptyCodepointAction.UseDelegateMethod)
            {
                codepoint = this.NullCodepointActionDelegate(message, props, callerMemberName, callerFilePath, callerLineNumber);
            }
            else
            {
                codepoint = codePoint;
            }

            if (this.EmitCodePoint)
            {
                props[nameof(codePoint)] = codepoint;
            }
        }

        protected void AddCallerFilePath(string callerFilePath, Dictionary<string, string> props)
        {
            if (this.EmitCallerFilePath)
            {
                props[nameof(callerFilePath)] = callerFilePath;
            }
        }

        protected void AddCallerLineNumber(int callerLineNumber, Dictionary<string, string> props)
        {
            if (this.EmitCallerLineNumber)
            {
                props[nameof(callerLineNumber)] = callerLineNumber.ToString();
            }
        }

        protected void AddCallerMemberName(string callerMemberName, Dictionary<string, string> props)
        {
            if (this.EmitCallerMemberName)
            {
                props[nameof(callerMemberName)] = callerMemberName;
            }
        }
    }
}
