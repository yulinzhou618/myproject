using AIVisualwfpnew.CommunicationMsg;
using AIVisualwfpnew.Controlers;
using AIVisualwfpnew.Entitys;
using AIVisualwfpnew.Helpers;
using AIVisualwfpnew.myClass;
using AIVisualwfpnew.Windows;
using Newtonsoft.Json;
using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace AIVisualwfpnew.ViewModle
{
    public class CommboBoxItemEntity
    {
        public string DisplayName { get; set; }

        public int CategroyId { get; set; }

        public override string ToString()
        {
            return this.DisplayName;
        }
    }

    public class SearchWindowVM : ViewModleBase
    {
        private ObservableCollection<Intrude> _dataGridSources;

        public ObservableCollection<Intrude> DataGridSources
        {
            get { return _dataGridSources; }
            set { _dataGridSources = value; NotityPropertyChange(); }
        }

        private DateTime _startDate;

        public DateTime StartDate
        {
            get { return _startDate; }
            set { _startDate = value; NotityPropertyChange(); }
        }

        private DateTime _dateTime;

        public DateTime EndDate
        {
            get { return _dateTime; }
            set { _dateTime = value; NotityPropertyChange(); }
        }

        private ObservableCollection<CommboBoxItemEntity> _categroy;

        public ObservableCollection<CommboBoxItemEntity> Category
        {
            get { return _categroy; }
            set { _categroy = value; NotityPropertyChange(); }
        }


        private int? _selectedCategory;
        /// <summary>
        /// 界面选中的查询种类
        /// </summary>
        public int? SelectedCategory
        {
            get { return _selectedCategory; }
            set { _selectedCategory = value; NotityPropertyChange(); }
        }

        private ObservableCollection<PositionInfo> _positionSource;

        public ObservableCollection<PositionInfo> PositionSource
        {
            get { return _positionSource; }
            set { _positionSource = value; NotityPropertyChange(); }
        }

        private int? _selectedPositionID;
        /// <summary>
        /// 选中的位置ID。
        /// </summary>
        public int? SelectedPositionID
        {
            get { return _selectedPositionID; }
            set { _selectedPositionID = value; NotityPropertyChange(); }
        }

        private int _totalDataCount;

        public int TotalDataCount
        {
            get { return _totalDataCount; }
            set { _totalDataCount = value; NotityPropertyChange(); }
        }

        public ICommand LoadedHandler { get; set; }

        public ICommand SearchCommand { get; set; }

        public ICommand PageNationCommand { get; set; }

        public ICommand ShowImageCommand { get; set; }

        public ICommand ShowVideoCommand { get; set; }

        public SearchWindowVM()
        {
            DataGridSources = new ObservableCollection<Intrude>();
            this.LoadedHandler = new RelayCommand(LoadedHandlerFunction);
            this.SearchCommand = new RelayCommand(SearchHandler);
            this.PageNationCommand = new RelayCommand(PageNationHandler);
            this.ShowImageCommand = new RelayCommand(ShowImageHandler);
            this.ShowVideoCommand = new RelayCommand(ShowVideoHandler);
            StartDate = DateTime.Now.AddDays(-7);
            EndDate = DateTime.Now.AddDays(7);
            Category = new ObservableCollection<CommboBoxItemEntity>();
            Category.Add(new CommboBoxItemEntity() { CategroyId = 0, DisplayName = "人类" });
            Category.Add(new CommboBoxItemEntity() { CategroyId = 1, DisplayName = "牲畜" });
            Category.Add(new CommboBoxItemEntity() { CategroyId = 99, DisplayName = "其它" });

            Task.Factory.StartNew(async () =>
            {
                var url = new Uri(new Uri(GlobalConfig.HostServer), "GetPositionInfo");
                System.Net.Http.HttpClient httpClient = new System.Net.Http.HttpClient();
                var response = await httpClient.PostAsync(url, new System.Net.Http.StringContent("", Encoding.UTF8));
                if (response.IsSuccessStatusCode)
                {
                    var jsonStr = await response.Content.ReadAsStringAsync();
                    if (string.IsNullOrEmpty(jsonStr))
                        return;

                    _ = Application.Current.Dispatcher.BeginInvoke(new Action(() =>
                    {
                        PositionSource = JsonConvert.DeserializeObject<ObservableCollection<PositionInfo>>(jsonStr);
                    }));
                }
            });
        }

        private void ShowVideoHandler(object obj)
        {
            if (obj == null)
                return;

            if (obj is Intrude intrude && !string.IsNullOrEmpty(intrude.VideoUrl))
            {
                VideoViewWindow videoViewWindow = new VideoViewWindow(new Uri(GlobalConfig.HostServer + "/static/" + intrude.VideoUrl));
                videoViewWindow.Owner = Application.Current.MainWindow;
                videoViewWindow.ShowDialog();
            }
        }

        private async void ShowImageHandler(object obj)
        {
            if (obj == null)
                return;

            if (!(obj is Intrude intrude))
                return;


            var url = new Uri(GlobalConfig.HostServer + "/static/" + intrude.PictrueUrl);
            System.Net.Http.HttpClient httpClient = new System.Net.Http.HttpClient();
            var response = await httpClient.GetAsync(url);

            if (response.IsSuccessStatusCode)
            {
                var responseStrream = await response.Content.ReadAsStreamAsync();
                if (responseStrream == null)
                    return;

                var fileName = Path.GetFileName(intrude.PictrueUrl);
                if (string.IsNullOrEmpty(fileName))
                    fileName = "未知文件名";

                await Application.Current.Dispatcher.BeginInvoke(new Action(() =>
                {
                    ImageViewWindow imageView = new ImageViewWindow(responseStrream) { Title = fileName };
                    imageView.Owner = Application.Current.MainWindow;
                    imageView.ShowDialog();
                }));
            }
        }

        /// <summary>
        /// 翻页按钮事件。
        /// </summary>
        /// <param name="obj"></param>
        /// <exception cref="NotImplementedException"></exception>
        private void PageNationHandler(object obj)
        {
            if (obj == null)
                return;

            if (obj is object[] tp && tp[0] != null && tp[0] is PageNationEventArgs args)
                CurrentPageIndex = args.PageIndex;

            SearchHandler(null);
        }

        public int CurrentPageIndex { get; set; } = 1;

        /// <summary>
        /// 执行查询。
        /// </summary>
        /// <param name="obj"></param>
        private async void SearchHandler(object obj)
        {
            var url = new Uri(new Uri(GlobalConfig.HostServer), "GetInvasionData");
            System.Net.Http.HttpClient httpClient = new System.Net.Http.HttpClient();
            var parameters = new InvasionSearch()
            {
                PageIndex = CurrentPageIndex,
                PageSize = 20,
                StartTime = StartDate.ToString("yyyy-MM-dd HH:mm:ss"),
                EndTime = EndDate.Date.ToString("yyyy-MM-dd HH:mm:ss"),
                PositionId = SelectedPositionID,
                Type = SelectedCategory
            };

            var contentJsonstr = JsonConvert.SerializeObject(parameters);
            var response = await httpClient.PostAsync(url, new JsonContent(contentJsonstr, Encoding.UTF8));

            if (response.IsSuccessStatusCode)
            {
                var responseStr = await response.Content.ReadAsStringAsync();
                if (string.IsNullOrEmpty(responseStr))
                    return;

                _ = Application.Current.Dispatcher.BeginInvoke(new Action(() =>
                {
                    DataGridSources = JsonConvert.DeserializeObject<ObservableCollection<Intrude>>(responseStr);
                    TotalDataCount = DataGridSources.Count;
                }));
            }
        }

        private void LoadedHandlerFunction(object obj)
        {
            SearchHandler(null);
        }
    }
}
