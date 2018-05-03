namespace Polytech.Common.Extension.Core
{
    /// <summary>
    /// Misc Extension Methods.
    /// </summary>
    public static partial class MiscExtensionMethods
    {
        /// <summary>
        /// Gets a safe (non-null) handle to this type. If the object being called is null,  a new one will be created.
        /// </summary>
        /// <typeparam name="T">The type of object that will attempted to be checked for null.</typeparam>
        /// <param name="provider">The object to check.</param>
        /// <returns>The object or a new instance of the type.</returns>
        public static T Safe<T>(this T provider)
            where T : new()
        {
            if (provider == null)
            {
                return new T();
            }

            return provider;
        }
    }
}
