using System;
using System.Collections;
using System.Globalization;

namespace MuOnlineItemsBufferReader
{
    public static class Utils
    {
        public static byte[] ToByteArray(String HexString)
        {
            int NumberChars = HexString.Length;
            byte[] bytes = new byte[NumberChars / 2];
            for (int i = 0; i < NumberChars; i += 2)
            {
                bytes[i / 2] = Convert.ToByte(HexString.Substring(i, 2), 16);
            }

            return bytes;
        }

        public static string HexToDec(string hex)
        {
            return int.Parse(hex, System.Globalization.NumberStyles.HexNumber).ToString();
        }

        public static BitArray ConvertHexToBitArray(string hexData)
        {
            if (hexData == null)
                return null; // or do something else, throw, ...

            BitArray ba = new BitArray(4 * hexData.Length);
            for (int i = 0; i < hexData.Length; i++)
            {
                byte b = byte.Parse(hexData[i].ToString(), NumberStyles.HexNumber);
                for (int j = 0; j < 4; j++)
                {
                    ba.Set(i * 4 + j, (b & (1 << (3 - j))) != 0);
                }
            }
            return ba;
        }

        public static string PrintBits(this BitArray bitArray)
        {
            var print = string.Empty;
            for (var i = 0; i < bitArray.Length; i++)
            {
                print += bitArray[i] ? 1 : 0;
            }

            return print;
        }
    }
}