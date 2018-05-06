using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Polytech.Common.Telemetron.ApplicationInsights.Tests
{
    using Microsoft.ApplicationInsights.Channel;
    using Polytech.Common.Telemetron.Core.Unit;
    using System;
    using System.Threading.Tasks;
    using static Common;
    using static Polytech.Common.Telemetron.Core.Unit.Helpers;
    using static Polytech.Common.Telemetron.Diagnostics.DiagnosticTrace;

    [TestClass]
    public class ApplicationInsightsTests
    {
        [TestMethod]
        public void CreateNoError()
        {
            var log = Get();

            // dont optimize out
            log.Info("foo");
        }

        [TestMethod]
        public async Task TestAllExtensionCoverageCore()
        {
            var log = Get();

            // refactored tests as hell.
            await log.TestTelemetronCore(() => new byte[] { 57, 5, 0, 0, 0, 0, 0, 0 }, "oOL8PlBx3kM", "c7eyVLlbyUM", "jYg0r0ZEykM");
        }

        [TestMethod]
        public void CreateNoConfig()
        {
            AssertThrow<ArgumentNullException>(() =>
            {
                ApplicationInsightsTelemetron at = new ApplicationInsightsTelemetron(null);
            });
        }

        [TestMethod]
        public void CreateMissingConfigKey()
        {
            var cfg = GetConfiguration();
            cfg.InstrumentationKey = null;
            AssertThrow<ArgumentNullException>(() =>
            {
                ApplicationInsightsTelemetron at = new ApplicationInsightsTelemetron(cfg);
            });
        }

        [TestMethod]
        public void TestExceptionalOperationCreation_ShouldReturnNullOperation()
        {
            ExceptionalApplicationInsightTelemetron eait = new ExceptionalApplicationInsightTelemetron(GetConfiguration());
            using (var harness = new TraceListenerHarness(Source))
            {
                using (IOperation nullOperation = eait.CreateOperation("foo"))
                {
                    Assert.IsInstanceOfType(nullOperation, typeof(NullOperation));
                }

                harness.AssertAny("FHnm64k800M");
            }
        }

        [TestMethod]
        public void TestExceptionalOperationCreation_ShouldReturnNullOperation_WithContext()
        {
            ExceptionalApplicationInsightTelemetron eait = new ExceptionalApplicationInsightTelemetron(GetConfiguration());

            using (var harness = new TraceListenerHarness(Source))
            {
                using (IOperation nullOperation = eait.CreateOperation("foo", new byte[] { 57, 5, 0, 0, 0, 0, 0, 0 }))
                {
                    Assert.IsInstanceOfType(nullOperation, typeof(NullOperation));
                }

                harness.AssertAny("7AKlKXaBwkM");
            }
        }

        [TestMethod]
        public void TestTelemetryObject_Normal()
        {
            ApplicationInsightsOperationTelemetry aiot = new ApplicationInsightsOperationTelemetry("operationName", "operationId", "correlationContext");

            aiot.CorrelationContext = "foo";

        }

        [TestMethod]
        public void TestTelemetryObject_Normal_NullProperty()
        {
            ApplicationInsightsOperationTelemetry aiot = new ApplicationInsightsOperationTelemetry("operationName", "operationId", "correlationContext");

            aiot.CorrelationContext = "foo";

            aiot.Properties.Remove(nameof(aiot.CorrelationContext));

            // would throw exception if direct.
            var ctx = aiot.CorrelationContext;

            Assert.IsNull(ctx);

        }

        [TestMethod]
        public void TestTelemetryObject_NullName()
        {
            AssertThrow<ArgumentNullException>("Expected operation name requirement", () =>
                {
                    ApplicationInsightsOperationTelemetry aiot = new ApplicationInsightsOperationTelemetry(
                        null, 
                        "operationId", 
                        "correlationContext");
                });

        }

        [TestMethod]
        public void TestTelemetryObject_NullId()
        {
            AssertThrow<ArgumentNullException>("Expected operation id requirement", () =>
            {
                ApplicationInsightsOperationTelemetry aiot = new ApplicationInsightsOperationTelemetry(
                    "OperationName",
                    null,
                    "correlationContext");
            });

        }

        [TestMethod]
        public void TestTelemetryObject_NullContext()
        {
            AssertThrow<ArgumentNullException>("Expected operation id requirement", () =>
            {
                ApplicationInsightsOperationTelemetry aiot = new ApplicationInsightsOperationTelemetry(
                    "OperationName",
                    "OperationId",
                    null);
            });

        }

        [TestMethod]
        public void TestTelemetryObject_DeepClone()
        {
            ApplicationInsightsOperationTelemetry aiot = new ApplicationInsightsOperationTelemetry("operationName", "operationId", "correlationContext");

            ITelemetry first = aiot;
            ITelemetry second = aiot.DeepClone();

            first.Context.Operation.Id = "Asdf";

            Assert.AreNotEqual(first.Context.Operation.Id, second.Context.Operation.Id);
        }
    }
}
