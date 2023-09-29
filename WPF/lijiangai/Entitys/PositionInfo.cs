namespace AIVisualwfpnew.Entitys
{
    public class PositionInfo
    {
        public int ID { get; set; }

        public string PositionName { get; set; }

        public override string ToString()
        {
            return this.PositionName;
        }
    }
}
