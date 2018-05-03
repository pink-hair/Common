using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;

namespace Polytech.Common.Telemetron
{
    public interface IContextualTestClass
    {
        TestContext TestContext {get;set;}
}
}
