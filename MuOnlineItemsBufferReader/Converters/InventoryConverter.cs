using System;
using System.Linq;
using System.Text;

namespace MuOnlineItemsBufferReader.Converters
{
    internal class InventoryConverter : IBufferConverter
    {
        public string Convert(string input, int itemSizeUInt8)
        {
            if (input.StartsWith("0x"))
            {
                input = input.Remove(0, 2);
            }

            var outputString = new StringBuilder();

            var itemSize = itemSizeUInt8 * 2;

            outputString.Append(
                $"ID\tLvl\tDur\tSerial\tExc\tAnc\tSegm.\tPink\tHarm.\tS1\tS2\tS3\tS4\tS5" + Environment.NewLine);

            for (var i = 0; i < input.Length; i += itemSize)
            {
                if (i + itemSize > input.Length - 1)
                {
                    outputString.Append(input.Substring(i, input.Length - i));
                    break;
                }
                
                var rawLine = input.Substring(i, itemSize);

                if(rawLine.All(q => q == 'F'))
                {
                    continue;
                }

                var buildLine = string.Empty;
                Reduce(ref buildLine, ref rawLine, 2);
                Reduce(ref buildLine, ref rawLine, 2);
                Reduce(ref buildLine, ref rawLine, 2);
                Reduce(ref buildLine, ref rawLine, 8);
                Reduce(ref buildLine, ref rawLine, 2);
                Reduce(ref buildLine, ref rawLine, 2);
                Reduce(ref buildLine, ref rawLine, 1);
                Reduce(ref buildLine, ref rawLine, 1);
                Reduce(ref buildLine, ref rawLine, 2);
                Reduce(ref buildLine, ref rawLine, 2);
                Reduce(ref buildLine, ref rawLine, 2);
                Reduce(ref buildLine, ref rawLine, 2);
                Reduce(ref buildLine, ref rawLine, 2);
                Reduce(ref buildLine, ref rawLine, 2);
                Reduce(ref buildLine, ref rawLine, rawLine.Length);

                void Reduce(ref string builtLine, ref string raw, int shift)
                {
                    builtLine += raw[..shift];
                    builtLine += "\t";
                    raw = raw.Remove(0, shift);
                }
                
                outputString.Append(buildLine + Environment.NewLine);
            }

            return outputString.ToString();
        }
    }
}