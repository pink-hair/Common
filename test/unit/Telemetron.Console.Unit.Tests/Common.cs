using System;
using System.Collections.Generic;
using System.Text;

namespace Telemetron.Console.Unit.Tests
{
    using PinkHair.Common.Telemetron;
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
                OperationConfiguration = new PinkHair.Common.Telemetron.Configuration.OperationConfiguration()
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
