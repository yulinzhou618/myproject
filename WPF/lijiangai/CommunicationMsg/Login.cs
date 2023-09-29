using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AIVisualwfpnew.CommunicationMsg
{
    [Serializable]
    public class Login
    {
        /// <summary>
        /// 账号
        /// </summary>
        [JsonProperty("account")]
        public string Account { get; set; }

        /// <summary>
        /// 密码
        /// </summary>
        [JsonProperty("pwd")]
        public string Password { get; set; }

    }
}
