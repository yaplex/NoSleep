using System;
using System.Windows;
using System.Windows.Threading;
using Yaplex.NoSleep;

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
            StatusLabel.Visibility = Visibility.Hidden;

            ActivateNoSleep();
        }

        private void ActivateNoSleep()
        {
            StatusLabel.Visibility = Visibility.Visible;

            var activityTimer = new DispatcherTimer();
            activityTimer.Tick += makeActive_tick;
            activityTimer.Interval = TimeSpan.FromMilliseconds(2500);
            activityTimer.Start();
        }


        private void makeActive_tick(object sender, EventArgs e)
        {
            if (NineToFive.IsChecked.HasValue && NineToFive.IsChecked.Value)
            {
                var time = DateTime.Now;
                if (TimeSpan.Compare(time.TimeOfDay, TimeSpan.FromHours(9)) > 0 
                && TimeSpan.Compare(time.TimeOfDay, TimeSpan.FromHours(17) ) < 0)
                {
                    InputSimulator.SimulateInput();
                }
            }
            else
            {
                InputSimulator.SimulateInput();
            }
        }

        private void displayLastUserActivity_tick(object sender, EventArgs e)
        {
            var idleTime = InputSimulator.GetIdleTime();
            label.Content = $"{idleTime:mm\\:ss\\:ffff} seconds";
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

            var dt = DateTime.Now;

            var countDownTimer = new DispatcherTimer();
            countDownTimer.Interval = TimeSpan.FromSeconds(1);
            countDownTimer.Tick += (o, args) =>
            {
                var difference = ExitInTime - (DateTime.Now - dt);
                StatusLabel.Content = $"Auto exit in {difference:hh\\:mm\\:ss}";
            };
            countDownTimer.Start();
        }
    }
}