using PierwszePodejscieDoQuizu.View;
using PierwszePodejscieDoQuizu.ViewModel;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace PierwszePodejscieDoQuizu
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
        private void NavigateToDeleteWindows_Click(object sender, RoutedEventArgs e)
        {
            // Tutaj umieść kod nawigacji do DeleteWindows
            PierwszePodejscieDoQuizu.View.DeleteWindows deleteWindows = new PierwszePodejscieDoQuizu.View.DeleteWindows();
            deleteWindows.Show(); // Pokaż nowe okno DeleteWindows
            this.Close(); // Zamknij obecne okno MainWindow
        }

        private void NavigateToEditWindow_Click(object sender, RoutedEventArgs e)
        {
            PierwszePodejscieDoQuizu.View.EditWindow editWindow = new PierwszePodejscieDoQuizu.View.EditWindow();
            editWindow.Show();
            this.Close();
        }
    }
}