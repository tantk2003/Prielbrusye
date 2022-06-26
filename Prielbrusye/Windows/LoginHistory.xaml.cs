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

namespace Prielbrusye.Windows
{
    /// <summary>
    /// Логика взаимодействия для LoginHistory.xaml
    /// </summary>
    public partial class LoginHistory : Window
    {
        public int i = 0, l = 0;
        public LoginHistory()
        {
            InitializeComponent();
            TimerMove();
            var allLogin = skiresortEntities.GetContext().users.ToList();
            allLogin.Insert(0, new user
            {
                login = "Все логины"
            });
            cbox_filter_login.ItemsSource = allLogin;
            dgrid_login_history.ItemsSource = skiresortEntities.GetContext().login_history.ToList();
        }

        /// <summary>
        /// Отображение таймера
        /// </summary>
        public async void TimerMove()
        {
            while (Models.Timer.allTime.TotalSeconds > 0)
            {
                await Task.Delay(1000);
                timer_display.Text = Models.Timer.allTime.Hours.ToString() + ":" + Models.Timer.allTime.Minutes.ToString() + ":" + Models.Timer.allTime.Seconds.ToString();
            }
        }

        private void btn_back_Click(object sender, RoutedEventArgs e)
        {
            Hide();
            new UserWin(Login.currentUser).ShowDialog();
        }

        /// <summary>
        /// Фильтрация данных таблиц по логину и сортировка по дате
        /// </summary>
        public void DataGridFilter()
        {
            string dateString = "";
            var sortByDates = new List<login_history>();
            var login_history = skiresortEntities.GetContext().login_history.ToList();

            if(cbox_filter_login.SelectedIndex > 0)
                login_history = login_history.Where(p => p.user == cbox_filter_login.SelectedItem as user).ToList();

            if (ccbox_sort_date.IsChecked.Value)
            {
                int size = login_history.Count();
                int[] massIntDate = new int[size];
                int ident = 0;
                foreach (var date in login_history)
                {
                    dateString = "";
                    string shortDateString = date.date;
                    string[] masShortDateString = shortDateString.Split(new char[] { '-' });
                    for (i = 0; i < masShortDateString.Length; i++)
                        dateString += masShortDateString[i];
                    massIntDate[ident] = Convert.ToInt32(dateString);
                    ident++;
                }
                Array.Sort(massIntDate, (a, b) => b - a);
                for (i = 0; i < massIntDate.Length; i++)
                {
                    foreach (var date in login_history)
                    {
                        dateString = "";
                        string shortDateString = date.date;
                        string[] masShortDateString = shortDateString.Split(new char[] { '-' });
                        for (l = 0; l < masShortDateString.Length; l++)
                            dateString += masShortDateString[l];

                        if(massIntDate[i] == Convert.ToInt32(dateString))
                        {
                            sortByDates.Add(new login_history() { id = date.id, id_user = date.id_user, date = date.date, time = date.time, status = date.status, user = date.user });
                            break;
                        }
                    }
                }
            }
            if (ccbox_sort_date.IsChecked == false)
                dgrid_login_history.ItemsSource = login_history.ToList();
            else
                dgrid_login_history.ItemsSource = sortByDates;
        }

        private void cbox_filter_login_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            DataGridFilter();
        }

        private void ccbox_sort_date_Checked(object sender, RoutedEventArgs e)
        {
            DataGridFilter();
        }

        private void ccbox_sort_date_Unchecked(object sender, RoutedEventArgs e)
        {
            DataGridFilter();
        }
    }
}
