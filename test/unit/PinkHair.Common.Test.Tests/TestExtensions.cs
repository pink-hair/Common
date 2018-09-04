using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace PinkHair.Common.Test.Tests
{
    using PinkHair.Common;
    using System;
    using System.Threading.Tasks;

    [TestClass]
    public class TestExtensions : ContextAwareTestClass
    {
        [TestMethod]
        public void Test_Success_RunExceptionalCode()
        {
            this.RunExpectingException<InvalidOperationException>(() =>
            {
                Console.WriteLine("foo");

                Action foo = () =>
                {
                    throw new InvalidOperationException();
                };

                Action bar = () =>
                {
                    foo();
                };

                bar();
            });
        }

        [TestMethod]
        public void Test_NonSuccess_RunExceptionalCode()
        {
            try
            {
                this.RunExpectingException<DivideByZeroException>(() =>
                {
                    Console.WriteLine("foo");

                    Action foo = () =>
                    {
                        throw new InvalidOperationException();
                    };

                    Action bar = () =>
                    {
                        foo();
                    };

                    bar();
                });

                Assert.Fail("Exception should break before here");
            }
            catch(InvalidOperationException)
            {
                // success
            }
        }

        [TestMethod]
        public async Task Test_Success_RunExceptionalCodeAsync()
        {
            await this.RunExpectingException<InvalidOperationException>(async () =>
            {
                await Task.Yield();

                Console.WriteLine("foo");

                Action foo = () =>
                {
                    throw new InvalidOperationException();
                };

                Action bar = () =>
                {
                    foo();
                };

                bar();
            });
        }

        [TestMethod]
        public async Task Test_NonSuccess_RunExceptionalCodeAsync()
        {
            try
            {
                await this.RunExpectingException<DivideByZeroException>(async () =>
                {
                    await Task.Yield();

                    Console.WriteLine("foo");

                    Action foo = () =>
                    {
                        throw new InvalidOperationException();
                    };

                    Action bar = () =>
                    {
                        foo();
                    };

                    bar();
                });

                Assert.Fail("Exception should break before here");
            }
            catch (InvalidOperationException)
            {
                // success
            }
        }
    }
}
