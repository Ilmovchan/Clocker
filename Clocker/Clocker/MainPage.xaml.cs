using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Clocker
{
    public partial class MainPage : ContentPage
    {
        private Stopwatch watch;
        private bool paused;
        private Timer timer;
        public MainPage()
        {
            InitializeComponent();
            paused = true;
        }

        protected override void OnAppearing()
        {
            StartButton.Clicked += StartButton_Clicked;
            PauseButton.Clicked += PauseButton_Clicked;
        }

        private void PauseButton_Clicked(object sender, EventArgs e)
        {
            if (paused == false) StopClock();
        }

        private void StartButton_Clicked(object sender, EventArgs e)
        {
            if (paused) StartClock();
        }

        private void StartClock()
        {
            paused = false;
            watch = new Stopwatch();

            watch.Start();

            timer = new Timer(TimerTick, null, 0, 10);
        }

        private void TimerTick(object state)
        {
            Device.BeginInvokeOnMainThread(() =>
            {
                double totalMilliseconds = watch.Elapsed.TotalMilliseconds;
                TimeSpan timeSpan = TimeSpan.FromMilliseconds(totalMilliseconds);

                string formattedTime = $"{timeSpan.Minutes:D2}:{timeSpan.Seconds:D2}:{timeSpan.Milliseconds/10:D2}";
                TimeLabel.Text = formattedTime;
            });
        }

        private void StopClock()
        {
            timer.Change(Timeout.Infinite, Timeout.Infinite);
            paused = true;
        }
    }
}
