namespace Polytech.Common.Telemetron
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    /// <summary>
    /// A provider for emitting events.
    /// </summary>
    public interface IEventProvider
    {
        /// <summary>
        /// Emit an event with the specified name and optional data.
        /// </summary>
        /// <param name="name">The name or id of the event. try to use path context. broad.medium.narrow.granular namespace style.</param>
        /// <param name="data">Optional data.</param>
        /// <returns>a value indicating if the submission of the event was successful.</returns>
        bool Event(string name, Dictionary<string, string> data = null);
    }
}
