namespace SignalVisualizerApplication.Data.Helpers
{
    public static class BitConverterHelper
    {
        public static (int length, int messageType) ParseHeader(byte[] headerBytes)
        {
            if (headerBytes == null || headerBytes.Length < 2)
                throw new ArgumentException();

            var lsb = headerBytes[0];
            var byte1 = headerBytes[1];

            var type = byte1 & 0x07;
            var msb = (byte1 >> 3) & 0x1F;
            var totalLength = lsb | (msb << 8);

            return (totalLength, type);
        }

        public static ulong ToUInt64(byte[] data, int startIndex)
            => BitConverter.ToUInt64(data, startIndex);

        public static uint ToUInt32(byte[] data, int startIndex)
            => BitConverter.ToUInt32(data, startIndex);

        public static double ToDouble(byte[] data, int startIndex)
            => BitConverter.ToDouble(data, startIndex);
    }
}
