namespace PinkHair.Common
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    public interface IContextualTestClass
    {
        TestContext TestContext { get; set; }
    }
}
