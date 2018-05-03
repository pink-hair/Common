using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;

namespace Telemtron.Test
{
    public abstract class ContextAwareTestClass
    {
        private TestContext context;

        /// <summary>
        /// Test harness handle
        /// </summary>
        public TestContext Context
        {
            get
            {
                return this.context;
            }
            set
            {
                this.context = value;
            }
        }
    }
}
