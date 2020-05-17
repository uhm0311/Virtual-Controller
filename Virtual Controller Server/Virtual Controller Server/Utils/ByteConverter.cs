using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VirtualControllerServer.Utils
{
    public static class ByteConverter
    {
        public static readonly System.Text.Encoding EUCKR = System.Text.Encoding.GetEncoding(51949);

        private static string byteArrayToHex(byte[] stream)
        {
            return BitConverter.ToString(stream).Replace("-", "").ToUpper();
        }
        private static byte[] stringToByteArray(string str)
        {
            return EUCKR.GetBytes(str);
        }
        private static byte[] hexToByteArray(string hex)
        {
            return Enumerable.Range(0, hex.Length).Where(x => x % 2 == 0).Select(x => Convert.ToByte(hex.Substring(x, 2), 16)).ToArray();
        }

        public static string stringToHex(string str)
        {
            return byteArrayToHex(stringToByteArray(str));
        }
        public static string hexToString(string hex)
        {
            return EUCKR.GetString(hexToByteArray(hex));
        }
    }
}
