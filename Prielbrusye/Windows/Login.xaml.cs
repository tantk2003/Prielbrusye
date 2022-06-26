using System;
using System.Collections.Generic;
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
using System.Windows.Threading;

namespace Prielbrusye
{
    /// <summary>
    /// Логика взаимодействия для Login.xaml
    /// </summary>
    public partial class Login : Window
    {
        public int counter = 0;
        private int inc = 0;
        private DispatcherTimer timer_login;
        public static user currentUser = null;
        public Login()
        {
            InitializeComponent();
        }
        public Login(int ident)
        {
            InitializeComponent();
            /// <summary>
            /// Блокировка авторизации на 10 секунд после ввода капчи
            /// </summary>
            if (ident == 1)
            {
                inc = 0;
                tbox_login.IsEnabled = false;
                tbox_pass_mask.IsEnabled = false;
                tbox_pass_open.IsEnabled = false;
                btn_login.IsEnabled = false;  
                timer_login = new DispatcherTimer();
                timer_login.Interval = TimeSpan.FromSeconds(1);
                timer_login.Tick += timerTicker1;
                timer_login.Start();               
            }
            /// <summary>
            /// Блокировка авторизации на 3 минуты после завершения сеанса
            /// </summary>
            else if(ident == 2)
            {
                inc = 0;
                tbox_login.IsEnabled = false;
                tbox_pass_mask.IsEnabled = false;
                tbox_pass_open.IsEnabled = false;
                btn_login.IsEnabled = false;
                timer_login = new DispatcherTimer();
                timer_login.Interval = TimeSpan.FromSeconds(1);
                timer_login.Tick += timerTicker2;
                timer_login.Start();
            }
        }

        /// <summary>
        /// Таймер на 10 секунд
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void timerTicker1(object sender, EventArgs e)
        {
            inc++;
            if (inc == 10)
            {
                timer_login.Stop();
                tbox_login.IsEnabled = true;
                tbox_pass_mask.IsEnabled = true;
                tbox_pass_open.IsEnabled = true;
                btn_login.IsEnabled = true;
            }
        }
        /// <summary>
        /// Таймер на 3 минуты
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void timerTicker2(object sender, EventArgs e)
        {
            inc++;
            if(inc == 180)
            {
                timer_login.Stop();
                tbox_login.IsEnabled = true;
                tbox_pass_mask.IsEnabled = true;
                tbox_pass_open.IsEnabled = true;
                btn_login.IsEnabled = true;
            }
        }

        /// <summary>
        /// Авторизация (проверка на совпадение введенного логина и пароля с данными в БД)
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_login_Click(object sender, RoutedEventArgs e)
        {
            StringBuilder errors = new StringBuilder();
            if (string.IsNullOrWhiteSpace(tbox_login.Text))
                errors.AppendLine("Вы не ввели логин");
            if (string.IsNullOrWhiteSpace(tbox_pass_mask.Password) && string.IsNullOrWhiteSpace(tbox_pass_open.Text))
                errors.AppendLine("Вы не ввели пароль");
            if(errors.Length > 0)
            {
                MessageBox.Show(errors.ToString(), "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            if (counter == 1)
            {
                tbox_pass_mask.Visibility = Visibility.Visible;
                tbox_pass_mask.Password = tbox_pass_open.Text;
                tbox_pass_open.Visibility = Visibility.Hidden;
            }

            string login = tbox_login.Text;
            string pass = tbox_pass_mask.Password;
            var user = skiresortEntities.GetContext().users.Where(x => x.login == login && x.password == pass).FirstOrDefault();
            var userlogin = skiresortEntities.GetContext().users.Where(x => x.login == login).FirstOrDefault();

            if (user == null)
            {
                if(userlogin != null)
                {
                    login_history login_history = new login_history();
                    login_history.id_user = userlogin.id;
                    string month = DateTime.Now.Month.ToString();
                    if (month.Length == 1)
                        month = "0" + month;
                    string day = DateTime.Now.Day.ToString();
                    if(day.Length == 1)
                        day = "0" + day;
                    string date_login = DateTime.Now.Year.ToString() + "-" + month + "-" + day;
                    login_history.date = date_login;
                    login_history.time = DateTime.Now.ToShortTimeString();
                    login_history.status = "Неуспешно";
                    skiresortEntities.GetContext().login_history.Add(login_history);
                    try
                    {
                        skiresortEntities.GetContext().SaveChanges();
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show(ex.Message.ToString(), "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }
                MessageBox.Show("Пользователь не найден", "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                Hide();
                new Captcha().ShowDialog();
            }
            else
            {               
                login_history login_history = new login_history();
                login_history.id_user = userlogin.id;
                string month = DateTime.Now.Month.ToString();
                if (month.Length == 1)
                    month = "0" + month;
                string day = DateTime.Now.Day.ToString();
                if (day.Length == 1)
                    day = "0" + day;
                string date_login = DateTime.Now.Year.ToString() + "-" + month + "-" + day;
                login_history.date = date_login;
                login_history.time = DateTime.Now.ToShortTimeString();
                login_history.status = "Успешно";
                skiresortEntities.GetContext().login_history.Add(login_history);
                try
                {
                    skiresortEntities.GetContext().SaveChanges();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message.ToString(), "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                currentUser = user;
                Hide();
                new UserWin(currentUser).ShowDialog();
            }
        }

        /// <summary>
        /// Скрытие/отображение пароля
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_show_pass_Click(object sender, RoutedEventArgs e)
        {
            counter++;
            if(counter == 1)
            {
                tbox_pass_open.Visibility = Visibility.Visible;
                tbox_pass_open.Text = tbox_pass_mask.Password;
                tbox_pass_mask.Visibility = Visibility.Hidden;
            }
            else
            {
                tbox_pass_mask.Visibility = Visibility.Visible;
                tbox_pass_mask.Password = tbox_pass_open.Text;
                tbox_pass_open.Visibility = Visibility.Hidden;
                counter = 0;
            }
        }
    }
}
