using System.Windows;
using MuOnlineItemsBufferReader.Converters;

namespace MuOnlineItemsBufferReader
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            DataContext = new MainWindowViewModel();
        }
    }
}