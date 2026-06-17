using System.Windows;
using ViewModel; // Было View.ViewModel

namespace View
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            DataContext = new MainVM();
        }
    }
}