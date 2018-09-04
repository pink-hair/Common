namespace PinkHair.Common
{
    using Microsoft.VisualStudio.TestTools.UnitTesting;

    /// <summary>
    /// Easy base class to implement <see cref="IContextualTestClass"/>
    /// </summary>
    public abstract class ContextAwareTestClass : IContextualTestClass
    {
        private TestContext context;

        /// <summary>
        /// Gets or sets the Context for the Unit Test Run.
        /// </summary>
        public TestContext TestContext
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
