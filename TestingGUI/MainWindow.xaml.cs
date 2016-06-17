using System.Windows;
using System.Windows.Controls;
using TestingGUI.Tools;

namespace TestingGUI
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private MainDataContext _dataContext = new MainDataContext();

        public MainWindow()
        {
            InitializeComponent();
            DataContext = _dataContext;
        }

        private void ListView_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            ListView l = sender as ListView;
            MainFrame.Navigate(l.SelectedItem);
        }
    }
}
