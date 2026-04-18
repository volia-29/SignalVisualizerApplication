using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Windows.Data;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.DependencyInjection;
using CommunityToolkit.Mvvm.Input;
using SignalVisualizerApplication.Data.Models;
using SignalVisualizerApplication.Data.Services;
using SignalVisualizerApplication.Data.Services.Interfaces;

namespace SignalVisualizerApplication.ViewModels
{
    public class MainViewModel : ObservableObject
    {
        private readonly ISignalService signalService;
        private readonly DataAggregator aggregator;
        private readonly ObservableCollection<SignalRecord> Records = [];
        private bool isRecording;
        private string? filterText;

        public ICollectionView RecordsView { get; }
        public RelayCommand StartCommand { get; }
        public RelayCommand StopCommand { get; }

        public bool IsRecording
        {
            get => isRecording;
            set
            {
                SetProperty(ref isRecording, value);
                StartCommand.NotifyCanExecuteChanged();
                StopCommand.NotifyCanExecuteChanged();
            }
        }

        public string? FilterText
        {
            get => filterText;
            set
            {
                SetProperty(ref filterText, value);
                OnPropertyChanged(nameof(FilterText));
                RecordsView.Refresh();
            }
        }

        public MainViewModel()
        {
            signalService = Ioc.Default.GetService<ISignalService>()!;
            aggregator = new DataAggregator();
            RecordsView = CollectionViewSource.GetDefaultView(Records);
            RecordsView.Filter = FilterLogic;

            StartCommand = new RelayCommand(StartRecording, () => !IsRecording);
            StopCommand = new RelayCommand(StopRecording, () => IsRecording);

            signalService.SignalReceived += OnSignalReceived;
            StartCommand.Execute(null);
        }

        private bool FilterLogic(object obj)
        {
            if (string.IsNullOrWhiteSpace(FilterText)) return true;
            if (obj is not SignalRecord record) return false;

            return record.Frequency.ToString().Contains(FilterText) ||
                   record.Bandwidth.ToString().Contains(FilterText) ||
                   record.SNR.ToString().Contains(FilterText);
        }

        private void StartRecording()
        {
            IsRecording = true;
            signalService.Start();
        }

        private void StopRecording()
        {
            IsRecording = false;
            signalService.Stop();
        }

        private void OnSignalReceived(SignalMessage message)
            => App.Current.Dispatcher.Invoke(() => aggregator.Aggregate(Records, message));
    }
}
