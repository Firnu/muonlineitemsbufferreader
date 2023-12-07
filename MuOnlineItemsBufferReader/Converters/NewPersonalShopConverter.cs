using System;
using System.Linq;
using System.Text;

namespace MuOnlineItemsBufferReader.Converters
{
    public class NewPersonalShopConverter : IBufferConverter
    {
        public string Convert(string input, int itemSizeUInt8, bool convertValueToDec = false)
        {
            if (input.StartsWith("0x"))
            {
                input = input.Remove(0, 2);
            }

            var outputString = new StringBuilder();

            var preItemsLines = new (int count, string note)[]
            {
                (2, " <- slot index"),
                (6, " <- start of items"),
            };

            var itemSize = itemSizeUInt8 * 2;

            var postItemsLines = new (int count, string note)[]
            {
                (6, " <- end of items"),
                (8, " <- zen (hex pairs in reverse)"),
                (8, " <- bless (hex pairs in reverse)"),
                (8, " <- soul (hex pairs in reverse)"),
            };

            var totalEntrySize = preItemsLines.Sum(q => q.count) + postItemsLines.Sum(q => q.count) + (itemSize * 5);

            for (var i = 0; i < input.Length; i += totalEntrySize)
            {
                var totalShift = i;

                foreach (var count in preItemsLines)
                {
                    outputString.Append(input.Substring(totalShift, count.count) + count.note + Environment.NewLine);
                    totalShift += count.count;
                }

                for (var j = 0; j < 5; j++)
                {
                    outputString.Append(
                        $"{input.Substring(totalShift, itemSize)} <- item{Environment.NewLine}");
                    totalShift += itemSize;
                }

                foreach (var count in postItemsLines)
                {
                    if (totalShift + count.count > input.Length)
                    {
                        outputString.Append(input.Substring(totalShift, input.Length - totalShift));
                        break;
                    }

                    outputString.Append(input.Substring(totalShift, count.count) + count.note + Environment.NewLine);
                    totalShift += count.count;
                }

                outputString.Append(Environment.NewLine);
            }

            return outputString.ToString();
        }
    }
}