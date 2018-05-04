using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;
using Polytech.Common.Telemetron;

namespace Telemetron.Core.Unit.Tests
{
    using static Helpers;

    [TestClass]
    public class CorreationCoreTests
    {
        private const long RootValue = 1337L;
        private const long SecondOperationId = 3L;
        private const long ThirdOperationId = 238472834234728L;

        [TestMethod]
        public void TestCorrelationCoreRootCreation()
        {
            CorrelationContext cc = new CorrelationContext(RootValue);

            Assert.AreEqual(1, cc.CurrentOperationPosition.Position);
        }

        [TestMethod]
        public void TestCorrelationCoreAddOperationAndVerify()
        {
            CorrelationContext cc = new CorrelationContext(RootValue);

            long op1 = cc.AddOperation(SecondOperationId);
            long op2 = cc.AddOperation(ThirdOperationId);
            long op3 = cc.AddOperation(2342384793843948343L);

            Assert.AreEqual(4, cc.CurrentOperationPosition.Position);

            string correlationContext = cc.Get();
            string[] parts = correlationContext.Split('|');

            // Assert.AreEqual("OQUAAAAAAAA=", parts[0]); always random
            Assert.AreEqual("AwAAAAAAAAA=", parts[1]);
            Assert.AreEqual("aLmSyOPYAAA=", parts[2]);
            Assert.AreEqual("N1tYPI/SgSA=", parts[3]);
        }

        [TestMethod]
        public void TestCorrelationCoreAddOperationAndVerify_RNG()
        {
            CorrelationContext cc = new CorrelationContext(1337);

            long op1 = cc.AddOperation();
            long op2 = cc.AddOperation();
            long op3 = cc.AddOperation();

            Assert.AreEqual(4, cc.CurrentOperationPosition.Position);

            string correlationContext = cc.Get();
            string[] parts = correlationContext.Split('|');

            // Assert.AreEqual(Convert.ToBase64String(BitConverter.GetBytes(RootValue)), parts[0]); always random
            Assert.AreEqual(Convert.ToBase64String(BitConverter.GetBytes(op1)), parts[1]);
            Assert.AreEqual(Convert.ToBase64String(BitConverter.GetBytes(op2)), parts[2]);
            Assert.AreEqual(Convert.ToBase64String(BitConverter.GetBytes(op3)), parts[3]);
        }

        [TestMethod]
        public void TestCorrelationCoreAddOperationAndVerify_RNG_Remove()
        {
            CorrelationContext cc = new CorrelationContext(1337);

            long op1 = cc.AddOperation();
            long op2 = cc.AddOperation();
            long op3 = cc.AddOperation();
            long op4 = cc.AddOperation();

            Assert.AreEqual(5, cc.CurrentOperationPosition.Position);

            long removed = cc.RemoveOperation();
            Assert.AreEqual(op4, removed);

            Assert.AreEqual(4, cc.CurrentOperationPosition.Position);

            string correlationContext = cc.Get();
            string[] parts = correlationContext.Split('|');

            // Assert.AreEqual(Convert.ToBase64String(BitConverter.GetBytes(RootValue)), parts[0]); always random
            Assert.AreEqual(Convert.ToBase64String(BitConverter.GetBytes(op1)), parts[1]);
            Assert.AreEqual(Convert.ToBase64String(BitConverter.GetBytes(op2)), parts[2]);
            Assert.AreEqual(Convert.ToBase64String(BitConverter.GetBytes(op3)), parts[3]);
        }

        [TestMethod]
        public void TestCorrelationCoreRestoreCapturedContext()
        {
            byte[] manualContext = new byte[] {57, 5, 0, 0, 0, 0, 0, 0, 3, 0, 0, 0, 0, 0, 0, 0, 104, 185, 146, 200, 227, 216, 0, 0, 55, 91, 88, 60, 143, 210, 129, 32};
            CorrelationContext cc = new CorrelationContext(manualContext);

            string correlationContext = cc.Get();
            string[] parts = correlationContext.Split('|');

            Assert.AreEqual("OQUAAAAAAAA=", parts[0]);
            Assert.AreEqual("AwAAAAAAAAA=", parts[1]);
            Assert.AreEqual("aLmSyOPYAAA=", parts[2]);
            Assert.AreEqual("N1tYPI/SgSA=", parts[3]);
        }

