using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using MuOnlineItemsBufferReader.Converters;

namespace MuOnlineItemsBufferReader
{
    public class MainWindowViewModel : ViewModelBase
    {
        private string input = string.Empty;
        private string outputHex = string.Empty;
        private string outputDec = string.Empty;
        private string outputBits = string.Empty;

        public RelayCommand ConvertCommand { get; }

        public string Input
        {
            get => input;
            set
            {
                input = value;
                RaisePropertyChanged(() => Input);
            }
        }

        public string OutputHex
        {
            get => outputHex;
            set
            {
                outputHex = value;
                RaisePropertyChanged(() => OutputHex);
            }
        }


        public string OutputDec
        {
            get => outputDec;
            set
            {
                outputDec = value;
                RaisePropertyChanged(() => OutputDec);
            }
        }

        public string OutputBits
        {
            get => outputBits;
            set
            {
                outputBits = value;
                RaisePropertyChanged(() => OutputBits);
            }
        }

        public MainWindowViewModel()
        {
            ConvertCommand = new RelayCommand(ConvertAction);
        }

        private void ConvertAction()
        {
            var converter = new InventoryConverter();

            const int itemUint8Size = 25;

            OutputHex = converter.Convert(Input, itemUint8Size, ValueFormat.Hex);
            OutputDec = converter.Convert(Input, itemUint8Size, ValueFormat.Dec);
            OutputBits = converter.Convert(Input, itemUint8Size, ValueFormat.Bit);
        }
    }
}