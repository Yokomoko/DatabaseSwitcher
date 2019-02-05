using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Runtime.Remoting.Channels;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace DatabaseSwitcher {
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window {
        private const string RAndRDirectory = @"C:\RMS\DB\Live\Working";
        private const string PTSDirectory = @"C:\RMS\DB\Live\Working\PTS";

        private enum Databases {
            RMS_Master,
            RMS_PTS,
            RMS_RAndR,
            RMS_Fleet,
            RMS_PdaComms,
            RMS_Portal,
            RMS_Purchase,
            RMS_Documents,
            RMS_Documents_Archive
        }

        public MainWindow() {
            InitializeComponent();
        }

        private void UxSwitchToRmsBtn_OnClick(object sender, RoutedEventArgs e) {
            try {
                using (var conn = new SqlConnection("Data Source=.;database=Master;Integrated Security=False;User=sa;pwd=password")) {
                    SqlCommand cmd = new SqlCommand("", conn);
                    conn.Open();
                    foreach (var value in Enum.GetValues(typeof(Databases))) {
                        var enumName = Enum.GetName(typeof(Databases), value);
                        try {
                            cmd.CommandText = $@"sys.sp_detach_db {enumName}";
                            cmd.ExecuteNonQuery();
                        }
                        catch (Exception error) {
                            MessageBox.Show("Error" + error.Message);
                        }
                        try {
                            cmd.CommandText = $@"sp_attach_db @dbname = N'{enumName}', @filename1 = N'C:\RMS\DB\Live\Working\{enumName}.mdf', @filename2 = N'C:\RMS\DB\Live\Working\{((Databases)value == Databases.RMS_Documents_Archive ? enumName + "_log" : enumName)}.ldf'";
                            cmd.ExecuteNonQuery();
                        }
                        catch (Exception error) {
                            MessageBox.Show("Error" + error.Message);
                        }
                    }
                }
            }
            catch (Exception ex) {
                MessageBox.Show("Error" + ex.Message);
            }
        }

        private void uxSwitchToPtsBtn_Click(object sender, RoutedEventArgs e) {
            try {
                using (var conn = new SqlConnection("Data Source=.;database=Master;Integrated Security=False;User=sa;pwd=password")) {
                    SqlCommand cmd = new SqlCommand("", conn);
                    conn.Open();
                    foreach (var value in Enum.GetValues(typeof(Databases))) {
                        var enumName = Enum.GetName(typeof(Databases), value);
                        try {
                            cmd.CommandText = $@"sys.sp_detach_db {enumName}";
                            cmd.ExecuteNonQuery();
                        }
                        catch (Exception error) {
                            MessageBox.Show("Error" + error.Message);
                        }
                        try {
                            cmd.CommandText = $@"sp_attach_db @dbname = N'{enumName}', @filename1 = N'C:\RMS\DB\Live\Working\PTS\{enumName}.mdf', @filename2 = N'C:\RMS\DB\Live\Working\PTS\{((Databases)value == Databases.RMS_Documents_Archive ? enumName + "_log" : enumName)}.ldf'";
                            cmd.ExecuteNonQuery();
                        }
                        catch (Exception error) {
                            MessageBox.Show("Error" + error.Message);
                        }
                    }
                }
            }
            catch (Exception ex) {
                MessageBox.Show("Error" + ex.Message);
            }
        }
    }
}
