using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SignalVisualizerApplication.Data.Helpers;

namespace SignalVisualizer.Tests
{
    public class BitConverterHelperTests
    {
        [Fact]
        public void ParseHeader_ShouldCorrectlyCalculateLengthAndType()
        {
            byte[] header = { 0x10, 0x0A };

            var result = BitConverterHelper.ParseHeader(header);

            Assert.Equal(272, result.length);
            Assert.Equal(2, result.messageType);
        }
        [Fact]
        public void ParseHeader_CorrectlyParsesStandardMessage()
        {
            byte[] header = { 0x08, 0x01 };

            var (length, messageType) = BitConverterHelper.ParseHeader(header);

            Assert.Equal(8, length);
            Assert.Equal(1, messageType);
        }

        [Fact]
        public void ParseHeader_CorrectlyParsesMaximumValues()
        {
            byte[] header = { 0xFF, 0xFB };

            var (length, messageType) = BitConverterHelper.ParseHeader(header);

            Assert.Equal(8191, length);
            Assert.Equal(3, messageType);
        }

        [Fact]
        public void ParseHeader_ShouldThrowArgumentException_WhenBufferTooSmall()
        {
            byte[] smallHeader = { 0x01 };

            Assert.Throws<ArgumentException>(() => BitConverterHelper.ParseHeader(smallHeader));
            Assert.Throws<ArgumentException>(() => BitConverterHelper.ParseHeader(null));
        }

        [Fact]
        public void ToUInt64_ShouldHandleLargeFrequencies()
        {
            byte[] data = { 0x00, 0x2D, 0x2C, 0x06, 0x00, 0x00, 0x00, 0x00 };

            ulong result = BitConverterHelper.ToUInt64(data, 0);

            Assert.Equal(103558400UL, result);
        }

        [Fact]
        public void ToDouble_ShouldHandleSNRValues()
        {
            byte[] data = BitConverter.GetBytes(15.235);

            double result = BitConverterHelper.ToDouble(data, 0);

            Assert.Equal(15.235, result, 5);
        }

        [Fact]
        public void ToUInt32_ShouldHandleBandwidth()
        {
            byte[] data = { 0xD4, 0x30, 0x00, 0x00 };

            uint result = BitConverterHelper.ToUInt32(data, 0);

            Assert.Equal(12500U, result);
        }
    }
}
