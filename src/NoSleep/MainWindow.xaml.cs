using System;
using System.Windows;
using System.Windows.Threading;

namespace NoSleep
{
    /// <summary>
    ///     Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();

            var dispatcherTimer = new DispatcherTimer();
            dispatcherTimer.Tick += displayLastUserActivity_tick;
            dispatcherTimer.Interval = TimeSpan.FromMilliseconds(100);
            dispatcherTimer.Start();
        }

        private void button_Click(object sender, RoutedEventArgs e)
        {
            button.IsEnabled = false;
            AppIsRunningMessage.Visibility = Visibility.Visible;

            var dispatcherTimer = new DispatcherTimer();
            dispatcherTimer.Tick += moveMouse_tick;
            dispatcherTimer.Interval = TimeSpan.FromMilliseconds(2500);
            dispatcherTimer.Start();
        }

        private void moveMouse_tick(object sender, EventArgs e)
        {
            MouseMover.MakeActive();
        }

        private void displayLastUserActivity_tick(object sender, EventArgs e)
        {
            var idleTime = MouseMover.GetIdleTime();
            label.Content = idleTime.ToString();
        }
    }
}