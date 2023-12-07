﻿using System;

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
    }
}