using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using System.Text;

namespace Telemetron.Core.Unit.Tests
{
    public class FakeException : Exception
    {
        public FakeException()
        {
        }

        public FakeException(string message) : base(message)
        {
        }

        public FakeException(string message, Exception innerException) : base(message, innerException)
        {
        }
    }
}
