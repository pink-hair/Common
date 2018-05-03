using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;

namespace Polytech.Common.Telemetron
{
    public static class ExtensionMethods
    {
        /// <summary>
        /// Emits a trace event at the <see cref="EventSeverity.Debug"/> level.
        /// </summary>
        /// <param name="provider">The trace provider being used.</param>
        /// <param name="message">Message.</param>
        /// <param name="codePoint">Code point.</param>
        /// <param name="data">Data.</param>
        /// <param name="callerMemberName">Caller member name.</param>
        /// <param name="callerFilePath">Caller file path.</param>
        /// <param name="callerLineNumber">Caller line number.</param>
        /// <returns>A value indicating if the emission of the trace event was successful.</returns>
        public static bool Debug(
            this ITraceProvider provider,
            string message,
            string codePoint = null,
            Dictionary<string, string> data = null,
            [CallerMemberName]string callerMemberName = "",
            [CallerFilePath]string callerFilePath = "",
            [CallerLineNumber]int callerLineNumber = -1) =>
                provider.Trace(
                    EventSeverity.Debug,
                    message,
                    codePoint,
                    data,
                    callerMemberName,
                    callerFilePath,
                    callerLineNumber);

        /// <summary>
        /// Emits a trace event at the <see cref="EventSeverity.Verbose"/> level.
        /// </summary>
        /// <param name="provider">The trace provider being used.</param>
        /// <param name="message">Message.</param>
        /// <param name="codePoint">Code point.</param>
        /// <param name="data">Data.</param>
        /// <param name="callerMemberName">Caller member name.</param>
        /// <param name="callerFilePath">Caller file path.</param>
        /// <param name="callerLineNumber">Caller line number.</param>
        /// <returns>A value indicating if the emission of the trace event was successful.</returns>
        public static bool Verbose(
            this ITraceProvider provider,
            string message,
            string codePoint = null,
            Dictionary<string, string> data = null,
            [CallerMemberName]string callerMemberName = "",
            [CallerFilePath]string callerFilePath = "",
            [CallerLineNumber]int callerLineNumber = -1) =>
                provider.Trace(
                    EventSeverity.Verbose,
                    message,
                    codePoint,
                    data,
                    callerMemberName,
                    callerFilePath,
                    callerLineNumber);

        /// <summary>
        /// Emits a trace event at the <see cref="EventSeverity.Info"/> level.
        /// </summary>
        /// <param name="provider">The trace provider being used.</param>
        /// <param name="message">Message.</param>
        /// <param name="codePoint">Code point.</param>
        /// <param name="data">Data.</param>
        /// <param name="callerMemberName">Caller member name.</param>
        /// <param name="callerFilePath">Caller file path.</param>
        /// <param name="callerLineNumber">Caller line number.</param>
        /// <returns>A value indicating if the emission of the trace event was successful.</returns>
        public static bool Info(
            this ITraceProvider provider,
            string message,
            string codePoint = null,
            Dictionary<string, string> data = null,
            [CallerMemberName]string callerMemberName = "",
            [CallerFilePath]string callerFilePath = "",
            [CallerLineNumber]int callerLineNumber = -1) =>
                provider.Trace(
                    EventSeverity.Info,
                    message,
                    codePoint,
                    data,
                    callerMemberName,
                    callerFilePath,
                    callerLineNumber);
        /// <summary>
        /// Emits a trace event at the <see cref="EventSeverity.Warning"/> level.
        /// </summary>
        /// <param name="provider">The trace provider being used.</param>
        /// <param name="message">Message.</param>
        /// <param name="codePoint">Code point.</param>
        /// <param name="data">Data.</param>
        /// <param name="callerMemberName">Caller member name.</param>
        /// <param name="callerFilePath">Caller file path.</param>
        /// <param name="callerLineNumber">Caller line number.</param>
        /// <returns>A value indicating if the emission of the trace event was successful.</returns>
        public static bool Warning(
            this ITraceProvider provider,
            string message,
            string codePoint = null,
            Dictionary<string, string> data = null,
            [CallerMemberName]string callerMemberName = "",
            [CallerFilePath]string callerFilePath = "",
            [CallerLineNumber]int callerLineNumber = -1) =>
                provider.Trace(
                    EventSeverity.Warning,
                    message,
                    codePoint,
                    data,
                    callerMemberName,
                    callerFilePath,
                    callerLineNumber);

