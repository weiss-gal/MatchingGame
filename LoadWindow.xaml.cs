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
using MatchingGame.GameManagement;

namespace MatchingGame
{
    /// <summary>
    /// Interaction logic for LoadWindow.xaml
    /// </summary>
    public partial class LoadWindow : Window
    {
        private LoadManager loadManager = null;
        public LoadWindow(LoadManager loadManager)
        {
            InitializeComponent();
            this.loadManager = loadManager;
            this.loadManager.start();
        }

        private void MoreInfo_Click(object sender, RoutedEventArgs e)
        {
            
        }
    }
}
