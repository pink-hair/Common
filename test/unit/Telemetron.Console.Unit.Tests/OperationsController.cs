using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text;

namespace Telemetron.Console.Unit.Tests
{
    using PinkHair.Common.Telemetron;
    using static global::PinkHair.Common.Telemetron.Diagnostics.DiagnosticTrace;
    using static Common;
    using System.Linq;

    [TestClass]
    [ExcludeFromCodeCoverage]
    public class OperationsController
    {
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

        [TestMethod]
        public void TestRunOperationVoid()
        {
            using (TraceListenerHarness harness = new TraceListenerHarness(Source))
            {
                var log = Get();

                log.Run(nameof(TestRunOperationVoid), (operation) =>
                {

                });
                //2

                harness.AssertDataAreEqual("OPB/DRz430M", 1, 4);
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
                //2

                harness.AssertDataAreEqual("OPB/DRz430M", 1, 4);
            }
        }

        [TestMethod]
        public void TestRunOperationReturn()
        {
            using (TraceListenerHarness harness = new TraceListenerHarness(Source))
            {
                var log = Get();

                string foo = log.Run(nameof(TestRunOperationVoid), (operation) =>
                {
                    operation.TelemetryData["test"] = "Ek588Agn3kM";
                    return "Ek588Agn3kM";
                });
                //2

                harness.AssertDataAreEqual("OPB/DRz430M", 1, 4);
            }
        }

    }
}
