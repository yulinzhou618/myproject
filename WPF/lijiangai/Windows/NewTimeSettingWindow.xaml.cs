using AIVisualwfpnew.Helpers;
using AIVisualwfpnew.myClass;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
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

namespace AIVisualwfpnew.Windows
{
    /// <summary>
    /// NewTimeSettingWindow.xaml 的交互逻辑
    /// </summary>
    public partial class NewTimeSettingWindow : Window, INotifyPropertyChanged
    {
        public NewTimeSettingWindow()
        {
            LunDuiPengConfirmCommand = new RelayCommand(LunDuiPengConfirmBtnHandler);
            ChuruduanConfirmCommand = new RelayCommand(ChuruduanConfirmBtnHandler);
            AllConfirmCommand = new RelayCommand(AllConfirmBtnHandler);
            SystemTimeEditCommand = new RelayCommand(SysTimeEditHandler);
            SystemTimeSaveCommand = new RelayCommand(SystemTimeSaveHandler);
            InitializeComponent();
            this.Loaded += NewTimeSettingWindow_Loaded;
        }



        private void NewTimeSettingWindow_Loaded(object sender, RoutedEventArgs e)
        {
            IsLunDuiPengEditable = false;
            IsentryEditable = false;
            IsSystemTimeEditable = false;
            InitSystemTime();
            InitConfigWorkingTime();
        }

