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

            if (System.Deployment.Application.ApplicationDeployment.IsNetworkDeployed)
            {
                System.Deployment.Application.ApplicationDeployment cd =
                    System.Deployment.Application.ApplicationDeployment.CurrentDeployment;
                NoSleepWindow.Title = $"NoSleep - Yaplex Inc. (https://www.yaplex.com) - {cd.CurrentVersion}";
            }
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

        private void Slider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            var exitInMinutesValue = (int) e.NewValue;
            ExitInTime = TimeSpan.FromMinutes(exitInMinutesValue);
            if(null != exitInButton)
                exitInButton.Content = $"Exit in {ExitInTime:hh\\:mm} hh:mm";
        }
        private TimeSpan ExitInTime { get; set; }

        private void exitInButton_Click(object sender, RoutedEventArgs e)
        {
            exitInButton.IsEnabled = false;
            ExitSlider.IsEnabled = false;

            var dispatcherTimer = new DispatcherTimer();
            dispatcherTimer.Tick += (o, args) =>
            {
                Application.Current.Shutdown();
            };
            dispatcherTimer.Interval = ExitInTime;
            dispatcherTimer.Start();
        }
    }
}