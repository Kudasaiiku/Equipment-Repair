using System.Windows;
using System.Data.SqlClient;
using System;
using System.Windows.Controls;

namespace Remontnik
{
    public partial class Editing : Window
    {
        public Editing()
        {
            InitializeComponent();
        }

        // Редактирование заявки.
        private void ChangeButton_Click(object sender, RoutedEventArgs e)
        {
            if (IsAllTextBoxesFilled())
            {
                string Number = NumberBox.Text;

                string _Status;
                string _Problem;
                string _Worker;
                string _Comment;

                DateTime currentDate = DateTime.Now;
                string formattedDate = currentDate.ToString("dd.MM.yyyy");
                string date = formattedDate;

                _Status = Convert.ToString(Status.Text);
                _Problem = Convert.ToString(Problem.Text);
                _Worker = Convert.ToString(Worker.Text);
                _Comment = Convert.ToString(Comment.Text);

                using (SqlConnection connection = new SqlConnection("Data Source=localhost;Initial Catalog=Remontnik1;Integrated Security=True"))
                {
                    connection.Open();

                    if (_Status == "Выполнено")
                    {
                        using (SqlCommand command = new SqlCommand("UPDATE Remont SET Статус = '" + _Status + "', " + "Описание = '" + _Problem + "', Ответственный = '" + _Worker + "', Комментарий = '" + _Comment + "', Дата_завершения = '" + date + "' WHERE Номер = '" + Number + "'", connection))
                        {
                            command.ExecuteNonQuery();
                        }
                    }
                    else
                    {
                        using (SqlCommand command = new SqlCommand("UPDATE Remont SET Статус = '" + _Status + "', " + "Описание = '" + _Problem + "', Ответственный = '" + _Worker + "', Комментарий = '" + _Comment + "', Дата_завершения = '' WHERE Номер = '" + Number + "'", connection))
                        {
                            command.ExecuteNonQuery();
                        }
                    }
                }

                this.Close();
            }
            else
            {
                MessageBox.Show("Заполните все поля!", "Ошибка!", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        // Загрузка значений редактируемой заявки.
        private void DownloadButton_Click(object sender, RoutedEventArgs e)
        {
            string Number = NumberBox.Text;

            using (SqlConnection connection = new SqlConnection("Data Source=localhost;Initial Catalog=Remontnik1;Integrated Security=True"))
            {
                connection.Open();

                string query = "SELECT * FROM Remont WHERE Номер = @Num";
                SqlCommand command = new SqlCommand(query, connection);
                command.Parameters.AddWithValue("@Num", Number);

                using (SqlDataReader reader = command.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        Status.Text = reader["Статус"].ToString();
                        Problem.Text = reader["Описание"].ToString();
                        Worker.Text = reader["Ответственный"].ToString();
                        Comment.Text = reader["Комментарий"].ToString();
                    }
                }
            }
        }

        // Проверка на заполненность всех TextBox.
        private bool IsAllTextBoxesFilled()
        {
            foreach (var element in grid.Children)
            {
                if (element is TextBox textBox && string.IsNullOrEmpty(textBox.Text))
                {
                    return false;
                }
            }
            return true;
        }
    }
}
