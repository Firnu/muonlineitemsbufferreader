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

            var serialHeadline = valueFormat == ValueFormat.Bit ? "Serial" : "ItemSerial";

            var column = BuildColumnString();
            outputString.Append(BuildColumnEntry(column, "ID"));
            outputString.Append(BuildColumnEntry(column, "Lvl|Skl|Lck"));
            outputString.Append(BuildColumnEntry(column, "Dur."));
            outputString.Append(BuildColumnEntry(column, "Serial"));
            outputString.Append(BuildColumnEntry(column, "Exc"));
            outputString.Append(BuildColumnEntry(column, "Anc"));
            outputString.Append(BuildColumnEntry(column, "Segment"));
            outputString.Append(BuildColumnEntry(column, "Pink"));
            outputString.Append(BuildColumnEntry(column, "Hrm|SOpt"));
            outputString.Append(BuildColumnEntry(column, "Soc1"));
            outputString.Append(BuildColumnEntry(column, "Soc2"));
            outputString.Append(BuildColumnEntry(column, "Soc3"));
            outputString.Append(BuildColumnEntry(column, "Soc4"));
            outputString.Append(BuildColumnEntry(column, "Soc5"));
            outputString.Append(BuildColumnEntry(column, "Other"));
            outputString.Append(Environment.NewLine);

            for (var i = 0; i < input.Length; i += itemSize)
            {
                if (i + itemSize > input.Length - 1)
                {
                    outputString.Append(input.Substring(i, input.Length - i));
                    break;
                }

                var rawLine = input.Substring(i, itemSize);

                if (rawLine.All(q => q == 'F'))
                {
                    continue;
                }

                var buildLine = string.Empty;
                Reduce(ref rawLine, 2);
                Reduce(ref rawLine, 2);
                Reduce(ref rawLine, 2);
                Reduce(ref rawLine, 8);
                Reduce(ref rawLine, 2);
                Reduce(ref rawLine, 2);
                Reduce(ref rawLine, 1);
                Reduce(ref rawLine, 1);
                Reduce(ref rawLine, 2);
                Reduce(ref rawLine, 2);
                Reduce(ref rawLine, 2);
                Reduce(ref rawLine, 2);
                Reduce(ref rawLine, 2);
                Reduce(ref rawLine, 2);
                Reduce(ref rawLine, rawLine.Length, true);

                void Reduce(ref string raw, int shift, bool lastEntry = false)
                {
                    var value = raw[..shift];

                    value = valueFormat switch
                    {
                        ValueFormat.Hex => value,
                        ValueFormat.Dec => shift <= 2 ? Utils.HexToDec(value) : value,
                        ValueFormat.Bit => shift <= 2 ? Utils.ConvertHexToBitArray(value).PrintBits() : value,
                        _ => throw new ArgumentOutOfRangeException(nameof(valueFormat), valueFormat, null)
                    };

                    var columnEntry = lastEntry ? value : BuildColumnEntry(column, value);
                    outputString.Append(columnEntry); ;
                    raw = raw.Remove(0, shift);
                }

                outputString.Append(buildLine + Environment.NewLine);
            }

            return outputString.ToString();
        }

        string BuildColumnEntry(string column, string entry)
        {
            return column.Remove(0, entry.Length).Insert(0, entry);
        }

        private string BuildColumnString()
        {
            var columnString = string.Empty;
            const int columnSize = 12;

            for (var i = 0; i < columnSize; i++)
            {
                columnString += " ";
            }

            return columnString;
        }
    }
}