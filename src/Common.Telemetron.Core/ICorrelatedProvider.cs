using System;
namespace Polytech.Common.Telemetron
{
    public interface ICorrelatedProvider
    {
        ICorrelationContext CorrelationContext { get; set; }

        void ReapplyOriginContext(byte[] capturedContext);
    }
}
