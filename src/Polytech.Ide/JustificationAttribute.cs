namespace Polytech.Ide
{
    using System;

    /// <summary>
    /// A justification attribute for class or method level 'comments'
    /// </summary>
    [System.AttributeUsage(AttributeTargets.All, Inherited = false, AllowMultiple = true)]
    public sealed class JustificationAttribute : Attribute
    {
        private readonly string action;
        private readonly string justification;

        /// <summary>
        /// Initializes a new instance of the <see cref="JustificationAttribute"/> class.
        /// </summary>
        /// <param name="action">The justification for the action being taken.</param>
        /// <param name="justification">The action that was taken that requires a reason.</param>
        public JustificationAttribute(string action, string justification)
        {
            this.action = action;
            this.justification = justification;
        }

        /// <summary>
        /// Gets the justification for the action being taken.
        /// </summary>
        public string Justification => this.justification;

        /// <summary>
        /// Gets the action that was taken that requires a reason.
        /// </summary>
        public string Action => this.action;
    }
}
