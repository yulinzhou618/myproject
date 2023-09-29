using AIVisualwfpnew.myClass;
using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Controls.Primitives;
using System.Windows.Input;

namespace AIVisualwfpnew.Controlers
{
    public class PageNationEventArgs : RoutedEventArgs
    {
        public PageNationEventArgs(RoutedEvent routedEvent) : base(routedEvent)
        {
        }

        public int PageIndex { get; set; }

        public int PageSize { get; set; }
    }

    /// <summary>
    /// 翻页路由事件
    /// </summary>
    /// <param name="sender">触发者</param>
    /// <param name="e">路由参数</param>
    public delegate void PageNationEventHandler(object sender, PageNationEventArgs e);

    /// <summary>
    /// PagerControl.xaml 的交互逻辑
    /// </summary>
    public partial class PagerControl : UserControl, INotifyPropertyChanged
    {
        private int _totalPage = 0;

        public int TotalPage
        {
            get { return _totalPage; }
            set { _totalPage = value; NotifyPropertyChanged(); }
        }

        public ICommand PageItemClick { get; set; }
        public ICommand PageUpCommand { get; set; }
        public ICommand PageDownCommand { get; set; }

        private ObservableCollection<PageItemInfo> _pageButtons;

        public ObservableCollection<PageItemInfo> PageButtons
        {
            get { return _pageButtons; }
            set { _pageButtons = value; NotifyPropertyChanged(); }
        }

        public int PageIndex
        {
            get { return (int)GetValue(PageIndexProperty); }
            set { SetValue(PageIndexProperty, value); }
        }

        public static readonly DependencyProperty PageIndexProperty =
            DependencyProperty.Register("PageIndex", typeof(int), typeof(PagerControl), new PropertyMetadata(1));

        public int PageSize
        {
            get { return (int)GetValue(PageSizeProperty); }
            set { SetValue(PageSizeProperty, value); }
        }

        public static readonly DependencyProperty PageSizeProperty =
            DependencyProperty.Register("PageSize", typeof(int), typeof(PagerControl), new PropertyMetadata(10));


        public static readonly RoutedEvent PagenationEvent = EventManager.RegisterRoutedEvent
            ("Pagenation", RoutingStrategy.Bubble, typeof(PageNationEventHandler), typeof(ButtonBase));

        // 翻页事件
        public event PageNationEventHandler Pagenation
        {
            add { this.AddHandler(PagenationEvent, value); }
            remove { this.RemoveHandler(PagenationEvent, value); }
        }

        public int TotalDataCount
        {
            get { return (int)GetValue(TotalDataCountProperty); }
            set { SetValue(TotalDataCountProperty, value); }
        }

        public static readonly DependencyProperty TotalDataCountProperty =
            DependencyProperty.Register("TotalDataCount", typeof(int), typeof(PagerControl), new PropertyMetadata(0, new PropertyChangedCallback(TotalDataCountChangeCB)));

        private static void TotalDataCountChangeCB(DependencyObject d, DependencyPropertyChangedEventArgs e)
        {
            if (!(d is PagerControl pc))
                return;

            if (pc.TotalDataCount <= 0)
                pc.IsEnabled = false;
            else
                pc.IsEnabled = true;

            pc.TotalPage = (int)Math.Ceiling(pc.TotalDataCount / (double)pc.PageSize);
            pc.RefreshPageIndex();
        }

        public PagerControl()
        {
            InitializeComponent();
            this.DataContext = this;
            PageItemClick = new RelayCommand(PageItemClickHandler);
            PageUpCommand = new RelayCommand(PageUpHandler);
            PageDownCommand = new RelayCommand(PageDownHandler);
            PageButtons = new ObservableCollection<PageItemInfo>();
            PageIndex = 1;
            PageSize = 20;
            this.Loaded += PagerControl_Loaded;
        }

        private void PagerControl_Loaded(object sender, RoutedEventArgs e)
        {
            RefreshPageIndex();
        }

        /// <summary>
        /// 指定数值按钮点击事件。
        /// </summary>
        /// <param name="obj"></param>
        private void PageItemClickHandler(object obj)
        {
            if (obj == null || !(obj is int pageindex))
                return;

            PageIndex = pageindex;
            RaisePageNationEvent();
        }

        private void PageUpHandler(object obj)
        {
            if (PageIndex - 1 < 1)
                return;

            PageIndex -= 1;
            RaisePageNationEvent();
        }

        private void PageDownHandler(object obj)
        {
            if (PageIndex + 1 > this.TotalPage)
                return;

            PageIndex += 1;
            RaisePageNationEvent();
        }

        private void RaisePageNationEvent()
        {
            PageNationEventArgs args = new PageNationEventArgs(PagenationEvent);
            args.Source = this;
            args.PageIndex = this.PageIndex;
            args.PageSize = this.PageSize;
            this.RaiseEvent(args);
            RefreshPageIndex();
        }

        /// <summary>
        /// 刷新页码,最多五个页码按钮
        /// </summary>
        private void RefreshPageIndex()
        {
            PageButtons.Clear();
            int pageStartIndex = PageIndex;
            int pageEndIndex = TotalPage > 5 ? 5 : TotalPage;
            if (TotalPage > 5)
            {
                pageStartIndex = PageIndex - 2;
                if (pageStartIndex < 1)
                    pageStartIndex = 1;
                pageEndIndex = pageStartIndex + 4;

                if (pageEndIndex > TotalPage)
                    pageEndIndex = TotalPage;

                if (pageEndIndex - pageStartIndex != 4)
                    pageStartIndex = pageEndIndex - 4;
            }
            else
            {
                pageStartIndex = 1;
                pageEndIndex = TotalPage;
            }



            for (int i = pageStartIndex; i < pageEndIndex + 1; i++)
                PageButtons.Add(new PageItemInfo() { Index = i, IsSelected = PageIndex == i });
        }

        public static bool GetIsSelectedItemBtn(DependencyObject obj)
        {
            return (bool)obj.GetValue(IsSelectedItemBtnProperty);
        }

        public static void SetIsSelectedItemBtn(DependencyObject obj, bool value)
        {
            obj.SetValue(IsSelectedItemBtnProperty, value);
        }

        public static readonly DependencyProperty IsSelectedItemBtnProperty =
            DependencyProperty.RegisterAttached("IsSelectedItemBtn", typeof(bool), typeof(PagerControl), new PropertyMetadata(false));

        public event PropertyChangedEventHandler PropertyChanged;

        public void NotifyPropertyChanged([CallerMemberName] string prop = "") => PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(prop));

        /// <summary>
        /// 跳转到指定页
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void TextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (!(sender is TextBox tb))
                return;

            if (string.IsNullOrEmpty(tb.Text))
                return;

            if (!int.TryParse(tb.Text, out int targetIndex))
            {
                tb.Text = string.Empty;
                return;
            }

            if (targetIndex < 1 || targetIndex > TotalPage)
            {
                tb.Text = string.Empty;
                return;
            }

            PageIndex = targetIndex;
            tb.Text = string.Empty;
            RaisePageNationEvent();
        }

        public class PageItemInfo
        {
            public int Index { get; set; }
            public bool IsSelected { get; set; }
        }
    }
}
