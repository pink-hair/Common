using System;
using System.Collections.Generic;

namespace Polytech.Common.Telemetron
{
    public delegate string TelemetronNullCodePointDelegte(string message, Dictionary<string, string> extraData, string callerMemberName, string callerFilePath, int callerLineNumber);
}
