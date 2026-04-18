using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SignalVisualizerApplication.Data.Models;
using SignalVisualizerApplication.Data.Services;

namespace SignalVisualizer.Tests
{
    public class DataAggregatorTests
    {
        [Fact]
        public void Aggregate_ShouldCreateNewRecord_WhenCollectionIsEmpty()
        {
            var aggregator = new DataAggregator();
            var records = new ObservableCollection<SignalRecord>();
            var message = new SignalMessage(1713520000, 100_000_000, 10_000, 15.5);

            aggregator.Aggregate(records, message);

            Assert.Single(records);
            Assert.Equal(100.0, records[0].Frequency);
            Assert.Equal(1, records[0].Count);
        }

        [Fact]
        public void Aggregate_ShouldIncrementCountAndCalculateMedian_WhenFrequencyInRange()
        {
            var aggregator = new DataAggregator();
            var records = new ObservableCollection<SignalRecord>();

            aggregator.Aggregate(records, new SignalMessage(1000, 100_000_000, 20_000, 10.0));
            aggregator.Aggregate(records, new SignalMessage(1001, 100_005_000, 20_000, 12.0));

            Assert.Single(records);
            Assert.Equal(2, records[0].Count);
            Assert.Equal(100.0025, records[0].Frequency);
        }

        [Fact]
        public void Aggregate_ShouldCreateNewRecord_WhenFrequencyOutOfRange()
        {
            var aggregator = new DataAggregator();
            var records = new ObservableCollection<SignalRecord>();

            aggregator.Aggregate(records, new SignalMessage(1000, 100_000_000, 10_000, 10.0));
            aggregator.Aggregate(records, new SignalMessage(1001, 105_000_000, 10_000, 12.0));

            Assert.Equal(2, records.Count);
        }
    }
}
