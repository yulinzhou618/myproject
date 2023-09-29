using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Windows;

namespace AIVisualwfpnew.Entitys
{
    [Serializable]
    public class PositionInvasionConfigInfo
    {
        [JsonProperty("id")]
        public int ID { get; set; }

        [JsonProperty("positionName")]
        public string PositionName { get; set; }

        [JsonProperty("zoneJson")]
        public string ConfigStr { get; set; }

        [JsonIgnore]
        public List<Point> Points
        {
            get
            {
                List<Point> result = new List<Point>();
                JArray jarrary = JsonConvert.DeserializeObject<JArray>(ConfigStr);
                if (jarrary == null)
                    return result;

                foreach (var item in jarrary)
                {
                    result.Add(new Point()
                    {
                        X = item["x"].Value<double>(),
                        Y = item["y"].Value<double>()
                    });
                }

                return result;
            }
        }
    }
}
