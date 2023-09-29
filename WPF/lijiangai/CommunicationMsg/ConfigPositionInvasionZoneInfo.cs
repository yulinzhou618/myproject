using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace AIVisualwfpnew.CommunicationMsg
{
    [Serializable]
    public class ServerPoint
    {
        [JsonProperty("X")]
        public double X { get; set; }

        [JsonProperty("Y")]
        public double Y { get; set; }
    }

    [Serializable]
    public class ConfigPositionInvasionZoneInfo
    {
        [JsonProperty("id")]
        public int ID { get; set; }

        [JsonProperty("points")]
        public List<ServerPoint> Points { get; set; }
    }
}
