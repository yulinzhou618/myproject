using System.Configuration;

namespace AIVisualwfpnew.Helpers
{
    public static class GlobalConfig
    {
        private static string _hostServer = ConfigurationManager.AppSettings["serverHost"];

        public static string HostServer
        {
            get
            {
                if (string.IsNullOrEmpty(_hostServer))
                    _hostServer = "http://10.12.44.22:7080";
                return _hostServer;
            }
            set { _hostServer = value; }
        }


        private static string _pushServer = ConfigurationManager.AppSettings["pushHost"];
        /// <summary>
        /// 推送服务端IP地址
        /// </summary>
        public static string PushHost
        {
            get
            {
                if (string.IsNullOrEmpty(_pushServer))
                    _pushServer = "10.12.44.22";
                return _pushServer;
            }
            set
            {
                _pushServer = value;
            }
        }

        private static int _pushPort = 7090;
        /// <summary>
        /// 推送服务端端口号
        /// </summary>
        public static int PushPort
        {
            get
            {
                var portstr = ConfigurationManager.AppSettings["pushPort"];
                if (string.IsNullOrEmpty(portstr))
                    return _pushPort;

                if (int.TryParse(portstr, out int r))
                    _pushPort = r;

                return _pushPort;
            }
            set { _pushPort = value; }
        }

        private static string _serverHostSourceUrl = ConfigurationManager.AppSettings["serverHostSourceUrl"];

        public static string ServerHostSourceUrl
        {
            get
            {
                if (string.IsNullOrEmpty(_pushServer))
                    _serverHostSourceUrl = "http://10.12.44.22:7080/static";
                return _serverHostSourceUrl;
            }
            set { _serverHostSourceUrl = value; }
        }

        private static string _speakerHost = ConfigurationManager.AppSettings["speakerurl"];

        public static string SpeakerHost
        {
            get
            {
                if (string.IsNullOrEmpty(_speakerHost))
                    _speakerHost = "http://10.12.44.1:80";
                return _speakerHost;
            }
            set { _speakerHost = value; }
        }

        private static string lunduipengspeakerid = ConfigurationManager.AppSettings["lunduipengid"];

        public static string LunDuiPengSpeakerID
        {
            get
            {
                if (string.IsNullOrEmpty(lunduipengspeakerid))
                    lunduipengspeakerid = "2";
                return lunduipengspeakerid;
            }
            set { lunduipengspeakerid = value; }
        }

        private static string churuduanspeakerid = ConfigurationManager.AppSettings["churuduanid"];

        public static string ChuRuDuanSpeakerID
        {
            get
            {
                if (string.IsNullOrEmpty(churuduanspeakerid))
                    churuduanspeakerid = "1";
                return churuduanspeakerid;
            }
            set { churuduanspeakerid = value; }
        }
    }
}
