using AIVisualwfpnew.CommunicationMsg.PushMsg;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Runtime.InteropServices;
using System.Threading.Tasks;

namespace AIVisualwfpnew.Helpers
{
    /// <summary>
    /// 消息回调委托
    /// </summary>
    /// <param name="header">消息头</param>
    /// <param name="body">消息体</param>
    public delegate void MessageByteHandler(PushHeaderEntity header, byte[] body);

    /// <summary>
    /// 消息回调委托
    /// </summary>
    /// <param name="header">消息头</param>
    /// <param name="body">消息体</param>
    public delegate void MessageStringHandler(PushHeaderEntity header, string body);

    /// <summary>
    /// 推送客户端，用于链接推送服务，且接受消息。暂时不支持分段接收，使用时注意，如果单次消息过大，可能会内存溢出
    /// 支持多消息一次发送，本客户端会自动拆解多包。
    /// </summary>
    public class PushClient
    {
        private Socket _socket;
        private ConcurrentDictionary<PushType, List<Delegate>> _handlers;
        public PushClient()
        {
            _socket = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            _handlers = new ConcurrentDictionary<PushType, List<Delegate>>();
        }

        /// <summary>
        /// 开启链接
        /// </summary>
        public void StartClient() => Task.Factory.StartNew(Handler);

        /// <summary>
        /// 连接服务端，并且接受消息。
        /// </summary>
        private async void Handler()
        {
            try
            {
                IPAddress address = IPAddress.Parse(GlobalConfig.PushHost);
                IPEndPoint endPoint = new IPEndPoint(address, GlobalConfig.PushPort);
                _socket.Connect(endPoint);
                while (true)
                {
                    try
                    {
                        if (_socket.Connected)
                        {
                            #region 接受消息头bytes
                            int totalHeaderReceivedLenght = 0;
                            int structLenght = Marshal.SizeOf<PushHeaderEntity>();
                            byte[] _buffer = new byte[structLenght];
                            do
                            {
                                int receivedLength = _socket.Receive(_buffer, totalHeaderReceivedLenght, structLenght - totalHeaderReceivedLenght, SocketFlags.None);
                                if (receivedLength <= 0)
                                {
                                    await Task.Delay(500);
                                    continue;
                                }
                                totalHeaderReceivedLenght += receivedLength;
                            } while (totalHeaderReceivedLenght < structLenght);

                            #endregion

                            // 解开消息头
                            var msgHeader = ByteUtil.BytesToStuct<PushHeaderEntity>(_buffer);

                            #region 接收消息体数据
                            byte[] msgAllBytes = new byte[msgHeader.DataLength];
                            if (msgHeader.DataLength > 0)
                            {
                                // 接收消息体部分。
                                int totalbodyreceivedlength = 0;
                                do
                                {
                                    int tbodylenth = _socket.Receive(msgAllBytes, totalbodyreceivedlength, msgHeader.DataLength - totalbodyreceivedlength, SocketFlags.None);
                                    if (tbodylenth <= 0)
                                    {
                                        await Task.Delay(500);
                                        continue;
                                    }
                                    totalbodyreceivedlength += tbodylenth;
                                } while (totalbodyreceivedlength < msgHeader.DataLength);
                            }
                            #endregion

                            if (!Enum.TryParse(msgHeader.MsgType.ToString(), out PushType pushType))
                                continue;

                            // 触发消息事件，继续接收下一个消息。
                            if (!_handlers.TryGetValue(pushType, out List<Delegate> handlers))
                                continue;

                            foreach (var handler in handlers)
                            {
                                try
                                {
                                    switch (handler)
                                    {
                                        case MessageByteHandler bytehandler:
                                            handler.DynamicInvoke(msgHeader, msgAllBytes);
                                            break;
                                        case MessageStringHandler strhandler:
                                            handler.DynamicInvoke(msgHeader, System.Text.Encoding.UTF8.GetString(msgAllBytes));
                                            break;
                                    }

                                }
                                catch (Exception ex)
                                {
                                    LogHelper.Log.Error($"推送消息执行时异常：", ex);
                                }
                            }
                        }
                        else
                        {
                            // 都没连接上，无法接受消息，直接退出。
                            LogHelper.Log.Error($"无法连上推送服务，推送功能自动关闭：");
                            return;
                        }
                    }
                    catch (System.Exception ex)
                    {
                        LogHelper.Log.Error($"解析推送服务给的数据时异常，推送功能自动关闭：", ex);
                        return;
                    }
                }
            }
            catch (Exception ex)
            {
                LogHelper.Log.Error(ex);
            }
        }

        public void RegisterByteMessage(PushType pushType, MessageByteHandler handler)
        {
            if (_handlers.ContainsKey(pushType))
            {
                if (!_handlers.TryGetValue(pushType, out List<Delegate> handlers))
                    throw new Exception("消息注册失败");

                handlers.Add(handler);
            }
            else
            {
                _handlers.TryAdd(pushType, new List<Delegate>() { handler });
            }
        }

        public void RegisterStringMessage(PushType pushType, MessageStringHandler handler)
        {
            if (_handlers.ContainsKey(pushType))
            {
                if (!_handlers.TryGetValue(pushType, out List<Delegate> handlers))
                    throw new Exception("消息注册失败");

                handlers.Add(handler);
            }
            else
            {
                _handlers.TryAdd(pushType, new List<Delegate>() { handler });
            }
        }

        public void RemoveMessage(PushType pushType, Delegate handler)
        {
            if (!_handlers.ContainsKey(pushType))
                return;

            if (!_handlers.TryGetValue(pushType, out List<Delegate> handlers))
                return;

            handlers.Remove(handler);
        }
    }
}
