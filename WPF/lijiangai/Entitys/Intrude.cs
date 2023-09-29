using Newtonsoft.Json;
using System;
using System.ComponentModel;

namespace AIVisualwfpnew.Entitys
{
    public enum InvasionType
    {
        /// <summary>
        /// 人
        /// </summary>
        [Description("人")]
        Person = 0,
        /// <summary>
        /// 牲畜
        /// </summary>
        [Description("牲畜")]
        Livestock = 1,
        /// <summary>
        /// 其它
        /// </summary>
        [Description("其它")]
        Others = 99,
    }

    public class Intrude
    {
        public int ID { get; set; }

        public int Positionid { get; set; }

        /// <summary>
        /// 地址
        /// </summary>
        public string PositionName { get; set; }

        /// <summary>
        /// 时间
        /// </summary>
        public DateTime Invasiontime { get; set; }

        /// <summary>
        /// 入侵种类
        /// </summary>
        [JsonProperty("invasionType")]
        public InvasionType IntrudeCatogroy { get; set; }

        /// <summary>
        /// 入侵抓图地址
        /// </summary>
        [JsonProperty("picturefilename")]
        public string PictrueUrl { get; set; }

        /// <summary>
        /// 入侵视频流地址
        /// </summary>
        [JsonProperty("videofilename")]
        public string VideoUrl { get; set; }
    }
}
