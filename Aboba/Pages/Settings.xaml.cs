using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace Aboba.Pages
{
    /// <summary>
    /// Логика взаимодействия для Settings.xaml
    /// </summary>
    public partial class Settings : UserControl
    {
        public Settings()
        {
            InitializeComponent();
        }

        private void update_Click(object sender, RoutedEventArgs e)
        {
            string login = MainWindow.login; 
            string name = newname.Text;
            string surname = newsurname.Text;
            int age = int.Parse(newage.Text);
            string country = newcountry.Text;
            string region = newregion.Text;
            string city = newcity.Text;

            UpdateUserData(login, name, surname, age, country, region, city);
        }

        private void UpdateUserData(string login, string name, string surname, int age, string country, string region, string city)
        {
            using (SqlConnection conn = new SqlConnection(MainWindow.ConnStrA))
            {
                using (SqlCommand cmd = new SqlCommand("[dbo].[UpdateDataUser]", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    // Добавляем параметры
                    cmd.Parameters.AddWithValue("@login", login);
                    cmd.Parameters.AddWithValue("@name", name);
                    cmd.Parameters.AddWithValue("@surname", surname);
                    cmd.Parameters.AddWithValue("@age", age);
                    cmd.Parameters.AddWithValue("@countryUser", country);
                    cmd.Parameters.AddWithValue("@regionUser", region);
                    cmd.Parameters.AddWithValue("@cityUser", city);

                    conn.Open();
                    int rowsAffected = cmd.ExecuteNonQuery();
                    conn.Close();
                                       
                }
            }
        }
    }
}