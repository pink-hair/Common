namespace PinkHair.Common
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;
    using System.Text;
    using System.Threading.Tasks;

    /// <summary>
    /// Extension Methods for contextual Test class.
    /// </summary>
    public static class ContextualTestClassExtensions
    {
        public static void RunExpectingException<T>(this IContextualTestClass provider, Action action, [CallerMemberName]string callerMemberName = "")
            where T : Exception
        {
            try
            {
                action();
            }
            catch (T caughtException)
            {
                Type exceptionType = caughtException.GetType();
                provider.TestContext.WriteLine($"Run {nameof(RunExpectingException)} successful. Caught Exception of type {exceptionType.FullName}.");
            }
        }

        public static async Task RunExpectingException<T>(this IContextualTestClass provider, Func<Task> action, [CallerMemberName]string callerMemberName = "")
            where T : Exception
        {
            try
            {
                await action();
            }
            catch (T caughtException)
            {
                Type exceptionType = caughtException.GetType();
                provider.TestContext.WriteLine($"Run {nameof(RunExpectingException)} successful. Caught Exception of type {exceptionType.FullName}.");
            }
        }
    }
}
