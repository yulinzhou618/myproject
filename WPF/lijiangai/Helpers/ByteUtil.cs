using System;
using System.Runtime.InteropServices;
using System.Text;

namespace AIVisualwfpnew.Helpers
{
    public static class ByteUtil
    {
        /// <summary>
        /// 结构体转为byte数组
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        /// <returns></returns>
        public static byte[] StructToBytes<T>(T obj)
        {
            int size = Marshal.SizeOf(typeof(T));
            IntPtr bufferPtr = Marshal.AllocHGlobal(size);
            try
            {
                Marshal.StructureToPtr(obj, bufferPtr, false);
                byte[] bytes = new byte[size];
                Marshal.Copy(bufferPtr, bytes, 0, size);
                return bytes;
            }
            catch (Exception ex)
            {
                throw new Exception("Error in StructToBytes ! " + ex.Message);
            }
            finally
            {
                Marshal.FreeHGlobal(bufferPtr);
            }
        }

        /// <summary>
        /// byte数组转化为结构体
        /// </summary>
        /// <param name="bytes"></param>
        /// <param name="type"></param>
        /// <returns></returns>
        public static T BytesToStuct<T>(byte[] bytes)
        {
            Type type = typeof(T);
            int size = Marshal.SizeOf(type);
            if (size > bytes.Length)
            {
                return default;
            }
            IntPtr structPtr = Marshal.AllocHGlobal(size);
            Marshal.Copy(bytes, 0, structPtr, size);
            T t = (T)Marshal.PtrToStructure(structPtr, type);
            Marshal.FreeHGlobal(structPtr);
            return t;
        }

        public static string BitForHighToLow(this byte b)
        {
            StringBuilder sb = new StringBuilder();
            for (int k = 0; k < 8; k++)
            {
                var value = (b & (byte)Math.Pow(2, k)) > 0 ? 1 : 0;
                sb.Append(value);
            }
            return sb.ToString();
        }

        /// <summary>
        /// 将二进制转为十六进制字符串
        /// </summary>
        /// <param name="ba"></param>
        /// <returns></returns>
        public static string ByteArrayToString(byte[] ba)
        {
            System.Text.StringBuilder hex = new System.Text.StringBuilder(ba.Length * 2);
            foreach (byte b in ba)
                hex.AppendFormat("{0:x2}", b);
            return hex.ToString();
        }
    }
}
