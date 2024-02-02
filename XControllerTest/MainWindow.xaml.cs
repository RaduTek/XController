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
            SetPointPosition(LeftThumbPoint, 0, 0);
            SetPointPosition(RightThumbPoint, 0, 0);
            SetBarHeightRelative(LeftTriggerBar, 0);
            SetBarHeightRelative(RightTriggerBar, 0);

            stopwatch = Stopwatch.StartNew();
            controller = new Controller();
            controller.ButtonsPressed += Controller_ButtonsPressed;
            controller.ButtonsReleased += Controller_ButtonsReleased;
            controller.ConnectionStatusChanged += Controller_ConnectionStatusChanged;
            controller.TriggersMoved += Controller_TriggersMoved;

            controller.LeftThumbMoved += Controller_LeftThumbMoved;
            controller.RightThumbMoved += Controller_RightThumbMoved;
        }

        private void Controller_RightThumbMoved(object sender, EventArgs e)
        {
            SetPointPosition(RightThumbPoint, controller.RightThumb.X, controller.RightThumb.Y);
            RightThumbLabel.Content = controller.RightThumb.ToString();
        }

        private void Controller_LeftThumbMoved(object sender, EventArgs e)
        {
            SetPointPosition(LeftThumbPoint, controller.LeftThumb.X, controller.LeftThumb.Y);
            LeftThumbLabel.Content = controller.LeftThumb.ToString();
        }

        private void Controller_TriggersMoved(object sender, EventArgs e)
        {
            LeftTriggerLabel.Content = controller.Triggers.Left.ToString("0.000");
            SetBarHeightRelative(LeftTriggerBar, controller.Triggers.Left);
            RightTriggerLabel.Content = controller.Triggers.Right.ToString("0.000");
            SetBarHeightRelative(RightTriggerBar, controller.Triggers.Right);
        }

        private void SetBarHeightRelative(Rectangle bar, double ratio)
        {
            Border parent = (Border)bar.Parent;

            bar.Height = (parent.ActualHeight - (parent.Padding.Top + parent.Padding.Bottom)) * ratio;
        }

        private void SetPointPosition(Ellipse point, double x, double y)
        {
            Canvas parent = (Canvas)point.Parent;

            point.Margin = new Thickness()
            {
                Top = (parent.ActualHeight) * ((y * -1) / 2 + 0.5) - point.Height / 2,
                Left = (parent.ActualWidth) * (x / 2 + 0.5) - point.Width / 2,
                Bottom = 0,
                Right = 0
            };
        }

        private void Controller_ConnectionStatusChanged(object sender, EventArgs e)
        {
            LogTextLine($"Controller {(controller.Connected ? "connected" : "disconnected")}");
        }

        private void Controller_ButtonsReleased(object sender, ButtonsReleasedEventArgs e)
        {
            LogTextLine($"Buttons released: {e.Buttons}");
        }

        private void LogTextLine(string s)
        {
            TimeSpan elapsed = stopwatch.Elapsed;
            LogTextBox.Text += $"[{elapsed:mm\\:ss\\.fff}] {s}\r\n";
            LogTextBox.ScrollToEnd();
        }

        private void Controller_ButtonsPressed(object sender, ButtonsPressedEventArgs e)
        {
            LogTextLine("Buttons pressed: " + e.Buttons.ToString());
        }
    }
}
