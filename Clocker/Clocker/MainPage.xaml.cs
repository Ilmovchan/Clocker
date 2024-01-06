using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace Clocker
{
    public partial class MainPage : ContentPage
    {
        Stopwatch watch;
        Timer timer;
        double lastWatch;

        private ImageButton StartButton;
        private ImageButton StopButton;
        private ImageButton ResetButton;
        private ImageButton FlagButton;

        public MainPage()
        {
            InitializeComponent();
            watch = new Stopwatch();
            lastWatch = 0;
        }

        protected override void OnAppearing()
        {
            CreateStartButton(2);
        }

        private void StartButton_Clicked(object sender, EventArgs e)
        {
            StartClock();

            ClearButtons();

            CreateFlagButton(1);
            CreateStopButton(3);
        }

        private void StopButton_Clicked(object sender, EventArgs e)
        {
            StopClock();

            ClearButtons();

            CreateStartButton(3);
            CreateResetButton(1);
        }

        private void ResetButton_Clicked(object sender, EventArgs e)
        {
            watch.Reset();
            lastWatch = 0;
            TimeLabel.Text = "00:00:00";

            ClearButtons();
            FlagLayout.Children.Clear();
            CreateStartButton(2);
        }

        private void FlagButton_Clicked(object sender, EventArgs e)
        {
            AddTimeFlag();
        }

        private void AddTimeFlag()
        {
            if (FlagLayout.Children.Count > 5) FlagLayout.Children.RemoveAt(0);

            FlagLayout.Children.Add(new Label
            {
                Text = GetTimeDifference(),
                FontSize = 25,
                HorizontalTextAlignment = TextAlignment.Center,
                TextColor = Color.Gray,
            });

            lastWatch = watch.Elapsed.TotalMilliseconds;
        }

        private void StartClock()
        {
            watch.Start();
            timer = new Timer(Timer_Tick, null, 0, 10);
        }

        private void StopClock()
        {
            watch.Stop();
            timer.Change(Timeout.Infinite, Timeout.Infinite);
        }

        private void Timer_Tick(object state)
        {
            Device.BeginInvokeOnMainThread(() =>
            {
                TimeLabel.Text = GetTime();
            });
        }

        private string GetTime()
        {
            double totalMilliseconds = watch.Elapsed.TotalMilliseconds;
            TimeSpan timeSpan = TimeSpan.FromMilliseconds(totalMilliseconds);

            string formattedTime = $"{timeSpan.Minutes:D2}:{timeSpan.Seconds:D2}:{timeSpan.Milliseconds / 10:D2}";
            return formattedTime;
        }

        private string GetTimeDifference()
        {
            double difference = watch.Elapsed.TotalMilliseconds - lastWatch;

            TimeSpan timeSpan = TimeSpan.FromMilliseconds(difference);

            string formattedTime = $"{timeSpan.Minutes:D2}:{timeSpan.Seconds:D2}:{timeSpan.Milliseconds / 10:D2}";
            return formattedTime;
        }

        private void CreateStartButton(int position)
        {
            StartButton = CreateButton("play.png");

            Grid.SetColumn (StartButton, position);
            ButtonGrid.Children.Add(StartButton);

            StartButton.Clicked += StartButton_Clicked;
        }

        private void CreateStopButton(int position)
        {
            StopButton = CreateButton("pause.png");

            Grid.SetColumn(StopButton, position);
            ButtonGrid.Children.Add(StopButton);

            StopButton.Clicked += StopButton_Clicked;
        }

        private void CreateResetButton(int position)
        {
            ResetButton = CreateButton("undo.png");

            Grid.SetColumn(ResetButton, position);
            ButtonGrid.Children.Add(ResetButton);

            ResetButton.Clicked += ResetButton_Clicked;
        }

        private void CreateFlagButton(int position)
        {
            FlagButton = CreateButton("flag.png");

            Grid.SetColumn(FlagButton, position);
            ButtonGrid.Children.Add(FlagButton);

            FlagButton.Clicked += FlagButton_Clicked;
        }

        private ImageButton CreateButton(string source)
        {
            return new ImageButton() 
            {
                Source = source,
                WidthRequest = 50,
                MinimumWidthRequest = 50,
                BackgroundColor = Color.FromHex("#fffafafa"),
            };
        }

        private void ClearButtons()
        {
            ButtonGrid.Children.Clear();
        }
    }
}
