using System;
using System.Collections.Generic;
using System.Text;

namespace Telemetron.Console.Unit.Tests
{
    using Polytech.Common.Telemetron;
    using System.Diagnostics.CodeAnalysis;

    [ExcludeFromCodeCoverage]
    internal static class Common
    {
        internal static ConsoleConfiguration CreateDefaultConfiguration()
        {
            return new ConsoleConfiguration()
            {
                EmitAdditionalData = true,
                EmitCodePoint = true,
                EmitCorrelationContext = true,
                OperationConfiguration = new Polytech.Common.Telemetron.Configuration.OperationConfiguration()
                {
                    EmitOperationMetrics = true
                }
            };
        }


        internal static ConsoleTelemetron Get()
        {
            return new ConsoleTelemetron(CreateDefaultConfiguration());
        }
    }
}
