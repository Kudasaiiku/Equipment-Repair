using System.Windows;
using System.Data.SqlClient;

namespace Remontnik
{
    public partial class StatisticWindow : Window
    {
        public StatisticWindow()
        {
            InitializeComponent();
        }

        // Метод загрузки статистики по заявкам.
        private void LoadButton_Click(object sender, RoutedEventArgs e)
        {
            // Статистика по кол-ву выполненных заявок.
            using (SqlConnection connection = new SqlConnection("Data Source=localhost;Initial Catalog=Remontnik1;Integrated Security=True"))
            {
                connection.Open();

                string CountQuery = "SELECT COUNT(*) FROM Remont WHERE Статус = 'Выполнено'";
                using (SqlCommand cmd = new SqlCommand(CountQuery, connection))
                {
                    object result = cmd.ExecuteScalar();

                    if (result != null)
                    {
                        Count.Text = result.ToString();
                    }
                    else
                        Count.Text = "Результат не найден";
                }

                // Статистика по времени выполнения заявок.
                string DateQuery = "SELECT AVG(DATEDIFF(DAY, CONVERT(DATETIME, Дата_начала, 104), CONVERT(DATETIME, Дата_завершения, 104))) FROM Remont";
                using (SqlCommand cmd = new SqlCommand(DateQuery, connection))
                {
                    object result = cmd.ExecuteScalar();

                    if (result != null)
                    {
                        Time.Text = result.ToString() + " дней";
                    }
                    else
                        Time.Text = "Результат не найден";
                }

                // Статитстика по колвичеству типов неисправности.
                if (Type.Text == "Тип 1")
                {
                    string query1 = "SELECT COUNT(*) FROM Remont WHERE Тип = 'Тип 1'";

                    using (SqlCommand cmd = new SqlCommand(query1, connection))
                    {
                        object result = cmd.ExecuteScalar();

                        if (result != null)
                        {
                            TypeCount.Text = result.ToString();
                        }
                        else
                            TypeCount.Text = "Результат не найден";
                    }    
                }    
                else if (Type.Text == "Тип 2")
                {
                    string query2 = "SELECT COUNT(*) FROM Remont WHERE Тип = 'Тип 2'";

                    using (SqlCommand cmd = new SqlCommand(query2, connection))
                    {
                        object result = cmd.ExecuteScalar();

                        if (result != null)
                        {
                            TypeCount.Text = result.ToString();
                        }
                        else
                            TypeCount.Text = "Результат не найден";
                    }
                }   
                else
                {
                    string query3 = "SELECT COUNT(*) FROM Remont WHERE Тип NOT IN ('Тип 1', 'Тип 2')";

                    using (SqlCommand cmd = new SqlCommand(query3, connection))
                    {
                        object result = cmd.ExecuteScalar();

                        if (result != null)
                        {
                            TypeCount.Text = result.ToString();
                        }
                        else
                            TypeCount.Text = "Результат не найден";
                    }
                }
            }
        }
    }
}
