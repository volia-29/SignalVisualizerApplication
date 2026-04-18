namespace SignalVisualizerApplication.Data.Models
{
    public class SignalRecord
    {
        private readonly List<double> frequencies = [];

        public DateTime Timestamp { get; set; }
        public double Frequency { get; private set; }
        public double Bandwidth { get; set; }
        public double SNR { get; set; }
        public int Count => frequencies.Count;

        public void AddFrequency(double freq)
        {
            frequencies.Add(freq);
            frequencies.Sort();

            var count = Count;
            if (count % 2 == 0)
            {
                Frequency = (frequencies[count / 2 - 1] + frequencies[count / 2]) / 2.0;
            }
            else
            {
                Frequency = frequencies[count / 2];
            }
        }
    }
}
