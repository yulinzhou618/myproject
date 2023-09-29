using AIVisualwfpnew.Entitys;
using AIVisualwfpnew.Helpers;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AIVisualwfpnew.CommunicationMsg.PushMsg
{
    /// <summary>
    /// 接收入侵推送消息的实体
    /// </summary>
    [Serializable]
    public class ReceivedMessageEntity
    {
        /// <summary>
        /// 入侵位置的ID
        /// </summary>
        [JsonProperty("positionid")]
        public int PositionID { get; set; }

        /// <summary>
        /// 位置名称
        /// </summary>
        [JsonProperty("positionName")]
        public string PositionName { get; set; }

        /// <summary>
        /// 入侵时的图片文件名称
        /// </summary>
        [JsonProperty("img")]
        public string ImageFileName { get; set; }

        /// <summary>
        /// 入侵发生时间
        /// </summary>
        [JsonProperty("time")]
        public DateTime Time { get; set; }

        /// <summary>
        /// 入侵种类
        /// </summary>
        [JsonProperty("type")]
        public InvasionType InvasionType { get; set; }

        /// <summary>
        /// 入侵种类描述字符串
        /// </summary>
        [JsonIgnore]
        public string InvasionTypeString
        {
            get
            {
                return InvasionType.GetDescription();
            }
        }

        /// <summary>
        /// 入侵次数
        /// </summary>
        [JsonProperty("count")]
        public int InvasionCount { get; set; }
    }
}
