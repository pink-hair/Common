using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using Polytech.Common.Telemetron;

namespace Telemetron.Console.Unit.Tests
{
    using System.Diagnostics.CodeAnalysis;
    using static global::Telemetron.Console.Unit.Tests.Common;
    using static global::Polytech.Common.Telemetron.Diagnostics.DiagnosticTrace;
    using System.Linq;

    [ExcludeFromCodeCoverage]
    [TestClass]
    public class CorrelationTests
    {
        private static readonly char[] spaceSplit = new[] { ' ' };

        [TestMethod]
        public void TestBasicEmission()
        {
            

            ConsoleTelemetron ct = new ConsoleTelemetron(CreateDefaultConfiguration());

            using (TraceListenerHarness harness = new TraceListenerHarness(Source))
            {

                ct.Info("foo");
                ct.Info("bar");

                // we have to use GTE for counte as
                // we dont know how many additional events
                // (we do know how many, but coupling to that number is stupid).
                Assert.IsTrue(harness.Events.Count >= 2);
            }
        }



        [TestMethod]
        public void TestOperationWithCorrenationContext_Depth_2()
        {
            ConsoleTelemetron ct = new ConsoleTelemetron(CreateDefaultConfiguration());

            using (TraceListenerHarness harness = new TraceListenerHarness(Source))
            {
                ct.Info("foo");
                ct.Info("bar");

                string operationId;
                using (IOperation op = ct.CreateOperation("foo"))
                {
                    operationId = op.OperationId;

                    ct.Info("whee");
                }

                //Assert.AreEqual(6, ces.Count);

                var maxDepth = GetMaxCorrelationContextDepth(harness.Events);

                Assert.AreEqual(5, maxDepth[1]);
                Assert.AreEqual(2, maxDepth[2]);
            }

            // events can arrive out of order 
        }

        [TestMethod]
        public void TestOperationWithCorrenationContext_Depth_3()
        {
            ConsoleTelemetron ct = new ConsoleTelemetron(CreateDefaultConfiguration());

            using (TraceListenerHarness harness = new TraceListenerHarness(Source))
            {

                ct.Info("foo");
                ct.Info("bar");

                string operationIdFirst;
                string operationIdSecond;
                using (IOperation op = ct.CreateOperation("foo"))
                {
                    operationIdFirst = op.OperationId;

                    ct.Info("whee");

                    using (IOperation op2 = ct.CreateOperation("foo"))
                    {
                        operationIdSecond = op.OperationId;

                        ct.Info("whee");
                    }
                }

                var maxDepth = GetMaxCorrelationContextDepth(harness.Events);

                Assert.AreEqual(5, maxDepth[1]);
                Assert.AreEqual(5, maxDepth[2]);
                Assert.AreEqual(2, maxDepth[3]);
            }
        }

        private static Dictionary<int, int> GetMaxCorrelationContextDepth(IEnumerable<TraceEventEvent> tee)
        {
            // new collection.
            var ces = tee.Where(t => t.Actual.Contains("irE1MsXYqkM")).ToArray();

            Dictionary<int, int> result = new Dictionary<int, int>();
            foreach (TraceEventEvent evt in ces)
            {
                string[] parts = evt.Data.Split(spaceSplit, System.StringSplitOptions.RemoveEmptyEntries);
                if(parts.Length < 2)
                {
                    continue; 
                }

                int length = int.Parse(parts[0]);
                if (!result.ContainsKey(length))
                {
                    result[length] = 1;
                }
                else
                {
                    result[length] = result[length] + 1;
                }

            }

            return result;
        }
    }
}
