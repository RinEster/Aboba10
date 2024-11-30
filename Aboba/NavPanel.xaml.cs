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
using Aboba.Pages;
namespace Aboba
{
    /// <summary>
    /// Логика взаимодействия для NavPanel.xaml
    /// </summary>
    public partial class NavPanel : Window
    {
        public NavPanel()
        {
            InitializeComponent();
        }

        private void exit_Click(object sender, RoutedEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Вы действительно хотите завершить работу?", "Завершение работы",
            MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
            {
                Close();
            }
        }  

        private void profileButton_Click(object sender, RoutedEventArgs e)
        {
            place.Children.Clear();
            Profile profile = new Profile();
            place.Children.Add(profile);
        }

        private void datingButton_Click(object sender, RoutedEventArgs e)
        {
            place.Children.Clear();
            relationship relationship = new relationship();
            place.Children.Add(relationship);
        }

        private void somethingButton_Click(object sender, RoutedEventArgs e)
        {
            place.Children.Clear();
            Settings settings = new Settings();
            place.Children.Add(settings);
        }
    }
}
