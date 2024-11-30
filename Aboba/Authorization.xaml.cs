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
using System.Windows.Shapes;

namespace Aboba
{
    /// <summary>
    /// Логика взаимодействия для Authorization.xaml
    /// </summary>
    public partial class Authorization : Window
    {
        public Authorization()
        {
            InitializeComponent();
        }

        private void registrationButton_Click(object sender, RoutedEventArgs e)
        {
           if(loginUser.Text != "" && passUser.Password != "")
           {
                AuthorizeUser(loginUser.Text, passUser.Password);
           }
            else
            {
                MessageBox.Show("Заполните поля");
            }
            
        }

        private void AuthorizeUser(string login, string password)
        {
            
            using (SqlConnection conn = new SqlConnection(MainWindow.ConnStrA))
            {
                using (SqlCommand cmd = new SqlCommand("[dbo].[AuthorizationUsers]", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;

                    cmd.Parameters.AddWithValue("@login", login);
                    cmd.Parameters.AddWithValue("@pass", password);

                    SqlParameter resultParam = new SqlParameter("@result", SqlDbType.NVarChar, 50)
                    {
                        Direction = ParameterDirection.Output
                    };
                    cmd.Parameters.Add(resultParam);
                    conn.Open();
                    cmd.ExecuteNonQuery();
                    
                    string result = resultParam.Value.ToString();
                    MessageBox.Show(result);
                    if (result == "good")
                    {
                       
                        MainWindow.login = login;
                        NavPanel navPanel = new NavPanel();
                        navPanel.Show();
                        this.Close();
                    }
                }


            }
        }
    }
}
