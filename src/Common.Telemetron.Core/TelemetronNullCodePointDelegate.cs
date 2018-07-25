using System;
using System.Collections.Generic;

namespace PinkHair.Common.Telemetron
{
    public delegate string TelemetronNullCodePointDelegte(string message, Dictionary<string, string> extraData, string callerMemberName, string callerFilePath, int callerLineNumber);
}
