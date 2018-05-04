namespace Polytech.Common.Telemetron.Configuration
{
    public class TestTelemetronConfiguration : TelemetronConfigurationBase, ITestTelemetronConfiguration
    {
        public bool EmitTestName { get; set; }

        public bool EmitCurrentTestOutcome { get; set; }
    }
}
