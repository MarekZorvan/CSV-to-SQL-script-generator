using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
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
using System.Windows.Navigation;
using System.Windows.Shapes;
using ScriptGenerator;

namespace CSV_Data_to_SQLscript
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            this.PathLabel.Content = "Please select your CSV file first...";
            this.CreateScriptButton.IsEnabled = false;
        }

        private void CreateScriptButton_Click(object sender, RoutedEventArgs e)
        {
            SaveFileDialog dialog = new SaveFileDialog()
            {
                Filter = "Text Files(*.csv)|*"
            };

            if (dialog.ShowDialog() == true)
            {
                Generator scriptGen = new Generator(); 
                string fileData = scriptGen.MakeScript(this.PathLabel.Content.ToString());
                File.WriteAllText(dialog.FileName, fileData, Encoding.UTF8);
            }
        }

        private void SelectPathButton_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog()
            {
                // Filter = "Text Files(*.csv)|*"
                Filter = "csv files (*.csv)|*.csv|All files (*.*)|*.*"
            };

            if (dialog.ShowDialog() == true)
            {
                this.PathLabel.Content = File.ReadAllText(dialog.FileName);
                this.CreateScriptButton.IsEnabled = true;
            }
        }

        //private void Button_Click(object sender, RoutedEventArgs e)
        //{

        //}
    }
}
