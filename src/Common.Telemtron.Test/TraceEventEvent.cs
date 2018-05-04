namespace Polytech.Common.Telemetron
{
    using System;
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using System.Text;
    using Microsoft.VisualStudio.TestTools.UnitTesting;
    using Polytech.Common.Telemetron.Diagnostics;

    /// <summary>
    /// A tracing event.
    /// </summary>
    public struct TraceEventEvent
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TraceEventEvent"/> struct.
        /// </summary>
        /// <param name="message">The message to construct from.</param>
        internal TraceEventEvent(string message)
        {
            this.Actual = message;
            this.Upper = message.ToUpperInvariant();

            int indexOf = message.IndexOf("##");
            if (indexOf != -1)
            {
                this.Data = message.Substring(indexOf + 2);
            }
            else
            {
                this.Data = null;
            }
        }

        public string Actual { get; set; }

        public string Upper { get; set; }

        public string Data { get; set; }

        public override string ToString()
        {
            return this.Actual;
        }
    }
}
