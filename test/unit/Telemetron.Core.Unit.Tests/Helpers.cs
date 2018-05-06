using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Security.Cryptography;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Polytech.Common.Telemetron.Core.Unit
{
    using static Polytech.Common.Telemetron.Diagnostics.DiagnosticTrace;

    public static class Helpers
    {
        private static Lazy<Random> random = new Lazy<Random>(() => { return new Random(); }, LazyThreadSafetyMode.ExecutionAndPublication);



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

        public static async Task AssertThrowAsync<T>(string failureMessage, Func<Task> action)
    where T : Exception
        {
            try
            {
                await action();
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

        public static async Task AssertThrowAsync<T>(Func<Task> action)
    where T : Exception
        {
            string fullName = typeof(T).FullName;

            await AssertThrowAsync<T>($"Expected to encounter a {fullName} during this action and did not", action);
        }

        public static async Task TestTelemetronCore<T>(
            this ITelemetronProvider<T> telemetron,
            Func<T> fakeContextFactory,
            string nullOperationId,
            string nullOperationIdContext,
            string nullContextId)
        {
            await TestTelemetronCoreInternalOuter2(telemetron, fakeContextFactory);

            using (var harness = new TraceListenerHarness(Source))
            {
                using (IOperation operation = telemetron.CreateOperation(null))
                {
                    operation.TelemetryData["this"] = "shouldnt get set";
                }

                using (IOperation operation = telemetron.CreateOperation(null, fakeContextFactory()))
                {
                    operation.TelemetryData["this"] = "shouldnt get set";
                }

                using (IOperation operation = telemetron.CreateOperation("foo", default(T)))
                {
                    operation.TelemetryData["this"] = "shouldnt get set";
                }

                await Task.Yield();

                harness.AssertAny(nullOperationId);
                harness.AssertAny(nullOperationIdContext);
                harness.AssertAny(nullContextId);
            }
        }

        private static async Task TestTelemetronCoreInternalOuter2<T>(ITelemetronProvider<T> telemetron,
            Func<T> fakeContextFactory)
        {
            using (IOperation op1 = telemetron.CreateOperation("firstOuter"))
            {
                telemetron.Run(() =>
                {
                    TestTelemetronCoreInternal(telemetron);
                });

                string result = telemetron.Run(() =>
                {
                    TestTelemetronCoreInternal(telemetron);
                    return "second";
                });

                await telemetron.RunAsync(async () =>
                {
                    await Task.Yield();
                    TestTelemetronCoreInternal(telemetron);
                });

                string asyncResult = await telemetron.RunAsync(async () =>
                {
                    await Task.Yield();
                    TestTelemetronCoreInternal(telemetron);

                    return "fourth";
                });
            }



            telemetron.Run("first", (op) =>
           {
               TestTelemetronCoreInternal(telemetron);
           });

            string parentresult = telemetron.Run("second", (op) =>
            {
                TestTelemetronCoreInternal(telemetron);
                return "second";
            });

            await telemetron.RunAsync("third", async (op) =>
            {
                await Task.Yield();
                TestTelemetronCoreInternal(telemetron);
            });

            string parentasyncResult = await telemetron.RunAsync("fourth", async (op) =>
            {
                await Task.Yield();
                TestTelemetronCoreInternal(telemetron);

                return "fourth";
            });

            telemetron.Run("first", fakeContextFactory(), (op) =>
            {
                TestTelemetronCoreInternal(telemetron);
            });

            string parentresulta = telemetron.Run("second", fakeContextFactory(), (op) =>
            {
                TestTelemetronCoreInternal(telemetron);
                return "second";
            });

            await telemetron.RunAsync("third", fakeContextFactory(), async (op) =>
            {
                await Task.Yield();
                TestTelemetronCoreInternal(telemetron);
            });

            string parentasyncResulta = await telemetron.RunAsync("fourth", fakeContextFactory(), async (op) =>
            {
                await Task.Yield();
                TestTelemetronCoreInternal(telemetron);

                return "fourth";
            });
        }

        private static void TestTelemetronCoreInternal<T>(ITelemetronProvider<T> telemetron)
        {
            telemetron.Debug("This is a debug message", "xe+hceL30EM");
            telemetron.Debug("This is a debug message with properties", "blQhLDeqsEM", GetRandomLogProperties());
            telemetron.Verbose("This is a verbose message", "HDkp0I2chEM");
            telemetron.Verbose("this is a verbose message with properties", "/ymqrP8U1UM", GetRandomLogProperties());
            telemetron.Info("This is an informational message", "NQcwppoDyEM");
            telemetron.Info("This is an informational message with properties", "V3YvgSu7x0M", GetRandomLogProperties());
            telemetron.Warning("This is a f**ing warning! ", "vJg+E15Mv0M");
            telemetron.Warning("This is a f#@(ng warning with some f(#(#@g properties", "iCAp90OQ1EM", GetRandomLogProperties());
            telemetron.Error("This is an error", ThrowAndCaptureException<InvalidOperationException>("Error message"), "FP1iLop+0UM");
            telemetron.Error("This is an error with some properties", ThrowAndCaptureException<InvalidOperationException>("Error message"), "J5E5eZPIzEM", GetRandomLogProperties());
            telemetron.Fatal("This is a fatal", ThrowAndCaptureException<InvalidOperationException>("Error message"), "bjSg8jYa0EM");
            telemetron.Fatal("This is a fatal", ThrowAndCaptureException<InvalidOperationException>("Error message"), "x675ZGPX3EM", GetRandomLogProperties());
            telemetron.Metric("test.default");
            telemetron.Metric("test.nonDefault", random.Value.Next());
            telemetron.Metric("test.nonDefaultWithProperties", random.Value.Next(), GetRandomLogProperties());
            telemetron.Event("test.eventNoProperties");
            telemetron.Event("test.eventWithProperties", GetRandomLogProperties());
        }

        public static Dictionary<string, string> GetRandomLogProperties(
            int propertyBagCountMinInclusive = 1,
            int propertyBagCountMaxExclusive = 255,
            int propertySizeMinInclusive = 512,
            int propertySizeMaxExclusive = 1 << 12)
        {
            if (propertyBagCountMinInclusive < 1)
            {
                throw new ArgumentException($"The {propertyBagCountMinInclusive} must be > 0", nameof(propertyBagCountMinInclusive));
            }

            if (propertySizeMinInclusive < 1)
            {
                throw new ArgumentException($"The {propertySizeMinInclusive} must be > 0", nameof(propertySizeMinInclusive));
            }

            if (propertyBagCountMaxExclusive > 1 << 19)
            {
                throw new ArgumentException($"Setting {propertySizeMaxExclusive} too high can lead to stability issues with mstest in constrained environments. max size = 1 << 19");
            }

            Dictionary<string, string> result = new Dictionary<string, string>();

            using (var rng = RandomNumberGenerator.Create())
            {
                byte[] propertyNameBuffer = new byte[512];

                for (int i = 0; i < random.Value.Next(propertyBagCountMinInclusive, propertyBagCountMaxExclusive); i++)
                {
                    byte[] newBuffer = new byte[random.Value.Next(propertySizeMinInclusive, propertySizeMaxExclusive)];

                    rng.GetNonZeroBytes(propertyNameBuffer);
                    rng.GetNonZeroBytes(newBuffer);

                    string name = Convert.ToBase64String(propertyNameBuffer);
                    string value = Convert.ToBase64String(newBuffer);

                    // i dont care if there is a collision lol
                    result[name] = value;
                }
            }

            return result;
        }

        public static Exception ThrowAndCaptureException<T>(string message)
            where T : Exception
        {
            try
            {
                ThrowAndCaptureExceptionInternal<T>(message);
                return default(T);
            }
            catch (Exception ex)
            {
                return ex;
            }
        }


        private static Exception ThrowAndCaptureExceptionInternal<T>(string message)
            where T : Exception
        {
            T exception = (T)Activator.CreateInstance(typeof(T), message);

            throw exception;
        }
    }
}