using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace AIVisualwfpnew.Helpers
{

    [Serializable]
    public class SpeakerResponseData
    {
        [JsonProperty("res")]
        public int State { get; set; }

        [JsonProperty("number")]
        public string Number { get; set; }

        [JsonProperty("guid")]
        public string GUID { get; set; }
    }

    /// <summary>
    /// 世邦音柱帮助类。
    /// </summary>
    public static class SpeakerHelper
    {
        /// <summary>
        /// 文字播报
        /// </summary>
        /// <param name="ids">终端列表</param>
        /// <param name="text">要播报的文字</param>
        /// <param name="count">播报次数</param>
        /// <returns>是否播报完成</returns>
        public static async Task<bool> TextBroadcast(IEnumerable<string> ids, string text, int count = 3)
        {
            string url = GlobalConfig.SpeakerHost + "/php/exeRealPlayFile.php";
            System.Net.Http.HttpClient client = new System.Net.Http.HttpClient();
            List<KeyValuePair<string, string>> nameValueCollection = new List<KeyValuePair<string, string>>();
            nameValueCollection.Add(new KeyValuePair<string, string>("jsondata[rtype]", "startbct"));
            nameValueCollection.Add(new KeyValuePair<string, string>("jsondata[param1]", string.Join("<", ids)));
            nameValueCollection.Add(new KeyValuePair<string, string>("jsondata[param2]", 3.ToString()));
            nameValueCollection.Add(new KeyValuePair<string, string>("jsondata[param4]", text));
            nameValueCollection.Add(new KeyValuePair<string, string>("jsondata[param7]", count.ToString()));
            nameValueCollection.Add(new KeyValuePair<string, string>("jsondata[param8]", 0.ToString()));
            var t = new System.Net.Http.FormUrlEncodedContent(nameValueCollection);
            var response = await client.PostAsync(url, t);
            if (!response.IsSuccessStatusCode)
                return false;

            return true;
        }


        /// <summary>
        /// 启动语音广播
        /// </summary>
        /// <param name="ids">终端列表</param>
        /// <returns>是否启动播报</returns>
        public static async Task<SpeakerResponseData> StartAudioBroadcast(IEnumerable<string> ids)
        {
            string url = GlobalConfig.SpeakerHost + "/php/exeRealPlayFile.php";
            System.Net.Http.HttpClient client = new System.Net.Http.HttpClient();
            List<KeyValuePair<string, string>> nameValueCollection = new List<KeyValuePair<string, string>>();
            nameValueCollection.Add(new KeyValuePair<string, string>("jsondata[rtype]", "startbct"));
            nameValueCollection.Add(new KeyValuePair<string, string>("jsondata[param1]", string.Join("<", ids)));
            nameValueCollection.Add(new KeyValuePair<string, string>("jsondata[param2]", 2.ToString()));
            var t = new System.Net.Http.FormUrlEncodedContent(nameValueCollection);
            var response = await client.PostAsync(url, t);
            if (!response.IsSuccessStatusCode)
                return null;

            var responseStr = await response.Content.ReadAsStringAsync();
            if (string.IsNullOrEmpty(responseStr))
                return null;

            return JsonConvert.DeserializeObject<SpeakerResponseData>(responseStr);
        }

        /// <summary>
        /// 停止语音广播
        /// </summary>
        /// <param name="ids">终端列表</param>
        /// <returns>是否停止播报成功</returns>
        public static async Task<SpeakerResponseData> StopAudioBroadcast(string number, string guid)
        {
            string url = GlobalConfig.SpeakerHost + "/php/exeRealPlayFile.php";
            System.Net.Http.HttpClient client = new System.Net.Http.HttpClient();
            List<KeyValuePair<string, string>> nameValueCollection = new List<KeyValuePair<string, string>>();
            nameValueCollection.Add(new KeyValuePair<string, string>("jsondata[rtype]", "stopbct"));
            nameValueCollection.Add(new KeyValuePair<string, string>("jsondata[param1]", number));
            nameValueCollection.Add(new KeyValuePair<string, string>("jsondata[param2]", guid));
            var t = new System.Net.Http.FormUrlEncodedContent(nameValueCollection);
            var response = await client.PostAsync(url, t);
            if (!response.IsSuccessStatusCode)
                return null;

            var responseStr = await response.Content.ReadAsStringAsync();
            if (string.IsNullOrEmpty(responseStr))
                return null;

            return JsonConvert.DeserializeObject<SpeakerResponseData>(responseStr);
        }
    }
}
