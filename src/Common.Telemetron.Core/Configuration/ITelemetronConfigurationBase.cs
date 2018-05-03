namespace Polytech.Common.Telemetron.Configuration
{
    public interface ITelemetronConfigurationBase
    {
        TelemetronNullCodePointDelegte NullCodepointAction { get; set; }
        bool EmitCodePoint { get; set; }
        bool EmitCallerMemberName { get; set; }
        bool EmitCallerFilePath { get; set; }
        bool EmitCallerLineNumber { get; set; }
        bool EmitAdditionalData { get; set; }
        bool EmitCorrelationContext { get; set; }
    }
}