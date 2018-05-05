using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace Telemetron.Console.Unit.Tests
{
    using Polytech.Common.Telemetron;
    using static global::Polytech.Common.Telemetron.Diagnostics.DiagnosticTrace;
    using static Common;
    using static Polytech.Common.Telemetron.Core.Unit.Helpers;
    using System.Linq;
    using Telemetron.Core.Unit.Tests;
    using System.Threading.Tasks;

    [TestClass]
    [ExcludeFromCodeCoverage]
    public class OperationsController
    {
        private static byte[] getContext()
        {
            return new byte[] { 57, 5, 0, 0, 0, 0, 0, 0 };
        }

        [TestMethod]
        public void TestOperationInitNoErrors()
        {
            using (TraceListenerHarness harness = new TraceListenerHarness(Source))
            {
                var log = Get();

                using (IOperation operation = log.CreateOperation("Foo"))
                {
                    log.Info("Bar");
                }

                harness.AssertContains("bar");
                harness.AssertContains("foo");
                harness.AssertNotContains("fatal");
                harness.AssertNotContains("zoolander");
                harness.AssertNotContains("schools for ants.");
            }
        }

        #region RunVoid
        [TestMethod]
        public void TestRunOperationVoid()
        {
            using (TraceListenerHarness harness = new TraceListenerHarness(Source))
            {
                var log = Get();

                log.Run(nameof(TestRunOperationVoid), (operation) =>
                {

                });

                harness.AssertDataAreEqual("OPB/DRz430M", 1, 3);
            }
        }

        [TestMethod]
        public void TestRunOperationVoidWithData()
        {
            using (TraceListenerHarness harness = new TraceListenerHarness(Source))
            {
                var log = Get();

                log.Run(nameof(TestRunOperationVoid), (operation) =>
                {
                    operation.TelemetryData["test"] = "Ek588Agn3kM";
                });

                harness.AssertDataAreEqual("OPB/DRz430M", 1, 4);
            }
        }

        [TestMethod]
        public void TestRunOperationVoid_ExceptionalOperationNameNullEmptyAndWhitespace()
        {
            using (TraceListenerHarness harness = new TraceListenerHarness(Source))
            {
                var log = Get();

                AssertThrow<ArgumentNullException>(() =>
                {
                    log.Run(null, (operation) =>
                    {

                    });
                });

                AssertThrow<ArgumentNullException>(() =>
                {
                    log.Run("", (operation) =>
                    {

                    });
                });

                AssertThrow<ArgumentNullException>(() =>
                {
                    log.Run("                                             ", (operation) =>
                    {

                    });
                });
            }
        }

        [TestMethod]
        public void TestRunOperationVoid_ExceptionNoDelegate()
        {
            using (TraceListenerHarness harness = new TraceListenerHarness(Source))
            {
                var log = Get();

                AssertThrow<ArgumentNullException>(() => log.Run(nameof(TestRunOperationVoid), action: null));
            }
        }

        [TestMethod]
        public void TestRunOperationVoid_EnsureInternalException()
        {
            using (TraceListenerHarness harness = new TraceListenerHarness(Source))
            {
                var log = Get();

                AssertThrow<FakeException>(() =>
                {
                    log.Run(nameof(TestRunOperationVoid), (operation) =>
                    {
                        throw new FakeException("AAAAAAAAAh");
                    });
                });
                //2

                // 3 because of exception.
                harness.AssertDataAreEqual("OPB/DRz430M", 1, 3);
            }
        }

        [TestMethod]
        public void TestRunOperationVoid_FailureSetInternal()
        {
            using (TraceListenerHarness harness = new TraceListenerHarness(Source))
            {
                var log = Get();

                log.Run(nameof(TestRunOperationVoid), (operation) =>
                {
                    operation.SetOperationResult(OperationResult.Failed);
                });

                harness.AssertDataContains("xOI8HWJxzkM", "failed", false);
            }
        }

        #endregion

        #region RunReturn
        [TestMethod]
        public void TestRunOperationReturn()
        {
            using (TraceListenerHarness harness = new TraceListenerHarness(Source))
            {
                var log = Get();

                string foo = log.Run(nameof(TestRunOperationVoid), (operation) =>
                {
                    return "foo";
                });

                harness.AssertDataAreEqual("OPB/DRz430M", 1, 3);
            }
        }

        [TestMethod]
        public void TestRunOperationReturnWithData()
        {
            using (TraceListenerHarness harness = new TraceListenerHarness(Source))
            {
                var log = Get();

                string foo = log.Run(nameof(TestRunOperationVoid), (operation) =>
                {
                    operation.TelemetryData["test"] = "Ek588Agn3kM";
                    return "foo";
                });

                harness.AssertDataAreEqual("OPB/DRz430M", 1, 4);
            }
        }

        [TestMethod]
        public void TestRunOperationReturn_ExceptionalOperationNameNullEmptyAndWhitespace()
        {
            using (TraceListenerHarness harness = new TraceListenerHarness(Source))
            {
                var log = Get();

                AssertThrow<ArgumentNullException>(() =>
                {
                    string foo = log.Run(null, (operation) =>
                    {
                        return "foo";
                    });
                });

                AssertThrow<ArgumentNullException>(() =>
                {
                    string bar = log.Run("", (operation) =>
                    {
                        return "bar";
                    });
                });

                AssertThrow<ArgumentNullException>(() =>
                {
                    string foobar = log.Run("                                             ", (operation) =>
                    {
                        return "foobar";
                    });
                });
            }
        }

        [TestMethod]
        public void TestRunOperationReturn_ExceptionNoDelegate()
        {
            using (TraceListenerHarness harness = new TraceListenerHarness(Source))
            {
                var log = Get();

                AssertThrow<ArgumentNullException>(() =>
                {
                    string foo = log.Run<string, byte[]>(nameof(TestRunOperationVoid), null);
                });
            }
        }

        [TestMethod]
        public void TestRunOperationReturn_EnsureInternalException()
        {
            using (TraceListenerHarness harness = new TraceListenerHarness(Source))
            {
                var log = Get();

                AssertThrow<FakeException>(() =>
                {
                    string foo = log.Run(nameof(TestRunOperationVoid), (operation) =>
                    {
                        throw new FakeException("AAAAAAAAAh");
                        return "foo";
                    });
                });
                //2

                // 3 because of exception.
                harness.AssertDataAreEqual("OPB/DRz430M", 1, 3);
            }
        }

        [TestMethod]
        public void TestRunOperationReturn_FailureSetInternal()
        {
            using (TraceListenerHarness harness = new TraceListenerHarness(Source))
            {
                var log = Get();

                string foo = log.Run(nameof(TestRunOperationVoid), (operation) =>
                {
                    operation.SetOperationResult(OperationResult.Failed);
                    return "foo";
                });

                harness.AssertDataContains("tG25Ntq2zEM", "failed", false);
            }
        }

        #endregion

        #region RunVoidAsync
        [TestMethod]
        public async Task TestAsyncRunOperationVoid()
        {
            using (TraceListenerHarness harness = new TraceListenerHarness(Source))
            {
                var log = Get();

                await log.RunAsync(nameof(TestRunOperationVoid), async (operation) =>
                {
                    await Task.Yield();
                });

                harness.AssertDataAreEqual("OPB/DRz430M", 1, 3);
            }
        }

        [TestMethod]
        public async Task TestAsyncRunOperationVoidWithData()
        {
            using (TraceListenerHarness harness = new TraceListenerHarness(Source))
            {
                var log = Get();

                await log.RunAsync(nameof(TestRunOperationVoid), async (operation) =>
                {
                    await Task.Yield();
                    operation.TelemetryData["test"] = "Ek588Agn3kM";
                });

                harness.AssertDataAreEqual("OPB/DRz430M", 1, 4);
            }
        }

        [TestMethod]
        public async Task TestAsyncRunOperationVoid_ExceptionalOperationNameNullEmptyAndWhitespace()
        {
            using (TraceListenerHarness harness = new TraceListenerHarness(Source))
            {
                var log = Get();

                await AssertThrowAsync<ArgumentNullException>(async () =>
                {
                    await log.RunAsync(null, async (operation) =>
                    {
                        await Task.Yield();
                    });
                });

                await AssertThrowAsync<ArgumentNullException>(async () =>
                {
                    await log.RunAsync("", async (operation) =>
                    {
                        await Task.Yield();
                    });
                });

                await AssertThrowAsync<ArgumentNullException>(async() =>
                {
                    await log.RunAsync("                                             ", async (operation) =>
                    {
                        await Task.Yield();
                    });
                });
            }
        }

        [TestMethod]
        public async Task TestAsyncRunOperationVoid_ExceptionNoDelegate()
        {
            using (TraceListenerHarness harness = new TraceListenerHarness(Source))
            {
                var log = Get();

                await AssertThrowAsync<ArgumentNullException>(async () => await log.RunAsync(nameof(TestRunOperationVoid), action: null));
            }
        }

        [TestMethod]
        public async Task TestAsyncRunOperationVoid_EnsureInternalException()
        {
            using (TraceListenerHarness harness = new TraceListenerHarness(Source))
            {
                var log = Get();

                await AssertThrowAsync<FakeException>(async () =>
                {
                    await log.RunAsync(nameof(TestRunOperationVoid), async (operation) =>
                    {
                        await Task.Yield();
                        throw new FakeException("AAAAAAAAAh");
                    });
                });
                //2

                // 3 because of exception.
                harness.AssertDataAreEqual("OPB/DRz430M", 1, 3);
            }
        }

        [TestMethod]
        public async Task TestAsyncRunOperationVoid_FailureSetInternal()
        {
            using (TraceListenerHarness harness = new TraceListenerHarness(Source))
            {
                var log = Get();

                await log.RunAsync(nameof(TestRunOperationVoid), async (operation) =>
                {
                    await Task.Yield();
                    operation.SetOperationResult(OperationResult.Failed);
                });

                harness.AssertDataContains("0kJ6Fmkh3UM", "failed", false);
            }
        }

        #endregion

        #region RunReturnAsync
        [TestMethod]
        public async Task TestAsyncRunOperationReturn()
        {
            using (TraceListenerHarness harness = new TraceListenerHarness(Source))
            {
                var log = Get();

                string foo = await log.RunAsync(nameof(TestRunOperationVoid), async (operation) =>
                {
                    await Task.Yield();
                    return "foo";
                });

                harness.AssertDataAreEqual("OPB/DRz430M", 1, 3);
            }
        }

        [TestMethod]
        public async Task TestAsyncRunOperationReturnWithData()
        {
            using (TraceListenerHarness harness = new TraceListenerHarness(Source))
            {
                var log = Get();

                string foo = await log.RunAsync(nameof(TestRunOperationVoid), async (operation) =>
                {
                    await Task.Yield();
                    operation.TelemetryData["test"] = "Ek588Agn3kM";
                    return "foo";
                });

                harness.AssertDataAreEqual("OPB/DRz430M", 1, 4);
            }
        }

        [TestMethod]
        public async Task TestAsyncRunOperationReturn_ExceptionalOperationNameNullEmptyAndWhitespace()
        {
            using (TraceListenerHarness harness = new TraceListenerHarness(Source))
            {
                var log = Get();

                await AssertThrowAsync<ArgumentNullException>(async () =>
                {
                    string foo = await log.RunAsync(null, async (operation) =>
                    {
                        await Task.Yield();
                        return "foo";
                    });
                });

                await AssertThrowAsync<ArgumentNullException>(async () =>
                {
                    string bar = await log.RunAsync("", async (operation) =>
                    {
                        await Task.Yield();
                        return "bar";
                    });
                });

                await AssertThrowAsync<ArgumentNullException>(async () =>
                {
                    string foobar = await log.RunAsync("                                             ", async (operation) =>
                    {
                        await Task.Yield();
                        return "foobar";
                    });
                });
            }
        }

        [TestMethod]
        public async Task TestAsyncRunOperationReturn_ExceptionNoDelegate()
        {
            using (TraceListenerHarness harness = new TraceListenerHarness(Source))
            {
                var log = Get();

                await AssertThrowAsync<ArgumentNullException>(async () =>
                {
                    await Task.Yield();
                    string foo = await log.RunAsync<string, byte[]>(nameof(TestRunOperationVoid), null);
                });
            }
        }

        [TestMethod]
        public async Task TestAsyncRunOperationReturn_EnsureInternalException()
        {
            using (TraceListenerHarness harness = new TraceListenerHarness(Source))
            {
                var log = Get();

                await AssertThrowAsync<FakeException>(async () =>
                {
                    string foo = await log.RunAsync(nameof(TestRunOperationVoid), async (operation) =>
                    {
                        await Task.Yield();
                        throw new FakeException("AAAAAAAAAh");
                        return "foo";
                    });
                });
                //2

                // 3 because of exception.
                harness.AssertDataAreEqual("OPB/DRz430M", 1, 3);
            }
        }

        [TestMethod]
        public async Task TestAsyncRunOperationReturn_FailureSetInternal()
        {
            using (TraceListenerHarness harness = new TraceListenerHarness(Source))
            {
                var log = Get();

                string foo = await log.RunAsync(nameof(TestRunOperationVoid), async (operation) =>
                {
                    await Task.Yield();
                    operation.SetOperationResult(OperationResult.Failed);
                    return "foo";
                });

                harness.AssertDataContains("VqQ2PitS20M", "failed", false);
            }
        }

        #endregion

        #region RunVoidWithContext

        [TestMethod]
        public void TestWithContextRunOperationVoid()
        {
            using (TraceListenerHarness harness = new TraceListenerHarness(Source))
            {
                var log = Get();

                log.Run(nameof(TestRunOperationVoid), getContext(), (operation) =>
                {

                });

                harness.AssertDataAreEqual("OPB/DRz430M", 1, 3);
            }
        }

        [TestMethod]
        public void TestWithContextRunOperationVoidWithData()
        {
            using (TraceListenerHarness harness = new TraceListenerHarness(Source))
            {
                var log = Get();

                log.Run(nameof(TestRunOperationVoid), getContext(), (operation) =>
                {
                    operation.TelemetryData["test"] = "Ek588Agn3kM";
                });

                harness.AssertDataAreEqual("OPB/DRz430M", 1, 4);
            }
        }

        [TestMethod]
        public void TestWithContextRunOperationVoid_ExceptionalOperationNameNullEmptyAndWhitespace()
        {
            using (TraceListenerHarness harness = new TraceListenerHarness(Source))
            {
                var log = Get();

                AssertThrow<ArgumentNullException>(() =>
                {
                    log.Run(null, (operation) =>
                    {

                    });
                });

                AssertThrow<ArgumentNullException>(() =>
                {
                    log.Run("", (operation) =>
                    {

                    });
                });

                AssertThrow<ArgumentNullException>(() =>
                {
                    log.Run("                                             ", (operation) =>
                    {

                    });
                });
            }
        }

        [TestMethod]
        public void TestWithContextRunOperationVoid_ExceptionNoDelegate()
        {
            using (TraceListenerHarness harness = new TraceListenerHarness(Source))
            {
                var log = Get();

                AssertThrow<ArgumentNullException>(() => log.Run(nameof(TestRunOperationVoid), getContext(), action: null));
            }
        }

        [TestMethod]
        public void TestWithContextRunOperationVoid_EnsureInternalException()
        {
            using (TraceListenerHarness harness = new TraceListenerHarness(Source))
            {
                var log = Get();

                AssertThrow<FakeException>(() =>
                {
                    log.Run(nameof(TestRunOperationVoid), getContext(), (operation) =>
                    {
                        throw new FakeException("AAAAAAAAAh");
                    });
                });
                //2

                // 3 because of exception.
                harness.AssertDataAreEqual("OPB/DRz430M", 1, 3);
            }
        }

        [TestMethod]
        public void TestWithContextRunOperationVoid_FailureSetInternal()
        {
            using (TraceListenerHarness harness = new TraceListenerHarness(Source))
            {
                var log = Get();

                log.Run(nameof(TestRunOperationVoid), getContext(), (operation) =>
                {
                    operation.SetOperationResult(OperationResult.Failed);
                });

                harness.AssertDataContains("yAU6I+QC3UM", "failed", false);
            }
        }

        #endregion

        #region RunReturnWithContext

        [TestMethod]
        public void TestWithContextRunOperationReturn()
        {
            using (TraceListenerHarness harness = new TraceListenerHarness(Source))
            {
                var log = Get();

                string foo = log.Run(nameof(TestRunOperationVoid), getContext(), (operation) =>
                {
                    return "foo";
                });

                harness.AssertDataAreEqual("OPB/DRz430M", 1, 3);
            }
        }

        [TestMethod]
        public void TestWithContextRunOperationReturnWithData()
        {
            using (TraceListenerHarness harness = new TraceListenerHarness(Source))
            {
                var log = Get();

                string foo = log.Run(nameof(TestRunOperationVoid), getContext(), (operation) =>
                {
                    operation.TelemetryData["test"] = "Ek588Agn3kM";
                    return "foo";
                });

                harness.AssertDataAreEqual("OPB/DRz430M", 1, 4);
            }
        }

        [TestMethod]
        public void TestWithContextRunOperationReturn_ExceptionalOperationNameNullEmptyAndWhitespace()
        {
            using (TraceListenerHarness harness = new TraceListenerHarness(Source))
            {
                var log = Get();

                AssertThrow<ArgumentNullException>(() =>
                {
                    string foo = log.Run(null, (operation) =>
                    {
                        return "foo";
                    });
                });

                AssertThrow<ArgumentNullException>(() =>
                {
                    string bar = log.Run("", (operation) =>
                    {
                        return "bar";
                    });
                });

                AssertThrow<ArgumentNullException>(() =>
                {
                    string foobar = log.Run("                                             ", (operation) =>
                    {
                        return "foobar";
                    });
                });
            }
        }

        [TestMethod]
        public void TestWithContextRunOperationReturn_ExceptionNoDelegate()
        {
            using (TraceListenerHarness harness = new TraceListenerHarness(Source))
            {
                var log = Get();

                AssertThrow<ArgumentNullException>(() =>
                {
                    string foo = log.Run<string, byte[]>(nameof(TestRunOperationVoid), null);
                });
            }
        }

        [TestMethod]
        public void TestWithContextRunOperationReturn_EnsureInternalException()
        {
            using (TraceListenerHarness harness = new TraceListenerHarness(Source))
            {
                var log = Get();

                AssertThrow<FakeException>(() =>
                {
                    string foo = log.Run(nameof(TestRunOperationVoid), getContext(), (operation) =>
                    {
                        throw new FakeException("AAAAAAAAAh");
                        return "foo";
                    });
                });
                //2

                // 3 because of exception.
                harness.AssertDataAreEqual("OPB/DRz430M", 1, 3);
            }
        }

        [TestMethod]
        public void TestWithContextRunOperationReturn_FailureSetInternal()
        {
            using (TraceListenerHarness harness = new TraceListenerHarness(Source))
            {
                var log = Get();

                string foo = log.Run(nameof(TestRunOperationVoid), getContext(), (operation) =>
                {
                    operation.SetOperationResult(OperationResult.Failed);
                    return "foo";
                });

                harness.AssertDataContains("P1Wtb5+q1kM", "failed", false);
            }
        }

        #endregion

        #region RunVoidAsyncWithContext
        [TestMethod]
        public async Task TestWithContextAsyncRunOperationVoid()
        {
            using (TraceListenerHarness harness = new TraceListenerHarness(Source))
            {
                var log = Get();

                await log.RunAsync(nameof(TestRunOperationVoid), getContext(), async (operation) =>
                {
                    await Task.Yield();
                });

                harness.AssertDataAreEqual("OPB/DRz430M", 1, 3);
            }
        }

        [TestMethod]
        public async Task TestWithContextAsyncRunOperationVoidWithData()
        {
            using (TraceListenerHarness harness = new TraceListenerHarness(Source))
            {
                var log = Get();

                await log.RunAsync(nameof(TestRunOperationVoid), getContext(), async (operation) =>
                {
                    await Task.Yield();
                    operation.TelemetryData["test"] = "Ek588Agn3kM";
                });

                harness.AssertDataAreEqual("OPB/DRz430M", 1, 4);
            }
        }

        [TestMethod]
        public async Task TestWithContextAsyncRunOperationVoid_ExceptionalOperationNameNullEmptyAndWhitespace()
        {
            using (TraceListenerHarness harness = new TraceListenerHarness(Source))
            {
                var log = Get();

                await AssertThrowAsync<ArgumentNullException>(async () =>
                {
                    await log.RunAsync(null, async (operation) =>
                    {
                        await Task.Yield();
                    });
                });

                await AssertThrowAsync<ArgumentNullException>(async () =>
                {
                    await log.RunAsync("", async (operation) =>
                    {
                        await Task.Yield();
                    });
                });

                await AssertThrowAsync<ArgumentNullException>(async () =>
                {
                    await log.RunAsync("                                             ", async (operation) =>
                    {
                        await Task.Yield();
                    });
                });
            }
        }

        [TestMethod]
        public async Task TestWithContextAsyncRunOperationVoid_ExceptionNoDelegate()
        {
            using (TraceListenerHarness harness = new TraceListenerHarness(Source))
            {
                var log = Get();

                await AssertThrowAsync<ArgumentNullException>(async () => await log.RunAsync(nameof(TestRunOperationVoid), getContext(), action: null));
            }
        }

        [TestMethod]
        public async Task TestWithContextAsyncRunOperationVoid_EnsureInternalException()
        {
            using (TraceListenerHarness harness = new TraceListenerHarness(Source))
            {
                var log = Get();

                await AssertThrowAsync<FakeException>(async () =>
                {
                    await log.RunAsync(nameof(TestRunOperationVoid), getContext(), async (operation) =>
                    {
                        await Task.Yield();
                        throw new FakeException("AAAAAAAAAh");
                    });
                });
                //2

                // 3 because of exception.
                harness.AssertDataAreEqual("OPB/DRz430M", 1, 3);
            }
        }

        [TestMethod]
        public async Task TestWithContextAsyncRunOperationVoid_FailureSetInternal()
        {
            using (TraceListenerHarness harness = new TraceListenerHarness(Source))
            {
                var log = Get();

                await log.RunAsync(nameof(TestRunOperationVoid), getContext(), async (operation) =>
                {
                    await Task.Yield();
                    operation.SetOperationResult(OperationResult.Failed);
                });

                harness.AssertDataContains("19j8r2ts3kM", "failed", false);
            }
        }

        #endregion

        #region RunReturnAsyncWithContext
        [TestMethod]
        public async Task TestWithContextAsyncRunOperationReturn()
        {
            using (TraceListenerHarness harness = new TraceListenerHarness(Source))
            {
                var log = Get();

                string foo = await log.RunAsync(nameof(TestRunOperationVoid), getContext(), async (operation) =>
                {
                    await Task.Yield();
                    return "foo";
                });

                harness.AssertDataAreEqual("OPB/DRz430M", 1, 3);
            }
        }

        [TestMethod]
        public async Task TestWithContextAsyncRunOperationReturnWithData()
        {
            using (TraceListenerHarness harness = new TraceListenerHarness(Source))
            {
                var log = Get();

                string foo = await log.RunAsync(nameof(TestRunOperationVoid), getContext(), async (operation) =>
                {
                    await Task.Yield();
                    operation.TelemetryData["test"] = "Ek588Agn3kM";
                    return "foo";
                });

                harness.AssertDataAreEqual("OPB/DRz430M", 1, 4);
            }
        }

        [TestMethod]
        public async Task TestWithContextAsyncRunOperationReturn_ExceptionalOperationNameNullEmptyAndWhitespace()
        {
            using (TraceListenerHarness harness = new TraceListenerHarness(Source))
            {
                var log = Get();

                await AssertThrowAsync<ArgumentNullException>(async () =>
                {
                    string foo = await log.RunAsync(null, async (operation) =>
                    {
                        await Task.Yield();
                        return "foo";
                    });
                });

                await AssertThrowAsync<ArgumentNullException>(async () =>
                {
                    string bar = await log.RunAsync("", async (operation) =>
                    {
                        await Task.Yield();
                        return "bar";
                    });
                });

                await AssertThrowAsync<ArgumentNullException>(async () =>
                {
                    string foobar = await log.RunAsync("                                             ", async (operation) =>
                    {
                        await Task.Yield();
                        return "foobar";
                    });
                });
            }
        }

        [TestMethod]
        public async Task TestWithContextAsyncRunOperationReturn_ExceptionNoDelegate()
        {
            using (TraceListenerHarness harness = new TraceListenerHarness(Source))
            {
                var log = Get();

                await AssertThrowAsync<ArgumentNullException>(async () =>
                {
                    await Task.Yield();
                    string foo = await log.RunAsync<string, byte[]>(nameof(TestRunOperationVoid), null);
                });
            }
        }

        [TestMethod]
        public async Task TestWithContextAsyncRunOperationReturn_EnsureInternalException()
        {
            using (TraceListenerHarness harness = new TraceListenerHarness(Source))
            {
                var log = Get();

                await AssertThrowAsync<FakeException>(async () =>
                {
                    string foo = await log.RunAsync(nameof(TestRunOperationVoid), getContext(), async (operation) =>
                    {
                        await Task.Yield();
                        throw new FakeException("AAAAAAAAAh");
                        return "foo";
                    });
                });
                //2

                // 3 because of exception.
                harness.AssertDataAreEqual("OPB/DRz430M", 1, 3);
            }
        }

        [TestMethod]
        public async Task TestWithContextAsyncRunOperationReturn_FailureSetInternal()
        {
            using (TraceListenerHarness harness = new TraceListenerHarness(Source))
            {
                var log = Get();

                string foo = await log.RunAsync(nameof(TestRunOperationVoid), getContext(), async (operation) =>
                {
                    await Task.Yield();
                    operation.SetOperationResult(OperationResult.Failed);
                    return "foo";
                });

                harness.AssertDataContains("sB0y9NcO2UM", "failed", false);
            }
        }

        #endregion

    }
}
