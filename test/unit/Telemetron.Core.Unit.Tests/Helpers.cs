using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace Telemetron.Core.Unit.Tests
{
    [ExcludeFromCodeCoverage]
    public static class Helpers
    {
        public static void AssertThrow<T>(string failureMessage, Action action)
            where T : Exception
        {
            try
            {
                action();
                Assert.Fail(failureMessage);
            }
            catch (Exception ex) when (ex.GetType() == typeof(T))
            {
                // do nothing, the expected exception was caught.
            }
        }

        public static void AssertThrow<T>(Action action)
            where T : Exception
        {
            string fullName = typeof(T).FullName;

            AssertThrow<T>($"Expected to encounter a {fullName} during this action and did not", action);
        }
    }
}
