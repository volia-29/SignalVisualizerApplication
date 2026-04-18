using System.Collections.ObjectModel;
using SignalVisualizerApplication.Data.Models;

namespace SignalVisualizerApplication.Data.Services
{
    public class DataAggregator
    {
        public void Aggregate(ObservableCollection<SignalRecord> records, SignalMessage message)
        {
            var msgFreqMhz = message.Frequency / 1_000_000.0;
            var msgBwKhz = message.Bandwidth / 1_000.0;

            var existingRecord = records.LastOrDefault();

            if (existingRecord != null)
            {
                var halfBwMhz = (existingRecord.Bandwidth / 1000.0) / 2.0;
                var minFreq = existingRecord.Frequency - halfBwMhz;
                var maxFreq = existingRecord.Frequency + halfBwMhz;

                if (msgFreqMhz >= minFreq && msgFreqMhz <= maxFreq)
                {
                    existingRecord.AddFrequency(msgFreqMhz);
                    return;
                }
            }

            var newRecord = new SignalRecord
            {
                Timestamp = DateTimeOffset.FromUnixTimeSeconds((long)message.Timestamp).DateTime,
                Bandwidth = msgBwKhz,
                SNR = message.SNR,
            };
            newRecord.AddFrequency(msgFreqMhz);

            records.Add(newRecord);
        }
    }
}
