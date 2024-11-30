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
using Aboba.MyUserControls;
namespace Aboba
{
    /// <summary>
    /// Логика взаимодействия для TestWindow.xaml
    /// </summary>
    public partial class TestWindow : Window
    {
        DataTable dataTableCateg = new DataTable();
        public void TestData()
        {
            using (SqlConnection conn = new SqlConnection(MainWindow.ConnStrA))
            {
                conn.Open();
                string proc = "[dbo].[ListCategory]";
                SqlCommand sqlCommand = new SqlCommand(proc, conn);
                sqlCommand.CommandType = CommandType.StoredProcedure;

                sqlCommand.ExecuteScalar();

                SqlDataAdapter adapter = new SqlDataAdapter(sqlCommand);
               
                adapter.Fill(dataTableCateg);

            }
        }
        public DataTable AnswerCategory(string category)
        {
            using (SqlConnection conn = new SqlConnection(MainWindow.ConnStrA))
            {
                conn.Open();
                string proc = "[dbo].[CategoryAnswer]";
                SqlCommand sqlCommand = new SqlCommand(proc, conn);
                sqlCommand.CommandType = CommandType.StoredProcedure;
                SqlParameter ParamCat = new SqlParameter
                {
                    ParameterName = "@category",
                    Value = category
                };
                sqlCommand.Parameters.Add(ParamCat);
                sqlCommand.ExecuteScalar();

                SqlDataAdapter adapter = new SqlDataAdapter(sqlCommand);
                DataTable dataTableAnsw = new DataTable();
                adapter.Fill(dataTableAnsw);
                return dataTableAnsw;
            }
        }
        public TestWindow()
        {
            InitializeComponent();
            TestData();
            Load();


        }
        public int startPoint = 0;
       
        public void Load()
        {
            hhhh.Children.Clear();
            if (startPoint < dataTableCateg.Rows.Count)
            {
                DataRow row = dataTableCateg.Rows[startPoint];
                string category1 = row.Field<string>("category");
                //MessageBox.Show(category1);
                category.Text = category1;
                int count = 0;
                DataTable dt = AnswerCategory(category1);
                for(int i = 0; i < dt.Rows.Count; i++)
                {
                    string text = dt.Rows[i][0].ToString();
                    AnswerUC answer = new AnswerUC();
                    CheckBox checkBox = new CheckBox();
                    checkBox.FontSize = 13;
                    checkBox.Foreground = Brushes.White;

                    checkBox.Content = text;

                    answer.cont.Children.Add(checkBox);
                    hhhh.Children.Add(answer);
                }
                //if(count < dt.Rows.Count)
                //{
                //    AnswerUC answer = new AnswerUC();
                //    CheckBox checkBox = new CheckBox();
                //    string answerOption = row.Field<string>("answerOption");
                //    checkBox.Content = answerOption;

                //    answer.cont.Children.Add(checkBox);
                //    hhhh.Children.Add(answer);
                //    count++;
                //}

            }
            else
            {
                NavPanel navPanel = new NavPanel();
                navPanel.Show();
                this.Close();
            }
                

            
        }


        private List<string> GetCheckedAnswers()
        {
            List<string> checkedAnswers = new List<string>();

            // Проходим по всем дочерним элементам StackPanel
            foreach (var child in hhhh.Children)
            {
                if (child is AnswerUC answerUC)
                {
                    // Проходим по всем дочерним элементам AnswerUC
                    foreach (var control in answerUC.cont.Children)
                    {
                        if (control is CheckBox checkBox && checkBox.IsChecked == true)
                        {
                            // Если CheckBox выбран, добавляем его содержимое в список
                            checkedAnswers.Add(checkBox.Content.ToString());
                        }
                    }
                }
            }
            return checkedAnswers;
        }

        private void next_Click(object sender, RoutedEventArgs e)
        {
            List<string> selectedAnswers = GetCheckedAnswers();

           
            foreach (var i in selectedAnswers)
            {
                using (SqlConnection conn = new SqlConnection(MainWindow.ConnStrA))
                {
                    conn.Open();

                    using (SqlCommand cmd = new SqlCommand("savePreferences", conn))
                    {
                        cmd.CommandType = CommandType.StoredProcedure;

                        // Добавляем параметры
                        cmd.Parameters.Add(new SqlParameter("@login", SqlDbType.NVarChar, 30)).Value = MainWindow.login;
                        cmd.Parameters.Add(new SqlParameter("@pref", SqlDbType.NVarChar, 50)).Value = i;

                        // Выполняем команду
                        cmd.ExecuteNonQuery();
                    }
                }
            }
            AnswerUC answer = new AnswerUC();
            startPoint++; //answer.cont.Children.Clear();
            Load();
            //  category.Text = dataTable.Rows[startData]["category"].ToString();

            //  TestUserControl test = new TestUserControl();


        }
    }
}
