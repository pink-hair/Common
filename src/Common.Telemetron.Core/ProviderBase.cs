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

        private IOperationConfiguration operationConfiguration;

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
            this.operationConfiguration = configuration.OperationConfiguration
                ?? throw new ArgumentNullException($"{nameof(configuration)}.{nameof(configuration.OperationConfiguration)}");

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

        public IOperationConfiguration OperationConfiguration => this.operationConfiguration;

        /// <summary>
        /// Gets the current time string from a date time object.
        /// </summary>
        /// <param name="dt">The date time object to stringify</param>
        /// <returns>The shortest time string possible.</returns>
        protected static string GetTimeString(DateTime dt)
        {
            if (dt.Hour == 0 && dt.Minute == 0)
            {
                // midnight
                return dt.ToString("MMM-dd-YY HH:mm:ss.fff") + " midnight";
            }
            else
            {
                if (dt.Second == 0)
                {
                    if (dt.Millisecond == 0)
                    {
                        return dt.ToString("HH:mm");
                    }

                    return dt.ToString("HH:mm:ss");
                }

                return dt.ToString("HH:mm:ss.fff");
            }
        }

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
