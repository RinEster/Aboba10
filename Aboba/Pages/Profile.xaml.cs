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
    /// Логика взаимодействия для Profile.xaml
    /// </summary>
    public partial class Profile : UserControl
    {
        public Profile()
        {
            InitializeComponent();
            DataUser();
            
        }

        public void LoadPreferences(string login)
        {
            using (SqlConnection conn = new SqlConnection(MainWindow.ConnStrA))
            {
                using (SqlCommand cmd = new SqlCommand("[dbo].[PrefUsers]", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@login", login);

                    try
                    {
                        conn.Open();
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            prefList.Items.Clear(); 

                            while (reader.Read())
                            {
                                string preference = reader["preferens"].ToString();
                                prefList.Items.Add(preference);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Ошибка: {ex.Message}");
                    }
                }
            }
        }

        public void DataUser()
        {
            DataTable dataUser = OutDataUser(MainWindow.login);
            if (dataUser.Rows.Count > 0)
            {
                loginUser.Text += MainWindow.login;
                string name = dataUser.Rows[0]["name"] != DBNull.Value ? dataUser.Rows[0]["name"].ToString() : "not found";
                nameUser.Text += name;
                string surname = dataUser.Rows[0]["surname"] != DBNull.Value ? dataUser.Rows[0]["surname"].ToString() : "not found";
                surnameUser.Text += surname;
                string age = dataUser.Rows[0]["age"] != DBNull.Value ? Convert.ToInt32(dataUser.Rows[0]["age"]).ToString() : "not found";
                ageUser.Text += age;
                string countryLoc = dataUser.Rows[0]["countryLoc"] != DBNull.Value ? dataUser.Rows[0]["countryLoc"].ToString() : "not found";
                countryUser.Text += countryLoc;
                string regionLoc = dataUser.Rows[0]["regionLoc"] != DBNull.Value ? dataUser.Rows[0]["regionLoc"].ToString() : "not found";
                regionUser.Text += regionLoc;
                string cityLoc = dataUser.Rows[0]["cityLoc"] != DBNull.Value ? dataUser.Rows[0]["cityLoc"].ToString() : "not found";
                cityUser.Text += cityLoc;
            }
            LoadPreferences(MainWindow.login);
        }

        public DataTable OutDataUser(string login)
        {
            using (SqlConnection conn = new SqlConnection(MainWindow.ConnStrA))
            {
                conn.Open();
                string proc = "[dbo].[OutUserData]";
                SqlCommand sqlCommand = new SqlCommand(proc, conn);
                sqlCommand.CommandType = CommandType.StoredProcedure;
                SqlParameter ParamLog = new SqlParameter
                {
                    ParameterName = "@login",
                    Value = login
                };
                sqlCommand.Parameters.Add(ParamLog);
                sqlCommand.ExecuteScalar();

                SqlDataAdapter adapter = new SqlDataAdapter(sqlCommand);
                DataTable dataTableUser = new DataTable();
                adapter.Fill(dataTableUser);
                return dataTableUser;
            }
        }
    }

}
