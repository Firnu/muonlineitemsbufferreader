using System;
using System.Linq;
using System.Text;

namespace MuOnlineItemsBufferReader.Converters
{
    internal class InventoryConverter : IBufferConverter
    {
        public string Convert(string input, int itemSizeUInt8, ValueFormat valueFormat)
        {
            if (input.StartsWith("0x"))
            {
                input = input.Remove(0, 2);
            }

            var outputString = new StringBuilder();

            var itemSize = itemSizeUInt8 * 2;

            var headlineSpacing = valueFormat switch
            {
                ValueFormat.Hex => "\t\t",
                ValueFormat.Dec => "\t\t",
                ValueFormat.Bit => "            ",
                _ => throw new ArgumentOutOfRangeException(nameof(valueFormat), valueFormat, null)
            };

            var valueSpacing = valueFormat switch
            {
                ValueFormat.Hex => "\t\t",
                ValueFormat.Dec => "\t\t",
                ValueFormat.Bit => "\t",
                _ => throw new ArgumentOutOfRangeException(nameof(valueFormat), valueFormat, null)
            };

            var serialHeadline = valueFormat == ValueFormat.Bit ? "Serial" : "ItemSerial";

            outputString.Append(
                $"ID{headlineSpacing}" +
                $"Lvl{headlineSpacing}" +
                $"Dur.{headlineSpacing}" +
                $"{serialHeadline}{headlineSpacing}" +
                $"Exc{headlineSpacing}" +
                $"Anc{headlineSpacing}" +
                $"Seg{headlineSpacing}" +
                $"Pink{headlineSpacing}" +
                $"HrmSocO{headlineSpacing}" +
                $"Soc1{headlineSpacing}" +
                $"Soc2{headlineSpacing}" +
                $"Soc3{headlineSpacing}" +
                $"Soc4{headlineSpacing}" +
                $"Soc5{headlineSpacing}" +
                $"Other" +
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
                Reduce(ref buildLine, ref rawLine, rawLine.Length, true);

                void Reduce(ref string builtLine, ref string raw, int shift, bool skipIndent = false)
                {
                    var value = raw[..shift];

                    value = valueFormat switch
                    {
                        ValueFormat.Hex => value,
                        ValueFormat.Dec => shift <= 2 ? Utils.HexToDec(value) : value,
                        ValueFormat.Bit => shift <= 2 ? Utils.ConvertHexToBitArray(value).PrintBits() : value,
                        _ => throw new ArgumentOutOfRangeException(nameof(valueFormat), valueFormat, null)
                    };
                    
                    builtLine += value;

                    if (!skipIndent)
                    {
                        builtLine += valueSpacing;
                    }
                    
                    raw = raw.Remove(0, shift);
                }
                
                outputString.Append(buildLine + Environment.NewLine);
            }

            return outputString.ToString();
        }
    }
}