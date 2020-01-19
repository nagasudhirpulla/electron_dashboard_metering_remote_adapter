using AdapterUtils;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;

namespace ElectronDashboardMeteringDataRemoteAdapter
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        private void Application_Startup(object sender, StartupEventArgs e)
        {
            AdapterParams prms = AdapterParams.ParseArgs(e.Args);
            if (prms.RequestType == RequestTypes.Config)
            {
                AdapterConfigWindow win = new AdapterConfigWindow();
                win.Show();
            }
            else if (prms.RequestType == RequestTypes.PickMeas)
            {
                MeasPickerWindow win = new MeasPickerWindow();
                win.Show();
            }
            else
            {
                // we assume that the request is getting data
                ConfigurationManager Config_ = new ConfigurationManager();
                Config_.Initialize();
                DataFetcher dataFetcher = new DataFetcher() { Config_ = Config_ };
                Task.Run(async () => await dataFetcher.FetchAndFlushData(prms)).Wait();
                Current.Shutdown();
            }
        }
    }
}
