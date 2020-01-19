using AdapterUtils;
using Npgsql;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace ElectronDashboardMeteringDataRemoteAdapter
{
    /// <summary>
    /// Interaction logic for MeasPickerWindow.xaml
    /// </summary>
    public partial class MeasPickerWindow : Window
    {
        public ConfigurationManager Config_ { get; set; } = new ConfigurationManager();
        public DataFetcher DataFetcher_ { get; set; }
        private List<FictMeasurement> MeteringMeasList_ = new List<FictMeasurement>();
        public MeasPickerWindow()
        {
            InitializeComponent();
            Config_.Initialize();
            DataFetcher_ = new DataFetcher() { Config_ = Config_ };
            RefreshMeasurements();
        }

        private async Task RefreshMeasurements()
        {
            MeteringMeasList_ = await DataFetcher_.FetchFictMeasList();
            MeasListView.ItemsSource = MeteringMeasList_;
        }

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            ShutdownApp();
        }

        private void OK_Click(object sender, RoutedEventArgs e)
        {
            //todo console the selected measurement
            int selectedIndex = MeasListView.SelectedIndex;
            if (selectedIndex > -1)
            {
                object selObj = MeasListView.SelectedItems[0];
                string measId = ((FictMeasurement)selObj).LocationTag;
                string measDesc = ((FictMeasurement)selObj).Description;
                // measId, measName, measDescription
                ConsoleUtils.FlushMeasData(measId, measId, measDesc);
                ShutdownApp();
            }
            else
            {
                MessageBox.Show("Please select a measurement...");
            }
        }

        private void RefreshBtn_Click(object sender, RoutedEventArgs e)
        {
            RefreshMeasurements();
        }

        private void FilterTxt_Changed(object sender, RoutedEventArgs e)
        {
            List<FictMeasurement> filteredMeasurements = MeteringMeasList_;
            if (!string.IsNullOrEmpty(NameFilter.Text))
            {
                filteredMeasurements = filteredMeasurements.Where(item =>
                {
                    string measId = item.LocationTag;
                    return measId.ToUpper().Contains(NameFilter.Text.ToUpper());
                }).ToList();
            }
            if (!string.IsNullOrEmpty(DescFilter.Text))
            {

                filteredMeasurements = filteredMeasurements.Where(item =>
                {
                    string measDesc = item.Description;
                    return measDesc.ToUpper().Contains(DescFilter.Text.ToUpper());
                }).ToList();
            }
            MeasListView.ItemsSource = filteredMeasurements;
        }

        private void ShutdownApp()
        {
            Application.Current.Shutdown();
        }
    }
}