        [TestMethod]
        public void TestCorrelationCoreRestoreCapturedContext_NULL()
        {
            byte[] manualContext = null;

            AssertThrow<ArgumentNullException>(() =>
            {
                CorrelationContext cc = new CorrelationContext(manualContext);
            });
        }

        [TestMethod]
        public void TestCorrelationCoreRestoreCapturedContext_Corrupt()
        {
            byte[] manualContext = new byte[] { 57, 5, 0, 0, 0, 0, 0, 0, 3, 0, 0, 0, 0, 0, 0, 0, 104, 185, 146, 200, 227, 216, 0, 0, 55, 91, 88, 60, 143, 210, 129, 32, 23 };

            CorrelationContext cc;
            AssertThrow<ArgumentException>(() =>
            {
                 cc = new CorrelationContext(manualContext);
            });
        }

        [TestMethod]
        public void TestCorrelationCoreRestoreCapturedContext_SizeMatters()
        {
            #region constant
            // This is just random copy paste garbage
            byte[] manualContext = new byte[] { 57, 5, 0, 0, 0, 0, 0, 0, 3, 0, 0, 0, 0, 0, 0, 0, 104, 185, 146, 200, 227, 216, 0, 0, 55, 91, 88, 60, 143, 210, 129, 32, 57, 5, 0, 0, 0, 0, 0, 0, 3, 0, 0, 0, 0, 0, 0, 0, 104, 185, 146, 200, 227, 216, 0, 0, 55, 91, 88, 60, 143, 210, 129, 32, 57, 5, 0, 0, 0, 0, 0, 0, 3, 0, 0, 0, 0, 0, 0, 0, 104, 185, 146, 200, 227, 216, 0, 0, 55, 91, 88, 60, 143, 210, 129, 32, 57, 5, 0, 0, 0, 0, 0, 0, 3, 0, 0, 0, 0, 0, 0, 0, 104, 185, 146, 200, 227, 216, 0, 0, 55, 91, 88, 60, 143, 210, 129, 32, 57, 5, 0, 0, 0, 0, 0, 0, 3, 0, 0, 0, 0, 0, 0, 0, 104, 185, 146, 200, 227, 216, 0, 0, 55, 91, 88, 60, 143, 210, 129, 32, 57, 5, 0, 0, 0, 0, 0, 0, 3, 0, 0, 0, 0, 0, 0, 0, 104, 185, 146, 200, 227, 216, 0, 0, 55, 91, 88, 60, 143, 210, 129, 32, 57, 5, 0, 0, 0, 0, 0, 0, 3, 0, 0, 0, 0, 0, 0, 0, 104, 185, 146, 200, 227, 216, 0, 0, 55, 91, 88, 60, 143, 210, 129, 32, 57, 5, 0, 0, 0, 0, 0, 0, 3, 0, 0, 0, 0, 0, 0, 0, 104, 185, 146, 200, 227, 216, 0, 0, 55, 91, 88, 60, 143, 210, 129, 32, 57, 5, 0, 0, 0, 0, 0, 0, 3, 0, 0, 0, 0, 0, 0, 0, 104, 185, 146, 200, 227, 216, 0, 0, 55, 91, 88, 60, 143, 210, 129, 32, 57, 5, 0, 0, 0, 0, 0, 0, 3, 0, 0, 0, 0, 0, 0, 0, 104, 185, 146, 200, 227, 216, 0, 0, 55, 91, 88, 60, 143, 210, 129, 32, 57, 5, 0, 0, 0, 0, 0, 0, 3, 0, 0, 0, 0, 0, 0, 0, 104, 185, 146, 200, 227, 216, 0, 0, 55, 91, 88, 60, 143, 210, 129, 32, 57, 5, 0, 0, 0, 0, 0, 0, 3, 0, 0, 0, 0, 0, 0, 0, 104, 185, 146, 200, 227, 216, 0, 0, 55, 91, 88, 60, 143, 210, 129, 32, 57, 5, 0, 0, 0, 0, 0, 0, 3, 0, 0, 0, 0, 0, 0, 0, 104, 185, 146, 200, 227, 216, 0, 0, 55, 91, 88, 60, 143, 210, 129, 32, 57, 5, 0, 0, 0, 0, 0, 0, 3, 0, 0, 0, 0, 0, 0, 0, 104, 185, 146, 200, 227, 216, 0, 0, 55, 91, 88, 60, 143, 210, 129, 32, 57, 5, 0, 0, 0, 0, 0, 0, 3, 0, 0, 0, 0, 0, 0, 0, 104, 185, 146, 200, 227, 216, 0, 0, 55, 91, 88, 60, 143, 210, 129, 32, 57, 5, 0, 0, 0, 0, 0, 0, 3, 0, 0, 0, 0, 0, 0, 0, 104, 185, 146, 200, 227, 216, 0, 0, 55, 91, 88, 60, 143, 210, 129, 32, 57, 5, 0, 0, 0, 0, 0, 0, 3, 0, 0, 0, 0, 0, 0, 0, 104, 185, 146, 200, 227, 216, 0, 0, 55, 91, 88, 60, 143, 210, 129, 32, 57, 5, 0, 0, 0, 0, 0, 0, 3, 0, 0, 0, 0, 0, 0, 0, 104, 185, 146, 200, 227, 216, 0, 0, 55, 91, 88, 60, 143, 210, 129, 32, 57, 5, 0, 0, 0, 0, 0, 0, 3, 0, 0, 0, 0, 0, 0, 0, 104, 185, 146, 200, 227, 216, 0, 0, 55, 91, 88, 60, 143, 210, 129, 32, 57, 5, 0, 0, 0, 0, 0, 0, 3, 0, 0, 0, 0, 0, 0, 0, 104, 185, 146, 200, 227, 216, 0, 0, 55, 91, 88, 60, 143, 210, 129, 32, 57, 5, 0, 0, 0, 0, 0, 0, 3, 0, 0, 0, 0, 0, 0, 0, 104, 185, 146, 200, 227, 216, 0, 0, 55, 91, 88, 60, 143, 210, 129, 32, 57, 5, 0, 0, 0, 0, 0, 0, 3, 0, 0, 0, 0, 0, 0, 0, 104, 185, 146, 200, 227, 216, 0, 0, 55, 91, 88, 60, 143, 210, 129, 32, 57, 5, 0, 0, 0, 0, 0, 0, 3, 0, 0, 0, 0, 0, 0, 0, 104, 185, 146, 200, 227, 216, 0, 0, 55, 91, 88, 60, 143, 210, 129, 32, 57, 5, 0, 0, 0, 0, 0, 0, 3, 0, 0, 0, 0, 0, 0, 0, 104, 185, 146, 200, 227, 216, 0, 0, 55, 91, 88, 60, 143, 210, 129, 32, 57, 5, 0, 0, 0, 0, 0, 0, 3, 0, 0, 0, 0, 0, 0, 0, 104, 185, 146, 200, 227, 216, 0, 0, 55, 91, 88, 60, 143, 210, 129, 32, 57, 5, 0, 0, 0, 0, 0, 0, 3, 0, 0, 0, 0, 0, 0, 0, 104, 185, 146, 200, 227, 216, 0, 0, 55, 91, 88, 60, 143, 210, 129, 32, 57, 5, 0, 0, 0, 0, 0, 0, 3, 0, 0, 0, 0, 0, 0, 0, 104, 185, 146, 200, 227, 216, 0, 0, 55, 91, 88, 60, 143, 210, 129, 32, 57, 5, 0, 0, 0, 0, 0, 0, 3, 0, 0, 0, 0, 0, 0, 0, 104, 185, 146, 200, 227, 216, 0, 0, 55, 91, 88, 60, 143, 210, 129, 32, 57, 5, 0, 0, 0, 0, 0, 0, 3, 0, 0, 0, 0, 0, 0, 0, 104, 185, 146, 200, 227, 216, 0, 0, 55, 91, 88, 60, 143, 210, 129, 32, 57, 5, 0, 0, 0, 0, 0, 0, 3, 0, 0, 0, 0, 0, 0, 0, 104, 185, 146, 200, 227, 216, 0, 0, 55, 91, 88, 60, 143, 210, 129, 32, 57, 5, 0, 0, 0, 0, 0, 0, 3, 0, 0, 0, 0, 0, 0, 0, 104, 185, 146, 200, 227, 216, 0, 0, 55, 91, 88, 60, 143, 210, 129, 32, 57, 5, 0, 0, 0, 0, 0, 0, 3, 0, 0, 0, 0, 0, 0, 0, 104, 185, 146, 200, 227, 216, 0, 0, 55, 91, 88, 60, 143, 210, 129, 32, 57, 5, 0, 0, 0, 0, 0, 0, 3, 0, 0, 0, 0, 0, 0, 0, 104, 185, 146, 200, 227, 216, 0, 0, 55, 91, 88, 60, 143, 210, 129, 32, 57, 5, 0, 0, 0, 0, 0, 0, 3, 0, 0, 0, 0, 0, 0, 0, 104, 185, 146, 200, 227, 216, 0, 0, 55, 91, 88, 60, 143, 210, 129, 32, 57, 5, 0, 0, 0, 0, 0, 0, 3, 0, 0, 0, 0, 0, 0, 0, 104, 185, 146, 200, 227, 216, 0, 0, 55, 91, 88, 60, 143, 210, 129, 32, 57, 5, 0, 0, 0, 0, 0, 0, 3, 0, 0, 0, 0, 0, 0, 0, 104, 185, 146, 200, 227, 216, 0, 0, 55, 91, 88, 60, 143, 210, 129, 32, 57, 5, 0, 0, 0, 0, 0, 0, 3, 0, 0, 0, 0, 0, 0, 0, 104, 185, 146, 200, 227, 216, 0, 0, 55, 91, 88, 60, 143, 210, 129, 32, 57, 5, 0, 0, 0, 0, 0, 0, 3, 0, 0, 0, 0, 0, 0, 0, 104, 185, 146, 200, 227, 216, 0, 0, 55, 91, 88, 60, 143, 210, 129, 32, 57, 5, 0, 0, 0, 0, 0, 0, 3, 0, 0, 0, 0, 0, 0, 0, 104, 185, 146, 200, 227, 216, 0, 0, 55, 91, 88, 60, 143, 210, 129, 32, 57, 5, 0, 0, 0, 0, 0, 0, 3, 0, 0, 0, 0, 0, 0, 0, 104, 185, 146, 200, 227, 216, 0, 0, 55, 91, 88, 60, 143, 210, 129, 32, 57, 5, 0, 0, 0, 0, 0, 0, 3, 0, 0, 0, 0, 0, 0, 0, 104, 185, 146, 200, 227, 216, 0, 0, 55, 91, 88, 60, 143, 210, 129, 32, 57, 5, 0, 0, 0, 0, 0, 0, 3, 0, 0, 0, 0, 0, 0, 0, 104, 185, 146, 200, 227, 216, 0, 0, 55, 91, 88, 60, 143, 210, 129, 32, 57, 5, 0, 0, 0, 0, 0, 0, 3, 0, 0, 0, 0, 0, 0, 0, 104, 185, 146, 200, 227, 216, 0, 0, 55, 91, 88, 60, 143, 210, 129, 32, 57, 5, 0, 0, 0, 0, 0, 0, 3, 0, 0, 0, 0, 0, 0, 0, 104, 185, 146, 200, 227, 216, 0, 0, 55, 91, 88, 60, 143, 210, 129, 32, 57, 5, 0, 0, 0, 0, 0, 0, 3, 0, 0, 0, 0, 0, 0, 0, 104, 185, 146, 200, 227, 216, 0, 0, 55, 91, 88, 60, 143, 210, 129, 32, 57, 5, 0, 0, 0, 0, 0, 0, 3, 0, 0, 0, 0, 0, 0, 0, 104, 185, 146, 200, 227, 216, 0, 0, 55, 91, 88, 60, 143, 210, 129, 32, 57, 5, 0, 0, 0, 0, 0, 0, 3, 0, 0, 0, 0, 0, 0, 0, 104, 185, 146, 200, 227, 216, 0, 0, 55, 91, 88, 60, 143, 210, 129, 32, 57, 5, 0, 0, 0, 0, 0, 0, 3, 0, 0, 0, 0, 0, 0, 0, 104, 185, 146, 200, 227, 216, 0, 0, 55, 91, 88, 60, 143, 210, 129, 32, 57, 5, 0, 0, 0, 0, 0, 0, 3, 0, 0, 0, 0, 0, 0, 0, 104, 185, 146, 200, 227, 216, 0, 0, 55, 91, 88, 60, 143, 210, 129, 32, 57, 5, 0, 0, 0, 0, 0, 0, 3, 0, 0, 0, 0, 0, 0, 0, 104, 185, 146, 200, 227, 216, 0, 0, 55, 91, 88, 60, 143, 210, 129, 32, 57, 5, 0, 0, 0, 0, 0, 0, 3, 0, 0, 0, 0, 0, 0, 0, 104, 185, 146, 200, 227, 216, 0, 0, 55, 91, 88, 60, 143, 210, 129, 32, 57, 5, 0, 0, 0, 0, 0, 0, 3, 0, 0, 0, 0, 0, 0, 0, 104, 185, 146, 200, 227, 216, 0, 0, 55, 91, 88, 60, 143, 210, 129, 32, 57, 5, 0, 0, 0, 0, 0, 0, 3, 0, 0, 0, 0, 0, 0, 0, 104, 185, 146, 200, 227, 216, 0, 0, 55, 91, 88, 60, 143, 210, 129, 32, 57, 5, 0, 0, 0, 0, 0, 0, 3, 0, 0, 0, 0, 0, 0, 0, 104, 185, 146, 200, 227, 216, 0, 0, 55, 91, 88, 60, 143, 210, 129, 32, 57, 5, 0, 0, 0, 0, 0, 0, 3, 0, 0, 0, 0, 0, 0, 0, 104, 185, 146, 200, 227, 216, 0, 0, 55, 91, 88, 60, 143, 210, 129, 32, 57, 5, 0, 0, 0, 0, 0, 0, 3, 0, 0, 0, 0, 0, 0, 0, 104, 185, 146, 200, 227, 216, 0, 0, 55, 91, 88, 60, 143, 210, 129, 32, 57, 5, 0, 0, 0, 0, 0, 0, 3, 0, 0, 0, 0, 0, 0, 0, 104, 185, 146, 200, 227, 216, 0, 0, 55, 91, 88, 60, 143, 210, 129, 32, 57, 5, 0, 0, 0, 0, 0, 0, 3, 0, 0, 0, 0, 0, 0, 0, 104, 185, 146, 200, 227, 216, 0, 0, 55, 91, 88, 60, 143, 210, 129, 32, 57, 5, 0, 0, 0, 0, 0, 0, 3, 0, 0, 0, 0, 0, 0, 0, 104, 185, 146, 200, 227, 216, 0, 0, 55, 91, 88, 60, 143, 210, 129, 32, 57, 5, 0, 0, 0, 0, 0, 0, 3, 0, 0, 0, 0, 0, 0, 0, 104, 185, 146, 200, 227, 216, 0, 0, 55, 91, 88, 60, 143, 210, 129, 32, 57, 5, 0, 0, 0, 0, 0, 0, 3, 0, 0, 0, 0, 0, 0, 0, 104, 185, 146, 200, 227, 216, 0, 0, 55, 91, 88, 60, 143, 210, 129, 32, 57, 5, 0, 0, 0, 0, 0, 0, 3, 0, 0, 0, 0, 0, 0, 0, 104, 185, 146, 200, 227, 216, 0, 0, 55, 91, 88, 60, 143, 210, 129, 32, 57, 5, 0, 0, 0, 0, 0, 0, 3, 0, 0, 0, 0, 0, 0, 0, 104, 185, 146, 200, 227, 216, 0, 0, 55, 91, 88, 60, 143, 210, 129, 32, 57, 5, 0, 0, 0, 0, 0, 0, 3, 0, 0, 0, 0, 0, 0, 0, 104, 185, 146, 200, 227, 216, 0, 0, 55, 91, 88, 60, 143, 210, 129, 32, };
            #endregion

            CorrelationContext cc;
            AssertThrow<ArgumentException>(() =>
            {
                cc = new CorrelationContext(manualContext);
            });
        }
    }
}