        private async void InitSystemTime()
        {
            var url = new Uri(GlobalConfig.HostServer + "/GetTime");
            System.Net.Http.HttpClient httpClient = new System.Net.Http.HttpClient();
            var response = await httpClient.PostAsync(url, new JsonContent(""));
            if (!response.IsSuccessStatusCode)
            {
                MessageBox.Show("获取系统时间失败", "错误", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            var jsonStr = await response.Content.ReadAsStringAsync();
            if (string.IsNullOrEmpty(jsonStr))
            {
                MessageBox.Show("获取系统时间失败", "错误", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            var temp = JsonConvert.DeserializeObject<Newtonsoft.Json.Linq.JObject>(jsonStr);
            if (temp == null || !temp.ContainsKey("date"))
            {
                MessageBox.Show("获取系统时间失败", "错误", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            if (!temp.TryGetValue("date", out Newtonsoft.Json.Linq.JToken jt))
                return;

            var valStr = jt.ToString();

            if (string.IsNullOrEmpty(valStr) ||!DateTime.TryParse(valStr, out DateTime n))
            {
                MessageBox.Show("获取系统时间失败", "错误", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            SystemTime = n;
        }

        private async void InitConfigWorkingTime()
        {
            var url = new Uri(GlobalConfig.HostServer + "/GetInvasionWorkTime");
            System.Net.Http.HttpClient httpClient = new System.Net.Http.HttpClient();
            var response = await httpClient.PostAsync(url, new JsonContent(""));
            if (!response.IsSuccessStatusCode)
                return;

            var jsonStr = await response.Content.ReadAsStringAsync();
            if (string.IsNullOrEmpty(jsonStr))
                return;

            var conf = JsonConvert.DeserializeObject<InvasionWoringTimeConfig>(jsonStr);
            if (conf == null)
                return;

            LunDuiPengStartTime = conf.LunDuiPengStartTime==default(DateTime)?DateTime.Now: conf.LunDuiPengStartTime;
            EntryStartTime = conf.ChuRuDuanStartTime == default(DateTime) ? DateTime.Now : conf.ChuRuDuanStartTime;
            LunDuiPengEndTime = conf.LunDuiPengEndTime == default(DateTime) ? DateTime.Now : conf.LunDuiPengEndTime;
            EntryEndTime = conf.ChuRuDuanEndTime == default(DateTime) ? DateTime.Now : conf.ChuRuDuanEndTime;
        }

        private DateTime? _lunDuiPengStartTime;

        public DateTime? LunDuiPengStartTime
        {
            get { return _lunDuiPengStartTime; }
            set { _lunDuiPengStartTime = value; PropertyChangedHandler(); }
        }


        private DateTime? _lunDuiPengEndTime;

        public DateTime? LunDuiPengEndTime
        {
            get { return _lunDuiPengEndTime; }
            set { _lunDuiPengEndTime = value; PropertyChangedHandler(); }
        }

        private DateTime? _entryStartTime;

        public DateTime? EntryStartTime
        {
            get { return _entryStartTime; }
            set { _entryStartTime = value; PropertyChangedHandler(); }
        }

        private DateTime? _entryEndTime;

        public DateTime? EntryEndTime
        {
            get { return _entryEndTime; }
            set { _entryEndTime = value; PropertyChangedHandler(); }
        }

        private DateTime _timeNow;

        public DateTime SystemTime
        {
            get { return _timeNow; }
            set { _timeNow = value; PropertyChangedHandler(); }
        }

        private bool _isSystemTimeEditable;

        public bool IsSystemTimeEditable
        {
            get { return _isSystemTimeEditable; }
            set { _isSystemTimeEditable = value; PropertyChangedHandler(); }
        }

        private bool _isLunDuiPengEditable;

        public bool IsLunDuiPengEditable
        {
            get { return _isLunDuiPengEditable; }
            set { _isLunDuiPengEditable = value; PropertyChangedHandler(); }
        }

        private bool _isentryEditable;

        public bool IsentryEditable
        {
            get { return _isentryEditable; }
            set { _isentryEditable = value; PropertyChangedHandler(); }
        }

        public ICommand LunDuiPengConfirmCommand { get; set; }

        public ICommand ChuruduanConfirmCommand { get; set; }

        public ICommand AllConfirmCommand { get; set; }

        public ICommand SystemTimeEditCommand { get; set; }

        public ICommand SystemTimeSaveCommand { get; set; }


        public event PropertyChangedEventHandler PropertyChanged;


        public void PropertyChangedHandler([CallerMemberName] string prop = "") => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));

        private void ChuruduanConfirmBtnHandler(object obj)
        {
            IsentryEditable = !IsentryEditable;
        }

        private void LunDuiPengConfirmBtnHandler(object obj)
        {
            IsLunDuiPengEditable = !IsLunDuiPengEditable;
        }
        private void SysTimeEditHandler(object obj)
        {
            IsSystemTimeEditable = !IsSystemTimeEditable;
        }

        private async void SystemTimeSaveHandler(object obj)
        {
            if (SystemTime == null)
                return;

            var url = new Uri(GlobalConfig.HostServer + "/SetSysTime");
            System.Net.Http.HttpClient httpClient = new System.Net.Http.HttpClient();
            var response = await httpClient.PostAsync(url, new JsonContent(JsonConvert.SerializeObject(new { time = SystemTime.ToString("yyyy-MM-dd HH:mm:ss") })));
            if (!response.IsSuccessStatusCode)
            {
                MessageBox.Show("系统时间校准失败", "异常", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
        }

        /// <summary>
        /// 保存入侵检测工作时间段配置信息到服务器
        /// </summary>
        /// <param name="obj"></param>
        private async void AllConfirmBtnHandler(object obj)
        {
            // 假如系统校准未保存，这里没处理的哈…
            InvasionWoringTimeConfig invasionWoringTimeConfig = new InvasionWoringTimeConfig()
            {
                LunDuiPengStartTime = this.LunDuiPengStartTime,
                LunDuiPengEndTime = this.LunDuiPengEndTime,
                ChuRuDuanStartTime = this.EntryStartTime,
                ChuRuDuanEndTime = this.EntryEndTime
            };

            var url = new Uri(GlobalConfig.HostServer + "/SetInvasionWorkTime");
            System.Net.Http.HttpClient httpClient = new System.Net.Http.HttpClient();
            var jsonSetting = new JsonSerializerSettings();
            IsoDateTimeConverter timeFormat = new IsoDateTimeConverter();
            timeFormat.DateTimeFormat = "yyyy-MM-dd HH:mm:ss";
            jsonSetting.Converters.Add(timeFormat);
            var response = await httpClient.PostAsync(url, new JsonContent(JsonConvert.SerializeObject(invasionWoringTimeConfig, jsonSetting)));
            if (!response.IsSuccessStatusCode)
            {
                MessageBox.Show("保存配置失败", "异常", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            this.Close();
            MessageBox.Show("配置成功", "提示", MessageBoxButton.OK, MessageBoxImage.Information);
        }


        [Serializable]
        private class InvasionWoringTimeConfig
        {
            [JsonProperty("lunduipengStart")]
            public DateTime? LunDuiPengStartTime { get; set; }

            [JsonProperty("lunduipengEnd")]
            public DateTime? LunDuiPengEndTime { get; set; }

            [JsonProperty("churuduanStart")]
            public DateTime? ChuRuDuanStartTime { get; set; }

            [JsonProperty("churuduanEnd")]
            public DateTime? ChuRuDuanEndTime { get; set; }
        }
    }
}
