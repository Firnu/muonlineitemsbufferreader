namespace MuOnlineItemsBufferReader.Converters
{
    public interface IBufferConverter
    {
        public string Convert(string input, int itemSizeUInt8, ValueFormat valueFormat);
    }
}