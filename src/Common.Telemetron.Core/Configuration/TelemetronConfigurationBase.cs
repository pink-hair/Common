using System;
namespace Polytech.Common.Telemetron.Configuration
{
    public class TelemetronConfigurationBase : ITelemetronConfigurationBase
    {
        /// <summary>
        /// Gets or sets the Delegate that will be ran when no codepoint is provided to the logging methods. fd
        /// </summary>
        /// <value>The null codepoint action.</value>
        public TelemetronNullCodePointDelegte NullCodepointAction { get; set; }

        public bool EmitCodePoint { get; set; }

        public bool EmitCallerMemberName { get; set; }

        public bool EmitCallerFilePath { get; set; }

        public bool EmitCallerLineNumber { get; set; }

        public bool EmitAdditionalData { get; set; }

        public bool EmitCorrelationContext { get; set; }
    }
}
