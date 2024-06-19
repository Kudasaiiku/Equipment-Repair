using System.Windows;
using System.Data.SqlClient;
using System.Data;
using System.Collections.Generic;
using System;
using System.Windows.Controls;

namespace Remontnik
{
    public partial class UserWindow : Window
    {
        public UserWindow()
        {
            InitializeComponent();

            // Загрузка таблицы заявок пользователя.
            DataBaseConnection();
        }

        // Отправка заявки пользователем.
        private void SendButton_Click(object sender, RoutedEventArgs e)
        {
            if (IsAllTextBoxesFilled())
            {
                string login = LoginClass.loginValue;

                string type;
                string item;
                string problem;

                DateTime currentDate = DateTime.Now;
                string formattedDate = currentDate.ToString("dd.MM.yyyy");
                string date = formattedDate;

                type = Convert.ToString(Type.Text);
                item = Convert.ToString(Item.Text);
                problem = Convert.ToString(Problem.Text);

                using (SqlConnection connection = new SqlConnection("Data Source=localhost;Initial Catalog=Remontnik1;Integrated Security=True"))
                {
                    connection.Open();

                    string query = "SELECT Имя FROM Remont WHERE Логин = @RemontLogin";
                    using (SqlCommand cmd = new SqlCommand(query, connection))
                    {
                        cmd.Parameters.AddWithValue("@RemontLogin", login);

                        string name = cmd.ExecuteScalar() as string;

                        using (SqlCommand command = new SqlCommand("INSERT INTO Remont (Дата_начала, Имя, Оборудование, Тип, Описание, Логин) VALUES ('" + date + "', '" + name + "', '" + item + "', '" + type + "', '" + problem + "', '" + login + "')", connection))
                        {
                            command.ExecuteNonQuery();
                        }
                    }
                }
                MessageBox.Show("Заявка успешно добавлена!", "Успешно!", MessageBoxButton.OK, MessageBoxImage.Information);
                DataBaseConnection();
            }
            else
            {
                MessageBox.Show("Заполните все поля!", "Ошибка!", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        // Класс, необходимый для подключения значений столбцов таблицы.
        public class Remont
        {
            public int Номер { get; set; }
            public string Статус { get; set; }
            public string Дата_начала { get; set; }
            public string Комментарий { get; set; }
        }

        // Подключение таблицы.
        public void DataBaseConnection()
        {
            using (SqlConnection connection = new SqlConnection("Data Source=localhost;Initial Catalog=Remontnik1;Integrated Security=True"))
            {
                connection.Open();

                SqlDataAdapter adapter = new SqlDataAdapter();
                adapter.SelectCommand = new SqlCommand("SELECT * FROM Remont WHERE Логин = '" + LoginClass.loginValue + "'", connection);

                DataTable dataTable = new DataTable();
                adapter.Fill(dataTable);

                List<Remont> remontList = new List<Remont>();

                foreach (DataRow row in dataTable.Rows)
                {
                    Remont remont = new Remont
                    {
                        Номер = Convert.ToInt32(row["Номер"]),
                        Статус = row["Статус"].ToString(),
                        Дата_начала = row["Дата_начала"].ToString(),
                        Комментарий = row["Комментарий"].ToString(),
                    };

                    remontList.Add(remont);
                }

                RemontGrid.ItemsSource = remontList;
            }
        }

        // Обновление таблицы.
        private void RefreshButton_Click(object sender, RoutedEventArgs e)
        {
            DataBaseConnection();
        }

        // Выход пользователя на окно авторизации.
        private void LogoutButton_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
            this.Close();
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
