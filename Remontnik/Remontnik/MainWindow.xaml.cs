using System.Windows;
using System.Windows.Input;
using System.Data.SqlClient;
using System.Windows.Controls;
using System;

namespace Remontnik
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        // Авторизация пользователя.
        private void AuthButton_Click(object sender, RoutedEventArgs e)
        {
            if (IsAllTextBoxesFilled())
            {
                LoginClass.loginValue = Convert.ToString(LoginBox.Text);

                string Login = Convert.ToString(LoginBox.Text);
                string Password = Convert.ToString(PasswordBox.Text);

                using (SqlConnection connection = new SqlConnection("Data Source=localhost;Initial Catalog=Remontnik1;Integrated Security=True"))
                {
                    connection.Open();

                    string query = "SELECT Пароль FROM Remont WHERE Логин = @RemontLogin";

                    using (SqlCommand cmd = new SqlCommand(query, connection))
                    {
                        cmd.Parameters.AddWithValue("@RemontLogin", Login);

                        string storedPassword = cmd.ExecuteScalar() as string;

                        if (storedPassword != null)
                        {
                            if (Password == storedPassword)
                            {
                                if (Login == "admin" && Password == "admin")
                                {
                                    // Вход менеджера.
                                    MessageBox.Show("Вы успешно вошли!", "Успешно!", MessageBoxButton.OK, MessageBoxImage.Information);
                                    ManagerWindow managerWindow = new ManagerWindow();
                                    managerWindow.Show();
                                    this.Close();
                                }
                                else
                                {
                                    // Вход пользователя.
                                    MessageBox.Show("Вы успешно вошли!", "Успешно!", MessageBoxButton.OK, MessageBoxImage.Information);
                                    UserWindow userWindow = new UserWindow();
                                    userWindow.Show();
                                    this.Close();
                                }    
                            }
                            else
                            {
                                MessageBox.Show("Неверный пароль!", "Ошибка!", MessageBoxButton.OK, MessageBoxImage.Error);
                            }
                        }
                        else
                        {
                            MessageBox.Show("Такого аккаунта не существует!", "Ошибка!", MessageBoxButton.OK, MessageBoxImage.Warning);
                        }
                    }
                }
            }
            else
            {
                MessageBox.Show("Заполните все поля!", "Ошибка!", MessageBoxButton.OK, MessageBoxImage.Error);
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
