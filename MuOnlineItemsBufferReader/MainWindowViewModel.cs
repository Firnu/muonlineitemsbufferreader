using GalaSoft.MvvmLight;
using GalaSoft.MvvmLight.Command;
using MuOnlineItemsBufferReader.Converters;

namespace MuOnlineItemsBufferReader
{
    public class MainWindowViewModel : ViewModelBase
    {
        private string input = string.Empty;
        private string output = string.Empty;

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

        public string Output
        {
            get => output;
            set
            {
                output = value;
                RaisePropertyChanged(() => Output);
            }
        }


        public MainWindowViewModel()
        {
            ConvertCommand = new RelayCommand(ConvertAction);
        }

        private void ConvertAction()
        {
            var converter = new InventoryConverter();
            Output = converter.Convert(Input, 25);
        }
    }
}