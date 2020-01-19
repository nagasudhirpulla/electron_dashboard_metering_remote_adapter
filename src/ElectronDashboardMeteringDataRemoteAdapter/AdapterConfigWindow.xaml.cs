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
    /// Interaction logic for AdapterConfigWindow.xaml
    /// </summary>
    public partial class AdapterConfigWindow : Window
    {
        public AdapterConfigWindow()
        {
            InitializeComponent();
            DataContext = SettingsEditVM;
        }

        SettingsEditVM SettingsEditVM = new SettingsEditVM();

        private void Cancel_Click(object sender, RoutedEventArgs e)
        {
            ShutdownApp();
        }

        private void Save_Click(object sender, RoutedEventArgs e)
        {
            if (MessageBox.Show("Save Changes ?", "Save Changes", MessageBoxButton.YesNo, MessageBoxImage.Warning) == MessageBoxResult.No)
            {
                //do no stuff
                return;
            }
            else
            {
                SettingsEditVM.Save();
                ShutdownApp();
            }
        }

        private void ShutdownApp()
        {
            Application.Current.Shutdown();
        }
    }
}