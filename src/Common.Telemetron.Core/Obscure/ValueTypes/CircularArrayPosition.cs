namespace Polytech.Common.Telemetron.Obscure.ValueTypes
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using Common.Telemetron.Diagnostics;

    /// <summary>
    /// A struct to represent the position of a circular array.
    /// </summary>
    internal struct CircularArrayPosition
    {
        private int totalTraversal;
        private int size;
        private int maxpos;

        private int position;

        private bool hasRolledOver;

        public CircularArrayPosition(int size)
        {
            if (size < 1)
            {
                throw new ArgumentException("The size cannot be negative or zero.", nameof(size));
            }

            this.size = size;
            this.maxpos = size - 1;
            this.position = 0;
            this.hasRolledOver = false;
            this.totalTraversal = 0;
        }

        public CircularArrayPosition(int size, int position)
        {
            if (size < 1)
            {
                throw new ArgumentException("The size cannot be negative or zero.", nameof(size));
            }

            if (position < 0)
            {
                throw new ArgumentException("The position cannot be negative", nameof(position));
            }

            this.maxpos = size - 1;

            if (position > this.maxpos)
            {
                throw new ArgumentException("The starting postiion cannot exceed the maximum position of the circular array", nameof(position));
            }

            this.size = size;

            this.position = position;

            this.hasRolledOver = false;
            this.totalTraversal = 0;
        }

        public int Position => this.position;

        public bool HasRolledOver => this.hasRolledOver;

        public int TotalTraversal => this.totalTraversal;

        public static CircularArrayPosition operator ++(CircularArrayPosition val)
        {
            if (val.position == val.maxpos)
            {
                val.position = 0;
                val.hasRolledOver = true;
            }
            else
            {
                val.position++;
            }

            val.totalTraversal++;

            return val;
        }

        public static CircularArrayPosition operator --(CircularArrayPosition val)
        {
            if (val.position == 0)
            {
                val.position = val.maxpos;
                val.hasRolledOver = true;
            }
            else
            {
                val.position--;
            }

            val.totalTraversal--;

            return val;
        }

        public void ResetRollover()
        {
            this.hasRolledOver = false;
        }
    }
}
