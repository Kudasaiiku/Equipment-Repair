using System.Windows;
using System.Data.SqlClient;
using System.Data;
using System.Collections.Generic;
using System;
using System.ComponentModel;
using System.Windows.Data;

namespace Remontnik
{
    public partial class ManagerWindow : Window
    {
        public ManagerWindow()
        {
            InitializeComponent();

            // Загрузка таблицы заявок пользователя.
            DataBaseConnection();
        }

        // Класс, необходимый для подключения значений столбцов таблицы.
        public class Remont
        {
            public int Номер { get; set; }
            public string Статус { get; set; }
            public string Дата_начала { get; set; }
            public string Дата_завершения { get; set; }
            public string Имя { get; set; }
            public string Оборудование { get; set; }
            public string Тип { get; set; }
            public string Описание { get; set; }
            public string Ответственный { get; set; }
            public string Комментарий { get; set; }
        }

        // Подключение таблицы.
        public void DataBaseConnection()
        {
            using (SqlConnection connection = new SqlConnection("Data Source=localhost;Initial Catalog=Remontnik1;Integrated Security=True"))
            {
                connection.Open();

                SqlDataAdapter adapter = new SqlDataAdapter();
                adapter.SelectCommand = new SqlCommand("SELECT * FROM Remont", connection);

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
                        Дата_завершения = row["Дата_завершения"].ToString(),
                        Имя = row["Имя"].ToString(),
                        Оборудование = row["Оборудование"].ToString(),
                        Тип = row["Тип"].ToString(),
                        Описание = row["Описание"].ToString(),
                        Ответственный = row["Ответственный"].ToString(),
                        Комментарий = row["Комментарий"].ToString(),
                    };

                    remontList.Add(remont);
                }

                RemontGrid.ItemsSource = remontList;
            }
        }

        // Сортировка таблицы по не выполненным заявкам.
        private void SortButton_Click(object sender, RoutedEventArgs e)
        {
            using (SqlConnection connection = new SqlConnection("Data Source=localhost;Initial Catalog=Remontnik1;Integrated Security=True"))
            {
                connection.Open();

                SqlDataAdapter adapter = new SqlDataAdapter();
                adapter.SelectCommand = new SqlCommand("SELECT * FROM Remont WHERE Remont.Статус = 'Не выполнено'", connection);

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
                        Дата_завершения = row["Дата_завершения"].ToString(),
                        Имя = row["Имя"].ToString(),
                        Оборудование = row["Оборудование"].ToString(),
                        Тип = row["Тип"].ToString(),
                        Описание = row["Описание"].ToString(),
                        Ответственный = row["Ответственный"].ToString(),
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

        // Переход на окно редактирования заявки.
        private void ChangeButton_Click(object sender, RoutedEventArgs e)
        {
            Editing editing = new Editing();
            editing.ShowDialog();

            DataBaseConnection();
        }

        // Поиск по номеру заявки.
        private void SearchBox_PreviewTextInput(object sender, System.Windows.Input.TextCompositionEventArgs e)
        {
            string searchText = SearchBox.Text + e.Text;

            ICollectionView dataView = CollectionViewSource.GetDefaultView(RemontGrid.ItemsSource);

            dataView.Filter = item =>
            {
                if (item is Remont remont)
                {
                    string numberAsString = remont.Номер.ToString();
                    return numberAsString.IndexOf(searchText, StringComparison.OrdinalIgnoreCase) >= 0;
                }

                return false;
            };
        }

        // Переход на окно статистики по заявкам.
        private void StatisticButton_Click(object sender, RoutedEventArgs e)
        {
            StatisticWindow statisticWindow = new StatisticWindow();
            statisticWindow.ShowDialog();
        }

        // Выход на окно авторицации.
        private void LogoutButton_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
            this.Close();
        }
    }
}
