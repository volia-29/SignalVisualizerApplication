using SignalVisualizerApplication.Data.Models;

namespace SignalVisualizerApplication.Data.Services.Interfaces
{
    public interface ISignalService
    {
        event Action<SignalMessage> SignalReceived;
        void Start();
        void Stop();
    }
}
