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
using System.Windows.Navigation;
using System.Windows.Shapes;
using MatchingGame.GameManagement;
using Microsoft.Win32;

namespace MatchingGame
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly string supportedFilesDesc = "Comma Separated Value files(*.csv)|*.csv|All files(*.*)|*.*";
        private GameManager gm = null;
        public MainWindow()
        {
            gm = new GameManager();
            InitializeComponent();
        }

        private void LoadButton_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog();
            openFileDialog.Filter = supportedFilesDesc;
            if (openFileDialog.ShowDialog() == false)
                return;
            
            var lm = gm.getLoadManager(openFileDialog.FileName);
            var loadWindow = new LoadWindow(lm);
            loadWindow.ShowDialog();
        }
    }
}
