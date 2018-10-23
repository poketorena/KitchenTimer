using Android.App;
using Android.Widget;
using Android.OS;
using System;
using System.Linq;
using System.Threading;
using Android.Media;

namespace KitchenTimer
{
    [Activity(Label = "KitchenTimer", MainLauncher = true)]
    public class MainActivity : Activity
    {
        private int _remainingMilliSec = 0;
        private bool _isStart = false;
        private Button _startButton;
        private Timer _timer;
        protected override void OnCreate(Bundle savedInstanceState)
        {
            base.OnCreate(savedInstanceState);

            // Set our view from the "main" layout resource
            SetContentView(Resource.Layout.Main);

            _startButton = FindViewById<Button>(Resource.Id.StartButton);
            _startButton.Click += this.StartButton_Click;

            var add10MinButton = FindViewById<Button>(Resource.Id.Add10MinButton);
            add10MinButton.Click += this.Add10MinButton_Click;
            var add1MinButton = FindViewById<Button>(Resource.Id.Add1MinButton);
            add1MinButton.Click += (s, e) =>
              {
                  _remainingMilliSec += 60 * 1000;
                  ShowRemainingTime();
              };
            var add10SecButton = FindViewById<Button>(Resource.Id.Add10SecButton);
            add10SecButton.Click += (s, e) =>
            {
                _remainingMilliSec += 10 * 1000;
                ShowRemainingTime();
            };
            var add1SecButton = FindViewById<Button>(Resource.Id.Add1SecButton);
            add1SecButton.Click += (s, e) =>
            {
                _remainingMilliSec += 1 * 1000;
                ShowRemainingTime();
            };
            var clearButton = FindViewById<Button>(Resource.Id.ClearButton);
            clearButton.Click += (s, e) =>
            {
                // クリア時の動作を書く
                if (!_isStart)
                {
                    _remainingMilliSec = 0;
                }
                ShowRemainingTime();
            };

            _timer = new Timer(Timer_OnTick, null, 0, 100);
        }

        private void StartButton_Click(object sender, EventArgs e)
        {
            _isStart = !_isStart;
            if (_isStart)
            {
                _startButton.Text = "ストップ";
            }
            else
            {
                _startButton.Text = "スタート";
            }
        }

        private void Add10MinButton_Click(object sender, System.EventArgs e)
        {
            _remainingMilliSec += 600 * 1000;
            ShowRemainingTime();
        }


        private void ShowRemainingTime()
        {
            var sec = _remainingMilliSec / 1000;
            FindViewById<TextView>(Resource.Id.RemainingTimeTextView).Text
                = string.Format("{0:f0}:{1:d2}",
                sec / 60,
                sec % 60);
        }
        private void Timer_OnTick(object state)
        {
            if (!_isStart)
            {
                return;
            }

            RunOnUiThread(() =>
           {
               _remainingMilliSec -= 100;
               if (_remainingMilliSec <= 0)
               {
                    // 0ミリ秒になった
                    _isStart = false;
                   _remainingMilliSec = 0;
                   _startButton.Text = "スタート";
                   // アラームを鳴らす
                   var toneGenerator = new ToneGenerator(Stream.System, 50);
                   toneGenerator.StartTone(Tone.PropBeep);
                }
               ShowRemainingTime();
           });
        }
    }
}

