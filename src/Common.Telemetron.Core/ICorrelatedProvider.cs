using System;
namespace PinkHair.Common.Telemetron
{
    public interface ICorrelatedProvider
    {
        ICorrelationContext CorrelationContext { get; set; }

        void ReapplyOriginContext(byte[] capturedContext);
    }
}
