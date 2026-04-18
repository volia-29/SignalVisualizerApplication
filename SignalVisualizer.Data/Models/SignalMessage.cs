namespace SignalVisualizerApplication.Data.Models
{
    public class SignalMessage(ulong timestamp, ulong frequency, uint bandwidth, double snr)
    {
        public ulong Timestamp { get; set; } = timestamp;
        public ulong Frequency { get; set; } = frequency;
        public uint Bandwidth { get; set; } = bandwidth;
        public double SNR { get; set; } = snr;
    }
}
