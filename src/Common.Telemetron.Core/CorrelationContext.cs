namespace Polytech.Common.Telemetron
{
    using System;
    using System.Text;
    using System.Threading;
    using Common.Telemetron.Obscure.ValueTypes;

    /// <summary>
    /// A correlation context struct. Designed to be paired with <see cref="AsyncLocal{T}"/>.
    /// </summary>
    public unsafe struct CorrelationContext : ICorrelationContext
    {
        private const int OPERATIONSTACKSIZE = 64;
        private const char BAR = '|';
        private const int OPERATIONIDBYTELENGTH = 8;

        private static Random rng = new Random();

        public static Random RngProvider => rng;

        private fixed long operationStack[OPERATIONSTACKSIZE]; // 512 bytes not including overhead

        private long currentId;
        private long parentId;
        private long rootId;

        private CircularArrayPosition currentOperationPosition;

        /// <summary>
        /// Initializes a new instance of the <see cref="CorrelationContext"/> struct.
        /// </summary>
        /// <param name="rootSessionId">Creates a new Context with a seeded random first Id.</param>
        public CorrelationContext(long rootSessionId)
        {
            long sid;
            unchecked
            {
                sid = rootSessionId + rng.NextInt64();
            }

            this.currentOperationPosition = new CircularArrayPosition(OPERATIONSTACKSIZE);

            fixed (long* localStack = this.operationStack)
            {
                localStack[0] = sid;
            }

            this.currentOperationPosition++;
            this.currentId = rootSessionId;
            this.parentId = default(long);
            this.rootId = sid;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CorrelationContext"/> struct from the captured correlation context. This context may be from another thread, application domain, process, machine or even application. 
        /// </summary>
        /// <param name="capturedContext">The captured context to recreate the operation tree.</param>
        public CorrelationContext(byte[] capturedContext)
        {
            if (capturedContext == null)
            {
                throw new ArgumentNullException(nameof(capturedContext));
            }

            if (capturedContext.Length % OPERATIONIDBYTELENGTH != 0)
            {
                throw new ArgumentException("The captured context is corrupt, it must be a multiple of 8 to resolve operation Ids");
            }

            if (capturedContext.Length > OPERATIONSTACKSIZE * OPERATIONIDBYTELENGTH)
            {
                throw new ArgumentException("The context captured i corrupt and unusable, it is from a different runtime (Too big)");
            }

            this.currentOperationPosition = new CircularArrayPosition(OPERATIONSTACKSIZE);
            this.currentId = default(long);
            this.parentId = default(long);

            long operationId;
            long rid = BitConverter.ToInt64(capturedContext, 0);
            this.rootId = rid;

            fixed (long* localStack = this.operationStack)
            {
                for (int i = 0; i < capturedContext.Length; i += OPERATIONIDBYTELENGTH)
                {
                    operationId = BitConverter.ToInt64(capturedContext, i);

                    localStack[this.currentOperationPosition.Position] = operationId;
                    this.currentOperationPosition++;

                    // copy
                    CircularArrayPosition parentFinder = this.currentOperationPosition;

                    // if rolled over, we will have a parent.
                    if (parentFinder.TotalTraversal == 1)
                    {
                        this.parentId = -1;
                        this.currentId = localStack[0];
                    }
                    else if (parentFinder.TotalTraversal == 2)
                    {
                        this.currentId = localStack[1];
                        this.parentId = localStack[0];
                    }
                    else
                    {
                        // might have traversal and be at [0].
                        // need to use circular array counter;
                        parentFinder--;
                        this.currentId = localStack[parentFinder.Position];

                        parentFinder--;
                        this.parentId = localStack[parentFinder.Position];
                    }
                }
            }
        }

        /// <summary>
        /// Gets the current Operation Id that this <see cref="CorrelationContext"/> represents.
        /// </summary>
        public long CurrentId => this.currentId;

        /// <summary>
        /// Gets the Id of the parent Operation Id that this <see cref="CorrelationContext"/> represents.
        /// </summary>
        public long ParentId => this.parentId;

        /// <summary>
        /// Gets the Root ([0]) operation that was set when the correlation context. If the correlation context has traversed circularly, 
        /// </summary>
        public long RootId => this.rootId;

        /// <summary>
        /// Gets the current operation position circular array object.
        /// </summary>
        internal CircularArrayPosition CurrentOperationPosition => this.currentOperationPosition;

        /// <summary>
        /// Creates a new OperationId (RNG), adds it to the stack and returns it.
        /// </summary>
        /// <returns>The new Random Operation Id</returns>
        public long AddOperation()
        {
            long newOperationId = rng.NextInt64();

            this.AddOperationInternal(newOperationId);

            return newOperationId;
        }

        /// <summary>
        /// Adds the specified operation Id to the stack.
        /// </summary>
        /// <param name="operationId">The operation Id to Add.</param>
        /// <returns>the operation provided.</returns>
        public long AddOperation(long operationId)
        {
            this.AddOperationInternal(operationId);

            return operationId;
        }

        /// <summary>
        /// Removes the current operation Id from the stack and returns it.
        /// </summary>
        /// <returns>The Operation id that was removed.</returns>
        public long RemoveOperation()
        {
            long removedOperation;

            // we just decrement the position, max speed.
            this.currentOperationPosition--;

            fixed (long* localStack = this.operationStack)
            {
                removedOperation = localStack[this.currentOperationPosition.Position];

                // copy
                CircularArrayPosition cap = this.currentOperationPosition;

                // we already ahve a handle to -1, we need to get to -2
                cap--;
                this.currentId = localStack[cap.Position];

                cap--;
                this.parentId = localStack[cap.Position];
            }

            return removedOperation;
        }

        /// <summary>
        /// Returns the current correlation context. This is the same result as <see cref="ToString"/>, but with better semantic meaning. 
        /// </summary>
        /// <returns>The current correlation context.</returns>
        /// <remarks>Keeping <see cref="ToString"/> so for debugger ease, the object will show the correlation context when collapsed.</remarks>
        public string Get() => this.ToString();

        /// <summary>
        /// Returns the current correlation context. This is the same result as <see cref="Get"/>. 
        /// </summary>
        /// <returns>The current correlation context.</returns>
        /// <remarks>Keeping <see cref="Get"/> so for debugger ease, the object will show the correlation context when collapsed.</remarks>
        public override string ToString()
        {
            StringBuilder operationIdBuilder = new StringBuilder();

            byte[] capturedContext = this.Capture();
            byte[] buffer = new byte[OPERATIONIDBYTELENGTH];

            for (int i = 0; i < capturedContext.Length; i += OPERATIONIDBYTELENGTH)
            {
                // populate buffer;
                Array.Copy(capturedContext, i, buffer, 0, OPERATIONIDBYTELENGTH);

                string operationIdFragment = Convert.ToBase64String(buffer);
                operationIdBuilder.Append(operationIdFragment);

                if (i != capturedContext.Length - 8)
                {
                    operationIdBuilder.Append(BAR);
                }
            }

            string result = operationIdBuilder.ToString();

            return result;
        }

        /// <summary>
        /// Captures the current correlation context and returns it. 
        /// </summary>
        /// <returns>The captured context.</returns>
        public byte[] Capture()
        {
            byte[] result;
            long operationId;
            byte[] buffer;

            // compute operationIdString
            if (this.currentOperationPosition.HasRolledOver)
            {
                // the circular array has rolled over, we must assume overwrite, we must also rollover to compute the Id.

                // 0123456789 123456789 123456789
                //               ^ current position (14)
                // we should index as  15, 16, 17 ... 11, 12 ,13 ,

                // use current position in case is at the very end.
                CircularArrayPosition roller = new CircularArrayPosition(OPERATIONSTACKSIZE, this.currentOperationPosition.Position);
                roller++;

                int traversal = this.currentOperationPosition.TotalTraversal > OPERATIONSTACKSIZE
                    ? OPERATIONSTACKSIZE
                    : this.currentOperationPosition.TotalTraversal;

                // allocate result 
                result = new byte[traversal * OPERATIONIDBYTELENGTH];

                fixed (long* localStack = this.operationStack)
                {
                    for (int i = 0; i < traversal; i++)
                    {

                        operationId = localStack[roller.Position];
                        buffer = BitConverter.GetBytes(operationId);

                        int destinationIndex = i * OPERATIONIDBYTELENGTH;

                        Array.Copy(buffer, 0, result, destinationIndex, OPERATIONIDBYTELENGTH);

                        // roll to next operation id;
                        roller++;
                    }
                }
            }
            else
            {
                int position = this.currentOperationPosition.Position;
                result = new byte[position * OPERATIONIDBYTELENGTH];

                fixed (long* localStack = this.operationStack)
                {
                    for (int i = 0; i < position; i++)
                    {
                        operationId = localStack[i];
                        buffer = BitConverter.GetBytes(operationId);

                        int destinationIndex = i * OPERATIONIDBYTELENGTH;

                        Array.Copy(buffer, 0, result, destinationIndex, OPERATIONIDBYTELENGTH);
                    }
                }
            }

            return result;
        }

        private void AddOperationInternal(long operationId)
        {
            fixed (long* localStack = this.operationStack)
            {
                localStack[this.currentOperationPosition.Position] = operationId;
            }

            // we dont need to worry about rollover because of the
            // CircularArrayPosition. 
            // in the unlikely circumstance that we have a depth tree of > 64
            this.currentOperationPosition++;

            // move internal variables
            this.parentId = this.currentId;
            this.currentId = operationId;
        }
    }
}
