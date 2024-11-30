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
    /// Логика взаимодействия для relationship.xaml
    /// </summary>
    public partial class relationship : UserControl
    {
       // public static int userId;
        // ListBox listUser = new ListBox();
        public relationship()
        {
            InitializeComponent();
            GetData();
        }

        public int GetUserId(string login)
        {
            int userId;
            using (SqlConnection conn = new SqlConnection(MainWindow.ConnStrA))
            {
                using (SqlCommand cmd = new SqlCommand("userId", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@login", MainWindow.login);
                    SqlParameter outputIdParam = new SqlParameter("@id", SqlDbType.Int)
                    {
                        Direction = ParameterDirection.Output
                    };
                    cmd.Parameters.Add(outputIdParam);
                    conn.Open();
                    cmd.ExecuteNonQuery();
                    userId = (int)outputIdParam.Value;
                    MessageBox.Show(userId.ToString());
                }
            }
            return userId;

        }

        public ListBox GetPrev(string login)
        {
            ListBox listUser = new ListBox();
            using (SqlConnection conn = new SqlConnection(MainWindow.ConnStrA))
            {
                using (SqlCommand cmd = new SqlCommand("[dbo].[PrefUsers]", conn))
                {
                    cmd.CommandType = CommandType.StoredProcedure;
                    cmd.Parameters.AddWithValue("@login", MainWindow.login);

                    try
                    {
                        conn.Open();
                        using (SqlDataReader reader = cmd.ExecuteReader())
                        {
                            listUser.Items.Clear();

                            while (reader.Read())
                            {
                                string preference = reader["preferens"].ToString();
                                listUser.Items.Add(preference);
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Ошибка: {ex.Message}");
                    }
                }
            }
            return listUser;
        }

        public int countPeople()
        {
            int countP = 0;
            using (SqlConnection connection = new SqlConnection(MainWindow.ConnStrA))
            {
                using (SqlCommand command = new SqlCommand("countPeople", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;
                    SqlParameter countParameter = new SqlParameter("@count", SqlDbType.Int)
                    {
                        Direction = ParameterDirection.Output
                    };
                    command.Parameters.Add(countParameter);
                    connection.Open();
                    command.ExecuteNonQuery();
                    countP = (int)countParameter.Value;
                }
            }
       
            return countP;
        }

        public ListBox allLogins()
        {
            ListBox allLog = new ListBox();
            using (SqlConnection connection = new SqlConnection(MainWindow.ConnStrA))
            {
                using (SqlCommand command = new SqlCommand("allLogins", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    connection.Open();
                    using (SqlDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            string login = reader.GetString(0);
                            allLog.Items.Add(login);
                        }
                    }
                }
            }
            return allLog; 
        }

        

        public void GetData()
        {
            User user = new User();
            user.login = MainWindow.login;
            string[] itemsArray = GetPrev(MainWindow.login).Items.Cast<string>().ToArray();
            user.preferences = itemsArray;
            string[] allLogin = allLogins().Items.Cast<string>().ToArray();
            User[] users = new User[countPeople()];
            for (int i = 0; i < countPeople(); ++i)
            {
                User u = new User();
                u.login = allLogin[i];
                u.preferences = GetPrev(u.login).Items.Cast<string>().ToArray();
                users[i] = u;
            }

            string[] recommendedUsers = user.recommendUsers(user, users);
            for(int i = 0; i<recommendedUsers.Length; i++)
            {
                using (SqlConnection connection = new SqlConnection(MainWindow.ConnStrA))
                {
                    connection.Open();
                    using (SqlCommand command = new SqlCommand("LoginAndTg", connection))
                    {
                        command.CommandType = CommandType.StoredProcedure;

                        command.Parameters.AddWithValue("@login", recommendedUsers[i]);
                       
                        command.ExecuteScalar();

                        SqlDataAdapter adapter = new SqlDataAdapter(command);
                        DataTable dataTable = new DataTable();
                        adapter.Fill(dataTable);

                        favorit.ItemsSource = dataTable.DefaultView;

                    }
                }
                
            }

        

        }


        public void LoadRecommendedUsers(string[] recommendedUsers)
        {
            //алгоритм работает, но у нас проблемы с выводом
            string users = string.Join(",", recommendedUsers);

           // List<UserTg> userListTg = new List<UserTg>();

            using (SqlConnection connection = new SqlConnection(MainWindow.ConnStrA))
            {
                connection.Open();
                using (SqlCommand command = new SqlCommand("LoginAndTg", connection))
                {
                    command.CommandType = CommandType.StoredProcedure;

                    command.Parameters.AddWithValue("@loginList", users);

                    SqlDataReader reader = command.ExecuteReader();
                    while (reader.Read())
                    {
                        //UserTg usertg = new UserTg
                        //{
                        //    Login = reader["Login"].ToString(),
                        //    Tg = reader["Tg"].ToString()
                        //};
                        //userListTg.Add(usertg);
                        //MessageBox.Show(usertg.Tg);
                    }
                }
            }

        //   favorit.ItemsSource = userListTg;
        }
        //public class UserTg
        //{
        //    public string Login { get; set; }
        //    public string Tg { get; set; }
        //}

    }
}
