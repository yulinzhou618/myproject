using System;
using System.ComponentModel;
using System.IO;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Threading;

namespace AIVisualwfpnew.Windows
{
    /// <summary>
    /// VideoViewWindow.xaml 的交互逻辑   MediaElement控件不好用，后续考虑改为vlc播放
    /// </summary>
    public partial class VideoViewWindow : Window, INotifyPropertyChanged
    {
        /// <summary>
        /// 视频源地址
        /// </summary>
        public Uri VideoUrl
        {
            get { return (Uri)GetValue(VideoUrlProperty); }
            set { SetValue(VideoUrlProperty, value); }
        }

        public static readonly DependencyProperty VideoUrlProperty =
            DependencyProperty.Register("VideoUrl", typeof(Uri), typeof(VideoViewWindow), new PropertyMetadata(null));

        public bool ShowPlayOrPauseCT
        {
            get { return (bool)GetValue(ShowPlayOrPauseCTProperty); }
            set { SetValue(ShowPlayOrPauseCTProperty, value); }
        }

        public static readonly DependencyProperty ShowPlayOrPauseCTProperty =
            DependencyProperty.Register("ShowPlayOrPauseCT", typeof(bool), typeof(VideoViewWindow), new PropertyMetadata(false));

        public bool IsPlaying
        {
            get { return (bool)GetValue(IsPlayingProperty); }
            set { SetValue(IsPlayingProperty, value); }
        }

        public static readonly DependencyProperty IsPlayingProperty =
            DependencyProperty.Register("IsPlaying", typeof(bool), typeof(VideoViewWindow), new PropertyMetadata(false));

        /// <summary>
        /// 已播放时间
        /// </summary>
        public string ElapsedStr
        {
            get { return (string)GetValue(ElapsedStrProperty); }
            set { SetValue(ElapsedStrProperty, value); }
        }

        public static readonly DependencyProperty ElapsedStrProperty =
            DependencyProperty.Register("ElapsedStr", typeof(string), typeof(VideoViewWindow), new PropertyMetadata("00:00"));

        /// <summary>
        /// 总时长
        /// </summary>
        public string TotalTime
        {
            get { return (string)GetValue(TotalTimeProperty); }
            set { SetValue(TotalTimeProperty, value); }
        }

        public static readonly DependencyProperty TotalTimeProperty =
            DependencyProperty.Register("TotalTime", typeof(string), typeof(VideoViewWindow), new PropertyMetadata("00:00"));

        /// <summary>
        /// 播放进度
        /// </summary>
        public double PlayingProgress
        {
            get { return (double)GetValue(PlayingProgressProperty); }
            set { SetValue(PlayingProgressProperty, value); }
        }

        public static readonly DependencyProperty PlayingProgressProperty =
            DependencyProperty.Register("PlayingProgress", typeof(double), typeof(VideoViewWindow), new PropertyMetadata(0d));

        public event PropertyChangedEventHandler PropertyChanged;
        private DispatcherTimer _timer = null;

        public VideoViewWindow()
        {
            InitializeComponent();
            this.Loaded += VideoViewWindow_Loaded;
            videoct.MediaOpened += Videoct_MediaOpened;
            videoct.MediaEnded += Videoct_MediaEnded;
            videoct.MediaFailed += Videoct_MediaFailed;
        }

        public VideoViewWindow(string url) : this()
        {
            if (string.IsNullOrEmpty(url))
                return;

            var fileName = Path.GetFileName(url);
            if (!string.IsNullOrEmpty(fileName))
                this.Title = fileName;

            this.VideoUrl = new Uri(url);
        }

        public VideoViewWindow(Uri neturl):this()
        {
            this.VideoUrl = neturl;
        }

        private void Videoct_MediaFailed(object sender, ExceptionRoutedEventArgs e)
        {
        }

        private void Videoct_MediaEnded(object sender, RoutedEventArgs e)
        {
            IsPlaying = false;
            videoct.Stop();
        }

        private void VideoViewWindow_Loaded(object sender, RoutedEventArgs e)
        {
            if (this.VideoUrl != null)
                videoct.Play();
        }

        private void Videoct_MediaOpened(object sender, RoutedEventArgs e)
        {
            ShowPlayOrPauseCT = true;
            IsPlaying = true;
            var totaltimes = videoct.NaturalDuration.TimeSpan;
            TotalTime = $"{totaltimes.Hours.ToString("D2")}:{totaltimes.Minutes.ToString("D2")}:{totaltimes.Seconds.ToString("D2")}";
            _timer = new DispatcherTimer();
            _timer.Interval = TimeSpan.FromMilliseconds(200);
            _timer.Tick += new EventHandler(timer_tick);
            _timer.Start();
        }

        private void timer_tick(object sender, EventArgs e)
        {
            var positionval = videoct.Position;
            if (positionval == null)
                return;

            ElapsedStr = $"{positionval.Hours.ToString("D2")}:{positionval.Minutes.ToString("D2")}:{positionval.Seconds.ToString("D2")}";
            if(videoct.NaturalDuration.HasTimeSpan)
                PlayingProgress = videoct.Position.TotalMilliseconds / videoct.NaturalDuration.TimeSpan.TotalMilliseconds;
        }

        private void Play_Btn_Click(object sender, RoutedEventArgs e)
        {
            IsPlaying = true;
            videoct.Play();
        }

        private void Pause_Btn_Click(object sender, RoutedEventArgs e)
        {
            IsPlaying = false;
            videoct.Pause();
        }

        private void NotifyPropertyChange([CallerMemberName] string prop = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));
        }

        private void Slider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            if (IsPlaying)
                return;
        }
    }
}
