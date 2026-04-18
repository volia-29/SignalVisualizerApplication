using System.Windows;
using CommunityToolkit.Mvvm.DependencyInjection;
using Microsoft.Extensions.DependencyInjection;
using SignalVisualizerApplication.Data.Services;
using SignalVisualizerApplication.Data.Services.Interfaces;
using SignalVisualizerApplication.ViewModels;

namespace SignalVisualizerApplication
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        public App()
        {
            var services = new ServiceCollection();

            services.AddSingleton<ISignalService, SignalTcpService>();
            services.AddTransient<MainViewModel>();

            Ioc.Default.ConfigureServices(services.BuildServiceProvider());
        }
    }

}
