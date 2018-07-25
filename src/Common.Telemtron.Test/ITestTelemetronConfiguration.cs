namespace PinkHair.Common.Telemetron.Configuration
{
    public interface ITestTelemetronConfiguration : ITelemetronConfigurationBase
    {
        bool EmitCurrentTestOutcome { get; set; }
        bool EmitTestName { get; set; }
    }
}