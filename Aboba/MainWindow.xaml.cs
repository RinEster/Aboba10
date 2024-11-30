using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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

namespace Aboba
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public static string login;
        public static string ConnStrA = "Data Source = DESKTOP-7807FQL\\SQLEXPRESS;Initial Catalog = \"aboba\"; Integrated Security = True";
        public MainWindow()
        {
            InitializeComponent();
        }      
               

        private void registrationButton_Click(object sender, RoutedEventArgs e)
        {
            //NavPanel navPanel = new NavPanel();
            //navPanel.Show();
            login = loginUser.Text.Trim();
            string password = passUser.Password.Trim();
            if (login != "" && password != "")
            {
                if (Correctness(login, password))
                {
                    RegistrationUser(login, password);
                }
                else
                {
                    MessageBox.Show("Некорректный ввод данных");
                }
            }
            else
            {
                MessageBox.Show("Заполните поля");
            }
        }

        bool Correctness(string login, string pass)
        {
            string patternPass = @"^.{5,}$"; 
            string patternLog = @"^.{5,}$"; 
            bool check = true;
            if (!Regex.IsMatch(login, patternLog))
            {
                check = false;
                loginUser.ToolTip = "Логин должен быть не менее 5.";
                loginUser.Background = Brushes.Red;
            }
            else
            {
                loginUser.Background = Brushes.White;
            }
            if (!Regex.IsMatch(pass, patternPass))
            {
                check = false;
                passUser.ToolTip = "Минимальная длина - 5 символов.";
                passUser.Background = Brushes.Coral;
            }
            else
            {
                passUser.Background = Brushes.White;
            }
            return check;
        }

        void RegistrationUser(string login, string pass)
        {
            string proc = "[dbo].[Registration]";

            using (SqlConnection connection = new SqlConnection(ConnStrA))
            {
                connection.Open();
                SqlCommand command = new SqlCommand(proc, connection);
                command.CommandType = CommandType.StoredProcedure;

                SqlParameter Paramlog = new SqlParameter
                {
                    ParameterName = "@login",
                    Value = login
                };
                command.Parameters.Add(Paramlog);

                SqlParameter Parampass = new SqlParameter
                {
                    ParameterName = "@pass",
                    Value = pass
                };
                command.Parameters.Add(Parampass);


                SqlParameter resultParameter = new SqlParameter("@result", SqlDbType.NVarChar, 50);
                resultParameter.Direction = ParameterDirection.Output;
                command.Parameters.Add(resultParameter);
                command.ExecuteNonQuery();

                string result = resultParameter.Value.ToString();
                MessageBox.Show(result);
                if(result == "new note added")
                {
                    TestWindow testWindow = new TestWindow();
                    testWindow.Show();
                    this.Close();
                }
            }
        }

        private void autoriz_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            Authorization authorization = new Authorization();
            authorization.Show();
            
        }

        private void exit_MouseLeftButtonDown(object sender, MouseButtonEventArgs e)
        {
            MessageBoxResult result = MessageBox.Show("Вы действительно хотите завершить работу?", "Завершение работы",
            MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
            {
                Close();
            }
        }
    }
}
