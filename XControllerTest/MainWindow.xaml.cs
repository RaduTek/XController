using System;
using System.Collections.Generic;
using System.Diagnostics;
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
using XController;

namespace XControllerTest
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        Controller controller;
        Stopwatch stopwatch;

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            stopwatch = Stopwatch.StartNew();
            controller = new Controller();
            controller.ButtonsPressed += Controller_ButtonsPressed;
            controller.ButtonsReleased += Controller_ButtonsReleased;
        }

        private void Controller_ButtonsReleased(object sender, ButtonsReleasedEventArgs e)
        {
            LogTextLine("Buttons released: " + e.JustReleased.ToString());
        }

        private void LogTextLine(string s)
        {
            TimeSpan elapsed = stopwatch.Elapsed;
            LogTextBox.Text += $"[{elapsed:mm\\:ss\\.fff}] {s}\r\n";
            LogTextBox.ScrollToEnd();
        }

        private void Controller_ButtonsPressed(object sender, ButtonsPressedEventArgs e)
        {
            LogTextLine("Buttons pressed: " + e.JustPressed.ToString());
        }
    }
}
