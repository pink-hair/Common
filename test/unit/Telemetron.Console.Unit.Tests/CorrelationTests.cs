using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using Polytech.Common.Telemetron;

namespace Telemetron.Console.Unit.Tests
{
    using static global::Telemetron.Console.Unit.Tests.Common;
    [TestClass]
    public class CorrelationTests
    {
        [TestMethod]
        public void TestBasicEmission()
        {
            

            ConsoleTelemetron ct = new ConsoleTelemetron(CreateDefaultConfiguration());

            List<ConsoleEvent> ces = new List<ConsoleEvent>();
            ct.EventEnqueued += (sender, args) =>
            {
                ces.Add(args);
            };

            ct.Info("foo");
            ct.Info("bar");

            Assert.AreEqual(2, ces.Count);
        }



        [TestMethod]
        public void TestOperationWithCorrenationContext_Depth_2()
        {
            ConsoleTelemetron ct = new ConsoleTelemetron(CreateDefaultConfiguration());

            List<ConsoleEvent> ces = new List<ConsoleEvent>();
            ct.EventEnqueued += (sender, args) =>
            {
                ces.Add(args);
            };

            ct.Info("foo");
            ct.Info("bar");

            string operationId;
            using (IOperation op = ct.CreateOperation("foo"))
            {
                operationId = op.OperationId;

                ct.Info("whee");
            }

            Assert.AreEqual(6, ces.Count);

            var maxDepth = GetMaxCorrelationContextDepth(ces);

            Assert.AreEqual(5, maxDepth[1]);
            Assert.AreEqual(1, maxDepth[2]);

            // events can arrive out of order 
        }

        [TestMethod]
        public void TestOperationWithCorrenationContext_Depth_3()
        {
            ConsoleTelemetron ct = new ConsoleTelemetron(CreateDefaultConfiguration());

            List<ConsoleEvent> ces = new List<ConsoleEvent>();
            ct.EventEnqueued += (sender, args) =>
            {
                // get rid of metrics
                if (args.EventSeverity == EventSeverity.Metric || args.EventSeverity == EventSeverity.Event)
                {
                    return;
                }

                ces.Add(args);
            };

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

            Assert.AreEqual(6, ces.Count);

            var maxDepth = GetMaxCorrelationContextDepth(ces);

            Assert.AreEqual(3, maxDepth[1]);
            Assert.AreEqual(2, maxDepth[2]);
            Assert.AreEqual(1, maxDepth[3]);
        }

        private static Dictionary<int, int> GetMaxCorrelationContextDepth(List<ConsoleEvent> ces)
        {
            Dictionary<int, int> result = new Dictionary<int, int>();
            foreach (ConsoleEvent evt in ces)
            {
                string[] parts = evt.CorrelationContext.Split('|');
                if (!result.ContainsKey(parts.Length))
                {
                    result[parts.Length] = 1;
                }
                else
                {
                    result[parts.Length] = result[parts.Length] + 1;
                }

            }

            return result;
        }
    }
}
