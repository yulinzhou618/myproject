namespace AIVisualwfpnew.CommunicationMsg
{
    public class InvasionSearch
    {
        public int PageIndex { get; set; }

        public int PageSize { get; set; }

        public string StartTime { get; set; }

        public string EndTime { get; set; }

        public int? PositionId { get; set; }

        public int? Type { get; set; }
    }
}
