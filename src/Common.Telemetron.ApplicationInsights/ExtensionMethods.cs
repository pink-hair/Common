namespace Polytech.Common.Telemetron
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using Microsoft.ApplicationInsights.Channel;
    using Microsoft.ApplicationInsights.DataContracts;
    using Polytech.Common.Extension;

    public static class ExtensionMethods
    {
        public static void SetOperationInfo(this ITelemetry telemetry, ApplicationInsightsTelemetron provider)
        {
            telemetry.Context.Operation.Id = provider.CorrelationContext.CurrentId.GetBase64String();
            telemetry.Context.Operation.ParentId = provider.CorrelationContext.ParentId.GetBase64String();
        }

        /// <summary>
        /// Merges the properties from an <see cref="IDictionary{TKey, TValue}"/> into the <see cref="ISupportProperties" />Telemetry object 
        /// </summary>
        /// <param name="telemetry">The object into which the properties will be merged.</param>
        /// <param name="properties">The properties to merge in.</param>
        public static void MergeProperties(this ISupportProperties telemetry, IDictionary<string, string> properties)
        {
            if (telemetry == null)
            {
                throw new ArgumentNullException(nameof(telemetry));
            }

            if (properties == null)
            {
                throw new ArgumentNullException(nameof(properties));
            }

            foreach (KeyValuePair<string, string> property in properties)
            {
                string kn;

                if (telemetry.Properties.ContainsKey(property.Key))
                {
                    kn = string.Concat("MERGED_", property.Key);
                }
                else
                {
                    kn = property.Key;
                }

                telemetry.Properties.Add(kn, property.Value);
            }
        }
    }
}
