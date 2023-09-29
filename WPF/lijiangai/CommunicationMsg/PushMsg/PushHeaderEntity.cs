using System.Runtime.InteropServices;

namespace AIVisualwfpnew.CommunicationMsg.PushMsg
{
    /// <summary>
    /// 推送消息头
    /// </summary>
    [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
    public struct PushHeaderEntity
    {
        /// <summary>
        /// 消息类型，对应枚举值
        /// </summary>
        public ushort MsgType;
        /// <summary>
        /// 数据总长度
        /// </summary>
        public int DataLength;
    }
}