        /// <summary>
        /// Emits a trace event at the <see cref="EventSeverity.Error"/> level.
        /// </summary>
        /// <param name="provider">The trace provider being used.</param>
        /// <param name="message">Message.</param>
        /// <param name="exception">the exception that caused the program to emit this event.</param>
        /// <param name="codePoint">Code point.</param>
        /// <param name="data">Data.</param>
        /// <param name="callerMemberName">Caller member name.</param>
        /// <param name="callerFilePath">Caller file path.</param>
        /// <param name="callerLineNumber">Caller line number.</param>
        /// <returns>A value indicating if the emission of the trace event was successful.</returns>
        public static bool Error(
            this ITraceProvider provider,
            string message,
            Exception exception,
            string codePoint = null,
            Dictionary<string, string> data = null,
            [CallerMemberName]string callerMemberName = "",
            [CallerFilePath]string callerFilePath = "",
            [CallerLineNumber]int callerLineNumber = -1) =>
                provider.Trace(
                    EventSeverity.Error,
                    message,
                    exception,
                    codePoint,
                    data,
                    callerMemberName,
                    callerFilePath,
                    callerLineNumber);

        /// <summary>
        /// Emits a trace event at the <see cref="EventSeverity.Fatal"/> level.
        /// </summary>
        /// <param name="provider">The trace provider being used.</param>
        /// <param name="message">Message.</param>
        /// <param name="exception">the exception that caused the program to emit this event.</param>
        /// <param name="codePoint">Code point.</param>
        /// <param name="data">Data.</param>
        /// <param name="callerMemberName">Caller member name.</param>
        /// <param name="callerFilePath">Caller file path.</param>
        /// <param name="callerLineNumber">Caller line number.</param>
        /// <returns>A value indicating if the emission of the trace event was successful.</returns>
        public static bool Fatal(
            this ITraceProvider provider,
            string message,
            Exception exception,
            string codePoint = null,
            Dictionary<string, string> data = null,
            [CallerMemberName]string callerMemberName = "",
            [CallerFilePath]string callerFilePath = "",
            [CallerLineNumber]int callerLineNumber = -1) =>
                provider.Trace(
                    EventSeverity.Fatal,
                    message,
                    exception,
                    codePoint,
                    data,
                    callerMemberName,
                    callerFilePath,
                    callerLineNumber);

        public static Dictionary<string, string> FlattenException(this Exception ex)
        {
            string currentPath = "/";

            Dictionary<string, string> result = new Dictionary<string, string>();

            FlattenInternal(ex, currentPath, result);

            return result;
        }

        public static string GetBase64String(this long num)
        {
            byte[] bytes = BitConverter.GetBytes(num);

            string result = Convert.ToBase64String(bytes);

            return result;
        }

        public static Dictionary<string, string> SafeCombine(this Dictionary<string, string> provider, Exception ex)
        {
            if (provider == null)
            {
                provider = new Dictionary<string, string>();
            }

            if (ex != null)
            {
                Dictionary<string, string> exceptionProperties = ex.FlattenException();
                foreach (KeyValuePair<string, string> row in exceptionProperties)
                {
                    provider[row.Key] = row.Value;
                }
            }

            return provider;
        }

        private static void FlattenInternal(Exception target, string currentPath, Dictionary<string, string> properties)
        {
            string path = currentPath + target.GetType().Name + '/'; ;
            if (target is AggregateException)
            {
                AggregateException aggregate = target as AggregateException;

                if (aggregate.InnerExceptions != null && aggregate.InnerExceptions.Count != 0)
                {
                    foreach (Exception ex in aggregate.InnerExceptions)
                    {
                        FlattenInternal(ex, path, properties);
                    }
                }
                else
                {
                    if (aggregate.InnerException != null)
                    {
                        FlattenInternal(aggregate.InnerException, currentPath, properties);
                    }
                }
            }
            else
            {
                if (target.InnerException != null)
                {
                    FlattenInternal(target.InnerException, path, properties);
                }
            }

            properties[path + "message"] = target.Message;
            properties[path + "stackTrace"] = target.StackTrace;

            string targetSite = target.TargetSite?.ToString();

            if (targetSite != null)
            {
                properties[path + "targetSite"] = targetSite;
            }

        }

        /// <summary>
        /// Creates a new pseudorandomnumber
        /// </summary>
        /// <param name="r">The random provider.</param>
        /// <returns>The next random.</returns>
        internal static long NextInt64(this Random r)
        {
            return (long)(r.NextDouble() * long.MaxValue);
        }
    }
}
