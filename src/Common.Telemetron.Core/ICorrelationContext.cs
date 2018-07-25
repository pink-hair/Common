namespace PinkHair.Common.Telemetron
{
    /// <summary>
    /// 
    /// </summary>
    public interface ICorrelationContext
    {
        /// <summary>
        /// Gets the current Operation Id that is in use.
        /// </summary>
        long CurrentId { get; }

        /// <summary>
        /// Gets the Parent Operation that is in use.
        /// </summary>
        long ParentId { get; }

        /// <summary>
        /// Gets the RootId 
        /// </summary>
        long RootId { get; }

        /// <summary>
        /// Add an operation with a randomly generated new Id to the stack, and return the created Id.
        /// </summary>
        /// <returns>the New Id.</returns>
        long AddOperation();

        /// <summary>
        /// Add an operation with the specified Id to the stack.
        /// </summary>
        /// <param name="operationId">The Id to add.</param>
        /// <returns>The provided Id</returns>
        long AddOperation(long operationId);

        /// <summary>
        /// Capture the current operation Id Stack.
        /// </summary>
        /// <returns>The current byte array representing the Id Stack.</returns>
        byte[] Capture();

        /// <summary>
        /// Gets the current operation Id Stack in human readable form.
        /// </summary>
        /// <returns>The stack.</returns>
        string Get();

        /// <summary>
        /// Removes the current operation and returns its Id. 
        /// </summary>
        /// <returns>The Id of the operation returned.</returns>
        long RemoveOperation();
    }
}