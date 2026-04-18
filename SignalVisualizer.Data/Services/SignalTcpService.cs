using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SignalVisualizerApplication.Data.Helpers;
using SignalVisualizerApplication.Data.Models;
using SignalVisualizerApplication.Data.Services.Interfaces;

namespace SignalVisualizerApplication.Data.Services
{
    public class SignalTcpService : ISignalService
    {
        private readonly Random random = new();
        private CancellationTokenSource? cancellationTokenSource;
        public event Action<SignalMessage>? SignalReceived;

        public void Start()
        {
            cancellationTokenSource = new CancellationTokenSource();
            Task.Run(() => GenerateData(cancellationTokenSource.Token));
        }

        public void Stop()
        {
            cancellationTokenSource?.Cancel();
        }

        private async Task GenerateData(CancellationToken token)
        {
            while (!token.IsCancellationRequested)
            {
                var rawPacket = CreateMockedPacket();

                var message = ParseRawPacket(rawPacket);

                SignalReceived?.Invoke(message);
                await Task.Delay(1000, token);
            }
        }

        private SignalMessage ParseRawPacket(byte[] packet)
        {
            byte[] headerBytes = { packet[0], packet[1] };
            var (length, type) = BitConverterHelper.ParseHeader(headerBytes);

            var ts = BitConverterHelper.ToUInt64(packet, 2);
            var freq = BitConverterHelper.ToUInt64(packet, 10);
            var bw = BitConverterHelper.ToUInt32(packet, 18);
            var snr = BitConverterHelper.ToDouble(packet, 22);

            return new SignalMessage(ts, freq, bw, snr);
        }

        private byte[] CreateMockedPacket()
        {
            using (MemoryStream ms = new MemoryStream())
            {
                using (BinaryWriter writer = new BinaryWriter(ms))
                {
                    var timestamp = (ulong)DateTimeOffset.UtcNow.ToUnixTimeSeconds();
                    var frequency = (ulong)random.NextInt64(90_000_000, 100_000_000);
                    var bandwidth = (uint)random.NextInt64(12_500, 900_000);
                    var snr = (12.0 + random.NextDouble() * 5.0);

                    writer.Write(timestamp);
                    writer.Write(frequency);
                    writer.Write(bandwidth);
                    writer.Write(snr);

                    var payload = ms.ToArray();
                    var totalLength = payload.Length + 2;
                    var messageType = 0x04;

                    var headerLsb = (byte)(totalLength & 0xFF);
                    var lengthMsb = (totalLength >> 8) & 0x1F;
                    var headerMsb = (byte)((lengthMsb << 3) | (messageType & 0x07));

                    var finalPacket = new byte[totalLength];
                    finalPacket[0] = headerLsb;
                    finalPacket[1] = headerMsb;
                    Array.Copy(payload, 0, finalPacket, 2, payload.Length);

                    return finalPacket;
                }
            }
        }
    }
}
