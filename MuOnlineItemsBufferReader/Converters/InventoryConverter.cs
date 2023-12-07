using System;
using System.Linq;
using System.Text;

namespace MuOnlineItemsBufferReader.Converters
{
    internal class InventoryConverter : IBufferConverter
    {
        public string Convert(string input, int itemSizeUInt8, bool convertValueToDec = false)
        {
            if (input.StartsWith("0x"))
            {
                input = input.Remove(0, 2);
            }

            var outputString = new StringBuilder();

            var itemSize = itemSizeUInt8 * 2;

            outputString.Append(
                $"ID\t\tLvl+S+L\t\tDur.\t\tSerial\t\t\tExc\t\tAnc\t\tSegment\t\tPink\t\tHarmony\t\tSocOpt\t\tSoc1\t\tSoc2\t\tSoc3\t\tSoc4\t\tSoc5\t\tOther" +
                Environment.NewLine);

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
                Reduce(ref buildLine, ref rawLine, 2);
                Reduce(ref buildLine, ref rawLine, rawLine.Length, true);

                void Reduce(ref string builtLine, ref string raw, int shift, bool skipIndent = false)
                {
                    var value = raw[..shift];

                    if (shift <= 2 && convertValueToDec)
                    {
                        value = Utils.HexToDec(value);
                    }

                    builtLine += value;

                    if (!skipIndent)
                    {
                        builtLine += "\t\t";
                    }
                    
                    raw = raw.Remove(0, shift);
                }
                
                outputString.Append(buildLine + Environment.NewLine);
            }

            return outputString.ToString();
        }
    }
}