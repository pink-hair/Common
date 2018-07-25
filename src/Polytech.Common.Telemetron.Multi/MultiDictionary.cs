namespace PinkHair.Common.Telemetron
{
    using System;
    using System.Collections;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using PinkHair.Common.Telemetron.Diagnostics;
    using static PinkHair.Common.Telemetron.Diagnostics.DiagnosticTrace;

    internal class MultiDictionary : IDictionary<string, string>
    {
        private IDictionary<Guid, IOperation> operations;
        private IOperation first;

        public MultiDictionary(MultiOperation parent)
        {
            if (parent == null)
            {
                throw new ArgumentNullException(nameof(parent));
            }

            if (parent.Operations == null)
            {
                throw new ArgumentNullException(nameof(parent) + '.' + nameof(parent.Operations));
            }

            if (parent.Operations.Count == 0)
            {
                throw new ArgumentException("A MultiOperation must be provided that containst at least one operation", nameof(parent) + '.' + nameof(parent.Operations));
            }

            this.operations = parent.Operations;
            this.first = this.operations.FirstOrDefault().Value;
        }

        public string this[string key]
        {
            get
            {
                try
                {
                    return this.first.TelemetryData[key];
                }
                catch (Exception ex)
                when (Filter($"An unexpected exception occurred when attempting to get telemetry data @ {key}", ex))
                {
                    return "ERR_EXCEPTION_UNK";
                }
            }

            set
            {
                try
                {
                    foreach (KeyValuePair<Guid, IOperation> operation in this.operations)
                    {
                        operation.Value.TelemetryData[key] = value;
                    }
                }
                catch (Exception ex)
                when (Filter($"An unexpected exception occurred when attempting to set telemetry data @ {key}", ex))
                { }
            }
        }

        public ICollection<string> Keys
        {
            get
            {
                try
                {
                    return this.first.TelemetryData?.Keys;
                }
                catch (Exception ex)
                when (Filter($"An unexpected exception occurred when attempting to get Keys", ex))
                {
                    return null;
                }
            }
        }

        public ICollection<string> Values
        {
            get
            {
                try
                {
                    return this.first.TelemetryData?.Values;
                }
                catch (Exception ex)
                when (Filter($"An unexpected exception occurred when attempting to get Values", ex))
                {
                    return null;
                }
            }
        }

        public int Count
        {
            get
            {
                try
                {
                    return this.first.TelemetryData.Count;
                }
                catch (Exception ex)
                when (Filter($"An unexpected exception occurred when attempting to get Keysd", ex))
                {
                    return -1;
                }
            }
        }

        public bool IsReadOnly => false;

        public void Add(string key, string value)
        {
            try
            {
                foreach (IOperation operation in this.operations.Values)
                {
                    operation.TelemetryData.Add(key, value);
                }
            }
            catch (Exception ex)
            when (Filter($"An unexpected exception occurred when attempting to add value at key {key}", ex))
            {
            }
        }

        public void Add(KeyValuePair<string, string> item)
        {
            try
            {
                foreach (IOperation operation in this.operations.Values)
                {
                    operation.TelemetryData.Add(item);
                }
            }
            catch (Exception ex)
            when (Filter($"An unexpected exception occurred when attempting to add value at key {item.Key}", ex))
            {
            }
        }

        public void Clear()
        {
            try
            {
                foreach (IOperation operation in this.operations.Values)
                {
                    operation.TelemetryData.Clear();
                }
            }
            catch (Exception ex)
            when (Filter($"An unexpected exception occurred when attempting to clear the dictionary", ex))
            {
            }
        }

        public bool Contains(KeyValuePair<string, string> item)
        {
            try
            {
                return this.first.TelemetryData.Contains(item);
            }
            catch (Exception ex)
            when (Filter($"An unexpected exception occurred when attempting to check if item of key {item.Key} exists via kvp", ex))
            {
                return false;
            }
        }

        public bool ContainsKey(string key)
        {
            try
            {
                return this.first.TelemetryData.ContainsKey(key);
            }
            catch (Exception ex)
            when (Filter($"An unexpected exception occurred when attempting to check if item of key {key}", ex))
            {
                return false;
            }
        }

        public void CopyTo(KeyValuePair<string, string>[] array, int arrayIndex)
        {
            try
            {
                this.first.TelemetryData.CopyTo(array, arrayIndex);
            }
            catch (Exception ex)
            when (Filter($"An unexpected exception occurred when attempting to copy the collection to an array", ex))
            {
            }
        }

        public IEnumerator<KeyValuePair<string, string>> GetEnumerator()
        {
            try
            {
                return this.first.TelemetryData.GetEnumerator();
            }
            catch (Exception ex)
            when (Filter($"An unexpected exception occurred when attempting to get the enumerator", ex))
            {
                return Enumerable.Empty<KeyValuePair<string, string>>().GetEnumerator();
            }
        }

        public bool Remove(string key)
        {
            bool success = true;
            foreach (IOperation operation in this.operations.Values)
            {
                try
                {
                    this.first.TelemetryData.Remove(key);
                }
                catch (Exception ex)
                when (Filter($"An unexpected exception occurred when attempting to remove the item @ key {key}", ex))
                {
                    success = false;
                }
            }

            return success;
        }

        public bool Remove(KeyValuePair<string, string> item)
        {
            bool success = true;
            foreach (IOperation operation in this.operations.Values)
            {
                try
                {
                    this.first.TelemetryData.Remove(item);
                }
                catch (Exception ex)
                when (Filter($"An unexpected exception occurred when attempting to remove the item @ key {item.Key} via kvp", ex))
                {
                    success = false;
                }
            }

            return success;
        }

        public bool TryGetValue(string key, out string value)
        {
            try
            {
                return this.first.TelemetryData.TryGetValue(key, out value);
            }
            catch (Exception ex)
            when (Filter($"An unexpected exception occurred when attempting to get the enumerator", ex))
            {
                value = null;
                return false;
            }
        }

        IEnumerator IEnumerable.GetEnumerator() => this.GetEnumerator();
    }
}
