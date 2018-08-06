namespace PinkHair.Common.Telemetron
{
    using System;
    using System.Collections.Generic;
    using System.Runtime.CompilerServices;
    using System.Text;
    using System.Threading.Tasks;

    public static class OperationsExtensions
    {
        /// <summary>
        /// Runs a delegate in the context of an operation.
        /// </summary>
        /// <typeparam name="TCorrelationContext">The type of correlation context that will be used by the provider.</typeparam>
        /// <param name="provider">The Operation Provider that will create operations.</param>
        /// <param name="operationName">The name of the operation that will be created.</param>
        /// <param name="action">The delegate Action</param>
        public static void Run(this IOperationProvider provider, string operationName, RunOperationDelegate action)
        {
            if (provider == null)
            {
                throw new ArgumentNullException(nameof(provider));
            }

            if (string.IsNullOrWhiteSpace(operationName))
            {
                throw new ArgumentNullException(nameof(operationName));
            }

            if (action == null)
            {
                throw new ArgumentNullException(nameof(action));
            }

            using (IOperation createdOperation = provider.CreateOperation(operationName))
            {
                try
                {
                    action(createdOperation);
                    createdOperation.SetOperationResult(OperationResult.Success);
                }
                catch (Exception ex)
                {
                    createdOperation.SetOperationResult(ex);
                    System.Runtime.ExceptionServices.ExceptionDispatchInfo.Capture(ex).Throw();
                    throw ex;
                }
            }
        }

        /// <summary>
        /// Runs a delegate in the context of an operation.
        /// </summary>
        /// <typeparam name="TCorrelationContext">The type of correlation context that will be used by the provider.</typeparam>
        /// <param name="provider">The Operation Provider that will create operations.</param>
        /// <param name="operationName">The name of the operation that will be created.</param>
        /// <param name="ctx">The correlation context to restore for this operation.</param>
        /// <param name="action">The delegate Action</param>
        public static void Run<TCorrelationContext>(this IOperationProvider<TCorrelationContext> provider, string operationName, TCorrelationContext ctx, RunOperationDelegate action)
        {
            if (provider == null)
            {
                throw new ArgumentNullException(nameof(provider));
            }

            if (string.IsNullOrWhiteSpace(operationName))
            {
                throw new ArgumentNullException(nameof(operationName));
            }

            if (action == null)
            {
                throw new ArgumentNullException(nameof(action));
            }

            using (IOperation createdOperation = provider.CreateOperation(operationName, ctx))
            {
                try
                {
                    action(createdOperation);
                    createdOperation.SetOperationResult(OperationResult.Success);

                }
                catch (Exception ex)
                {
                    createdOperation.SetOperationResult(ex);
                    System.Runtime.ExceptionServices.ExceptionDispatchInfo.Capture(ex).Throw();
                    throw ex;
                }
            }
        }

        /// <summary>
        /// Runs a delegate in the context of an operation.
        /// </summary>
        /// <typeparam name="T">The return type from the function.</typeparam>
        /// <typeparam name="TCorrelationContext">The type of correlation context that will be used by the provider.</typeparam>
        /// <param name="provider">The Operation Provider that will create operations.</param>
        /// <param name="operationName">The name of the operation that will be created.</param>
        /// <param name="function">The delegate Action</param>
        public static T Run<T>(this IOperationProvider provider, string operationName, RunOperationDelegate<T> function)
        {
            if (provider == null)
            {
                throw new ArgumentNullException(nameof(provider));
            }

            if (string.IsNullOrWhiteSpace(operationName))
            {
                throw new ArgumentNullException(nameof(operationName));
            }

            if (function == null)
            {
                throw new ArgumentNullException(nameof(function));
            }

            using (IOperation createdOperation = provider.CreateOperation(operationName))
            {
                try
                {
                    T result = function(createdOperation);
                    createdOperation.SetOperationResult(OperationResult.Success);
                    return result;
                }
                catch (Exception ex)
                {
                    createdOperation.SetOperationResult(ex);
                    System.Runtime.ExceptionServices.ExceptionDispatchInfo.Capture(ex).Throw();
                    throw ex;
                }
            }
        }

        /// <summary>
        /// Runs a delegate in the context of an operation.
        /// </summary>
        /// <typeparam name="T">The return type from the function.</typeparam>
        /// <typeparam name="TCorrelationContext">The type of correlation context that will be used by the provider.</typeparam>
        /// <param name="provider">The Operation Provider that will create operations.</param>
        /// <param name="operationName">The name of the operation that will be created.</param>
        /// <param name="ctx">The correlation context to restore for this operation.</param>
        /// <param name="function">The delegate Action</param>
        public static T Run<T, TCorrelationContext>(this IOperationProvider<TCorrelationContext> provider, string operationName, TCorrelationContext ctx, RunOperationDelegate<T> function)
        {
            if (provider == null)
            {
                throw new ArgumentNullException(nameof(provider));
            }

            if (string.IsNullOrWhiteSpace(operationName))
            {
                throw new ArgumentNullException(nameof(operationName));
            }

            if (function == null)
            {
                throw new ArgumentNullException(nameof(function));
            }

            using (IOperation createdOperation = provider.CreateOperation(operationName, ctx))
            {
                try
                {
                    T result = function(createdOperation);
                    createdOperation.SetOperationResult(OperationResult.Success);
                    return result;

                }
                catch (Exception ex)
                {
                    createdOperation.SetOperationResult(ex);
                    System.Runtime.ExceptionServices.ExceptionDispatchInfo.Capture(ex).Throw();
                    throw ex;
                }
            }
        }

        /// <summary>
        /// Runs a delegate in the context of an operation.
        /// </summary>
        /// <typeparam name="TCorrelationContext">The type of correlation context that will be used by the provider.</typeparam>
        /// <param name="provider">The Operation Provider that will create operations.</param>
        /// <param name="operationName">The name of the operation that will be created.</param>
        /// <param name="action">The delegate Action</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        public static async Task RunAsync(this IOperationProvider provider, string operationName, RunOperationAsyncDelegate action)
        {
            if (provider == null)
            {
                throw new ArgumentNullException(nameof(provider));
            }

            if (string.IsNullOrWhiteSpace(operationName))
            {
                throw new ArgumentNullException(nameof(operationName));
            }

            if (action == null)
            {
                throw new ArgumentNullException(nameof(action));
            }

            using (IOperation createdOperation = provider.CreateOperation(operationName))
            {
                try
                {
                    await action(createdOperation);
                    createdOperation.SetOperationResult(OperationResult.Success);
                }
                catch (Exception ex)
                {
                    createdOperation.SetOperationResult(ex);
                    System.Runtime.ExceptionServices.ExceptionDispatchInfo.Capture(ex).Throw();
                    throw ex;
                }
            }
        }

        /// <summary>
        /// Runs a delegate in the context of an operation.
        /// </summary>
        /// <typeparam name="TCorrelationContext">The type of correlation context that will be used by the provider.</typeparam>
        /// <param name="provider">The Operation Provider that will create operations.</param>
        /// <param name="operationName">The name of the operation that will be created.</param>
        /// <param name="ctx">The correlation context to restore for this operation.</param>
        /// <param name="action">The delegate Action</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        public static async Task RunAsync<TCorrelationContext>(this IOperationProvider<TCorrelationContext> provider, string operationName, TCorrelationContext ctx, RunOperationAsyncDelegate action)
        {
            if (provider == null)
            {
                throw new ArgumentNullException(nameof(provider));
            }

            if (string.IsNullOrWhiteSpace(operationName))
            {
                throw new ArgumentNullException(nameof(operationName));
            }

            if (action == null)
            {
                throw new ArgumentNullException(nameof(action));
            }

            using (IOperation createdOperation = provider.CreateOperation(operationName, ctx))
            {
                try
                {
                    await action(createdOperation);
                    createdOperation.SetOperationResult(OperationResult.Success);
                }
                catch (Exception ex)
                {
                    createdOperation.SetOperationResult(ex);
                    System.Runtime.ExceptionServices.ExceptionDispatchInfo.Capture(ex).Throw();
                    throw ex;
                }
            }
        }

        /// <summary>
        /// Runs a delegate in the context of an operation.
        /// </summary>
        /// <typeparam name="T">The return type from the function.</typeparam>
        /// <typeparam name="TCorrelationContext">The type of correlation context that will be used by the provider.</typeparam>
        /// <param name="provider">The Operation Provider that will create operations.</param>
        /// <param name="operationName">The name of the operation that will be created.</param>
        /// <param name="function">The delegate Action</param>
        /// <returns>The result of the async operation, awaited.</returns>
        public static async Task<T> RunAsync<T>(this IOperationProvider provider, string operationName, RunOperationAsyncDelegate<T> function)
        {
            if (provider == null)
            {
                throw new ArgumentNullException(nameof(provider));
            }

            if (string.IsNullOrWhiteSpace(operationName))
            {
                throw new ArgumentNullException(nameof(operationName));
            }

            if (function == null)
            {
                throw new ArgumentNullException(nameof(function));
            }

            using (IOperation createdOperation = provider.CreateOperation(operationName))
            {
                try
                {
                    T result = await function(createdOperation);
                    createdOperation.SetOperationResult(OperationResult.Success);
                    return result;
                }
                catch (Exception ex)
                {
                    createdOperation.SetOperationResult(ex);
                    System.Runtime.ExceptionServices.ExceptionDispatchInfo.Capture(ex).Throw();
                    throw ex;
                }
            }
        }

        /// <summary>
        /// Runs a delegate in the context of an operation.
        /// </summary>
        /// <typeparam name="T">The return type from the function.</typeparam>
        /// <typeparam name="TCorrelationContext">The type of correlation context that will be used by the provider.</typeparam>
        /// <param name="provider">The Operation Provider that will create operations.</param>
        /// <param name="operationName">The name of the operation that will be created.</param>
        /// <param name="ctx">The correlation context to restore for this operation.</param>
        /// <param name="function">The delegate Action</param>
        /// <returns>The result of the async operation, awaited.</returns>
        public static async Task<T> RunAsync<T, TCorrelationContext>(this IOperationProvider<TCorrelationContext> provider, string operationName, TCorrelationContext ctx, RunOperationAsyncDelegate<T> function)
        {
            if (provider == null)
            {
                throw new ArgumentNullException(nameof(provider));
            }

            if (string.IsNullOrWhiteSpace(operationName))
            {
                throw new ArgumentNullException(nameof(operationName));
            }

            if (function == null)
            {
                throw new ArgumentNullException(nameof(function));
            }

            using (IOperation createdOperation = provider.CreateOperation(operationName, ctx))
            {
                try
                {
                    T result = await function(createdOperation);
                    createdOperation.SetOperationResult(OperationResult.Success);
                    return result;
                }
                catch (Exception ex)
                {
                    createdOperation.SetOperationResult(ex);
                    System.Runtime.ExceptionServices.ExceptionDispatchInfo.Capture(ex).Throw();
                    throw ex;
                }
            }
        }

        /// <summary>
        /// Runs a delegate in the context of an operation.
        /// </summary>
        /// <typeparam name="TCorrelationContext">The type of correlation context that will be used by the provider.</typeparam>
        /// <param name="provider">The Operation Provider that will create operations.</param>
        /// <param name="action">The delegate Action</param>
        /// <param name="callerMemberName">The name of the calling method that will be used as the name of the operation..</param>
        public static void Run(
            this IOperationProvider provider, 
            Action action, 
            [CallerMemberName]string callerMemberName = "") =>
                Run(provider, callerMemberName, (operation) => action());

        /// <summary>
        /// Runs a delegate in the context of an operation.
        /// </summary>
        /// <typeparam name="TCorrelationContext">The type of correlation context that will be used by the provider.</typeparam>
        /// <param name="provider">The Operation Provider that will create operations.</param>
        /// <param name="ctx">The correlation context to restore for this operation.</param>
        /// <param name="action">The delegate Action</param>
        /// <param name="callerMemberName">The name of the calling method that will be used as the name of the operation..</param>
        public static void Run<TCorrelationContext>(this IOperationProvider<TCorrelationContext> provider, TCorrelationContext ctx, Action action, [CallerMemberName]string callerMemberName = "") =>
            Run(provider, callerMemberName, ctx, (operation) => action());

        /// <summary>
        /// Runs a delegate in the context of an operation.
        /// </summary>
        /// <typeparam name="T">The return type from the function.</typeparam>
        /// <typeparam name="TCorrelationContext">The type of correlation context that will be used by the provider.</typeparam>
        /// <param name="provider">The Operation Provider that will create operations.</param>
        /// <param name="function">The delegate Action</param>
        /// <param name="callerMemberName">The name of the calling method that will be used as the name of the operation..</param>
        public static T Run<T>(
            this IOperationProvider provider, 
            Func<T> function, 
            [CallerMemberName]string callerMemberName = "") =>
                Run(provider, callerMemberName, (operation) => function());

        /// <summary>
        /// Runs a delegate in the context of an operation.
        /// </summary>
        /// <typeparam name="T">The return type from the function.</typeparam>
        /// <typeparam name="TCorrelationContext">The type of correlation context that will be used by the provider.</typeparam>
        /// <param name="provider">The Operation Provider that will create operations.</param>
        /// <param name="ctx">The correlation context to restore for this operation.</param>
        /// <param name="function">The delegate Action</param>
        /// <param name="callerMemberName">The name of the calling method that will be used as the name of the operation..</param>
        public static T Run<T, TCorrelationContext>(this IOperationProvider<TCorrelationContext> provider,TCorrelationContext ctx, Func<T> function, [CallerMemberName]string callerMemberName = "") =>
                Run(provider, callerMemberName, ctx, (operation) => function());

        /// <summary>
        /// Runs a delegate in the context of an operation.
        /// </summary>
        /// <typeparam name="TCorrelationContext">The type of correlation context that will be used by the provider.</typeparam>
        /// <param name="provider">The Operation Provider that will create operations.</param>
        /// <param name="action">The delegate Action</param>
        /// <param name="callerMemberName">The name of the calling method that will be used as the name of the operation..</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        public static async Task RunAsync(
            this IOperationProvider provider, 
            Func<Task> action, 
            [CallerMemberName]string callerMemberName = "") =>
                await RunAsync(provider, callerMemberName, (operation) => action());

        /// <summary>
        /// Runs a delegate in the context of an operation.
        /// </summary>
        /// <typeparam name="TCorrelationContext">The type of correlation context that will be used by the provider.</typeparam>
        /// <param name="provider">The Operation Provider that will create operations.</param>
        /// <param name="ctx">The correlation context to restore for this operation.</param>
        /// <param name="action">The delegate Action</param>
        /// <param name="callerMemberName">The name of the calling method that will be used as the name of the operation..</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
        public static async Task RunAsync<TCorrelationContext>(
            this IOperationProvider<TCorrelationContext> provider,  
            TCorrelationContext ctx, 
            Func<Task> action, 
            [CallerMemberName]string callerMemberName = "") =>
                await RunAsync(provider, callerMemberName, ctx, (operation) => action());


        /// <summary>
        /// Runs a delegate in the context of an operation.
        /// </summary>
        /// <typeparam name="T">The return type from the function.</typeparam>
        /// <typeparam name="TCorrelationContext">The type of correlation context that will be used by the provider.</typeparam>
        /// <param name="provider">The Operation Provider that will create operations.</param>
        /// <param name="function">The delegate Action</param>
        /// <param name="callerMemberName">The name of the calling method that will be used as the name of the operation..</param>
        /// <returns>The result of the async operation, awaited.</returns>
        public static async Task<T> RunAsync<T>(this IOperationProvider provider, Func<Task<T>> function, [CallerMemberName]string callerMemberName = "") =>
                await RunAsync(provider, callerMemberName, (operation) => function());


        /// <summary>
        /// Runs a delegate in the context of an operation.
        /// </summary>
        /// <typeparam name="T">The return type from the function.</typeparam>
        /// <typeparam name="TCorrelationContext">The type of correlation context that will be used by the provider.</typeparam>
        /// <param name="provider">The Operation Provider that will create operations.</param>
        /// <param name="ctx">The correlation context to restore for this operation.</param>
        /// <param name="function">The delegate Action</param>
        /// <param name="callerMemberName">The name of the calling method that will be used as the name of the operation..</param>
        /// <returns>The result of the async operation, awaited.</returns>
        public static async Task<T> RunAsync<T, TCorrelationContext>(
            this IOperationProvider<TCorrelationContext> provider, 
            TCorrelationContext ctx, 
            Func<Task<T>> function, 
            [CallerMemberName]string callerMemberName = "") =>
                await RunAsync(provider, callerMemberName, ctx, (operation) => function());

    }
}