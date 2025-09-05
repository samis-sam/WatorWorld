using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace WatorWorld
{
    /// <summary>
    /// Interaction logic for StartupWindow.xaml
    /// </summary>
    public partial class StartupWindow : Window
    {
        public StartupWindow()
        {
            InitializeComponent();
        }

        private void NumberValidationTextBox(object sender, TextCompositionEventArgs e)
        {
            e.Handled = !e.Text.All(c => char.IsDigit(c));
        }

        private void StartButton_Click(object sender, RoutedEventArgs e)
        {
            if (GridWidthTextBox.Text.Length > 0 && GridHeightTextBox.Text.Length > 0)
            {
                int width = Int32.Parse(GridWidthTextBox.Text);
                int height = Int32.Parse(GridHeightTextBox.Text);
                MainWindow mainWindow = new(width, height);
                mainWindow.Show();
                Close();
            }
        }
    }
}
