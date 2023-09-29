namespace AIVisualwfpnew.Helpers
{
    /// <summary>
    /// 管理所有推送客户端，目前只有一个。
    /// </summary>
    public static class PushManager
    {
        public static PushClient Current { get; private set; }

        static PushManager()
        {
            Current = new PushClient();
        }
    }
}
