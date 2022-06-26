using SF2022User_NN_Lib;
using Prielbrusye.Windows;
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

namespace Prielbrusye
{
    /// <summary>
    /// Логика взаимодействия для UserWin.xaml
    /// </summary>
    public partial class UserWin : Window
    {
        public UserWin(user currentUser)
        {
            InitializeComponent();
            /// <summary>
            /// Таймер (10 минут)
            /// </summary>
            if (Models.Timer.limiter)
            {
                Models.Timer.TimerStart();
                TimerWork();
                Models.Timer.limiter = false;
            }
            timer_display.Text = Models.Timer.allTime.Hours.ToString() + ":" + Models.Timer.allTime.Minutes.ToString() + ":" + Models.Timer.allTime.Seconds.ToString();
            TimerMove();
            TimerCheck();
            /// <summary>
            /// Отображение нужных элементов исходя из роли авторизовавшегося пользователя
            /// </summary>
            DataContext = currentUser;
            if (currentUser.role == "Продавец")
            {
                Button btn_createOrder = new Button();
                btn_createOrder.Style = (Style)FindResource("button_style");
                btn_createOrder.Content = "Сформировать заказ";
                btn_createOrder.Width = 160;
                btn_createOrder.Name = "btn_createOrder";
                btn_createOrder.Margin = new Thickness(0, 30, 0, 0);
                btn_createOrder.Click += Btn_createOrder_Click;
                panel_tasks.Children.Add(btn_createOrder);
                Button btn_exit = new Button();
                btn_exit.Style = (Style)FindResource("button_style");
                btn_exit.Content = "Выход";
                btn_exit.Name = "btn_exit";
                btn_exit.Margin = new Thickness(0, 10, 0, 0);
                btn_exit.Click += Btn_exit_Click;
                panel_tasks.Children.Add(btn_exit);
                Button btn_free_time = new Button()
                {
                    Background = Brushes.White,
                    BorderThickness = new Thickness(0),
                    FontFamily = new FontFamily("Comic Sans MS"),
                    Width = 210,
                    Content = "Свободные временные интервалы",
                    Height = 25,
                    Margin = new Thickness(0, 20, 0, 0)
                };
                btn_free_time.Click += Btn_free_time_Click;
                free_time_panel.Children.Add(btn_free_time);
            }
            else if (currentUser.role == "Старший смены")
            {
                Button btn_createOrder = new Button();
                btn_createOrder.Style = (Style)FindResource("button_style");
                btn_createOrder.Content = "Сформировать заказ";
                btn_createOrder.Width = 160;
                btn_createOrder.Name = "btn_createOrder";
                btn_createOrder.Margin = new Thickness(0, 30, 0, 0);
                btn_createOrder.Click += Btn_createOrder_Click;
                panel_tasks.Children.Add(btn_createOrder);
                Button btn_acceptEquipment = new Button();
                btn_acceptEquipment.Style = (Style)FindResource("button_style");
                btn_acceptEquipment.Content = "Принять товары";
                btn_acceptEquipment.Name = "btn_acceptEquipment";
                btn_acceptEquipment.Margin = new Thickness(0, 10, 0, 0);
                btn_acceptEquipment.Click += Btn_acceptEquipment_Click;
                panel_tasks.Children.Add(btn_acceptEquipment);
                Button btn_exit = new Button();
                btn_exit.Style = (Style)FindResource("button_style");
                btn_exit.Content = "Выход";
                btn_exit.Name = "btn_exit";
                btn_exit.Margin = new Thickness(0, 10, 0, 0);
                btn_exit.Click += Btn_exit_Click;
                panel_tasks.Children.Add(btn_exit);
                Button btn_free_time = new Button()
                {
                    Background = Brushes.White,
                    BorderThickness = new Thickness(0),
                    FontFamily = new FontFamily("Comic Sans MS"),
                    Width = 210,
                    Content = "Свободные временные интервалы",
                    Height = 25,
                    Margin = new Thickness(0, 20, 0, 0)
                };
                btn_free_time.Click += Btn_free_time_Click1; ;
                free_time_panel.Children.Add(btn_free_time);
            }
            else if (currentUser.role == "Администратор")
            {
                Button btn_createRequest = new Button();
                btn_createRequest.Style = (Style)FindResource("button_style");
                btn_createRequest.Content = "Сформировать отчеты";
                btn_createRequest.Width = 160;
                btn_createRequest.Name = "btn_createRequest";
                btn_createRequest.Margin = new Thickness(0, 30, 0, 0);
                btn_createRequest.Click += Btn_createRequest_Click;
                panel_tasks.Children.Add(btn_createRequest);
                Button btn_enterHistoty = new Button();
                btn_enterHistoty.Style = (Style)FindResource("button_style");
                btn_enterHistoty.Content = "История входа пользователей";
                btn_enterHistoty.Width = 200;
                btn_enterHistoty.Name = "btn_enterHistory";
                btn_enterHistoty.Margin = new Thickness(0, 10, 0, 0);
                btn_enterHistoty.Click += Btn_enterHistoty_Click;
                panel_tasks.Children.Add(btn_enterHistoty);
                Button btn_consumables = new Button();
                btn_consumables.Style = (Style)FindResource("button_style");
                btn_consumables.Content = "Расходные материалы";
                btn_consumables.Width = 160;
                btn_consumables.Name = "btn_consumables";
                btn_consumables.Margin = new Thickness(0, 10, 0, 0);
                btn_consumables.Click += Btn_consumables_Click;
                panel_tasks.Children.Add(btn_consumables);
                Button btn_exit = new Button();
                btn_exit.Style = (Style)FindResource("button_style");
                btn_exit.Content = "Выход";
                btn_exit.Name = "btn_exit";
                btn_exit.Margin = new Thickness(0, 10, 0, 0);
                btn_exit.Click += Btn_exit_Click;
                panel_tasks.Children.Add(btn_exit);
                Button btn_free_time = new Button()
                {
                    Background = Brushes.White,
                    BorderThickness = new Thickness(0),
                    FontFamily = new FontFamily("Comic Sans MS"),
                    Width = 210,
                    Content = "Свободные временные интервалы",
                    Height = 25,
                    Margin = new Thickness(0, 20, 0, 0)
                };
                btn_free_time.Click += Btn_free_time_Click2; ;
                free_time_panel.Children.Add(btn_free_time);
            }
        }

        private void Btn_free_time_Click2(object sender, RoutedEventArgs e)
        {
            StringBuilder free_time = new StringBuilder();
            TimeSpan[] ts = new TimeSpan[5] { new TimeSpan(10, 0, 0), new TimeSpan(11, 0, 0), new TimeSpan(15, 0, 0), new TimeSpan(15, 30, 0), new TimeSpan(16, 50, 0) };
            int[] ds = new int[5] { 60, 30, 10, 10, 40 };
            TimeSpan begin = new TimeSpan(10, 0, 0);
            TimeSpan end = new TimeSpan(18, 0, 0);
            int time = 30;
            string[] s;
            s = Calculations.AvailablePeriods(ts, ds, begin, end, time);
            int a = 0;
            for (int i = 0; i < s.Length; i++)
            {
                free_time.AppendLine(s[i]);
            }
            MessageBox.Show(free_time.ToString(), "Свободные временные интервалы", MessageBoxButton.OK, MessageBoxImage.None);
        }

        private void Btn_free_time_Click1(object sender, RoutedEventArgs e)
        {
            StringBuilder free_time = new StringBuilder();
            TimeSpan[] ts = new TimeSpan[5] { new TimeSpan(10, 0, 0), new TimeSpan(11, 0, 0), new TimeSpan(15, 0, 0), new TimeSpan(15, 30, 0), new TimeSpan(16, 50, 0) };
            int[] ds = new int[5] { 60, 30, 10, 10, 40 };
            TimeSpan begin = new TimeSpan(9, 0, 0);
            TimeSpan end = new TimeSpan(18, 0, 0);
            int time = 30;
            string[] s;
            s = Calculations.AvailablePeriods(ts, ds, begin, end, time);
            int a = 0;
            for (int i = 0; i < s.Length; i++)
            {
                free_time.AppendLine(s[i]);
            }
            MessageBox.Show(free_time.ToString(), "Свободные временные интервалы", MessageBoxButton.OK, MessageBoxImage.None);
        }

        private void Btn_free_time_Click(object sender, RoutedEventArgs e)
        {
            StringBuilder free_time = new StringBuilder();
            TimeSpan[] ts = new TimeSpan[5] { new TimeSpan(10, 0, 0), new TimeSpan(11, 0, 0), new TimeSpan(15, 0, 0), new TimeSpan(15, 30, 0), new TimeSpan(16, 50, 0) };
            int[] ds = new int[5] { 60, 30, 10, 10, 40 };
            TimeSpan begin = new TimeSpan(8, 0, 0);
            TimeSpan end = new TimeSpan(18, 0, 0);
            int time = 30;
            string[] s;
            s = Calculations.AvailablePeriods(ts, ds, begin, end, time);
            int a = 0;
            for (int i = 0; i < s.Length; i++)
            {
                free_time.AppendLine(s[i]);
            }
            MessageBox.Show(free_time.ToString(), "Свободные временные интервалы", MessageBoxButton.OK, MessageBoxImage.None);
        }

        /// <summary>
        /// Работа таймера
        /// </summary>
        public async void TimerWork()
        {
            while(Models.Timer.allTime.TotalSeconds > 0)
            {
                await Task.Delay(1000);
                Models.Timer.allTime -= new TimeSpan(0, 0, 1);
                timer_display.Text = Models.Timer.allTime.Hours.ToString() + ":" + Models.Timer.allTime.Minutes.ToString() + ":" + Models.Timer.allTime.Seconds.ToString();
            }
        }

        /// <summary>
        /// Отображение таймера
        /// </summary>
        public async void TimerMove()
        {
            while(Models.Timer.allTime.TotalSeconds > 0)
            {
                await Task.Delay(1000);
                timer_display.Text = Models.Timer.allTime.Hours.ToString() + ":" + Models.Timer.allTime.Minutes.ToString() + ":" + Models.Timer.allTime.Seconds.ToString();
            }
        }

        /// <summary>
        /// Проверка на окончание таймера, а также предупреждение после 5 минут сеанса
        /// </summary>
        public async void TimerCheck()
        {
            while (true)
            {
                await Task.Delay(1000);
                if(Models.Timer.allTime.TotalSeconds == 300)
                {
                    MessageBox.Show("Осталось 5 минут сеанса!", "Внимание", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
                if(Models.Timer.allTime.TotalSeconds == 0)
                {
                    Models.Timer.limiter = true;
                    Hide();
                    new Login(2).ShowDialog();
                    break;
                }
            }
        }

        private void Btn_acceptEquipment_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("В разработке", "Внимание", MessageBoxButton.OK, MessageBoxImage.Warning);
        }

        private void Btn_consumables_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("В разработке", "Внимание", MessageBoxButton.OK, MessageBoxImage.Warning);
        }

        private void Btn_enterHistoty_Click(object sender, RoutedEventArgs e)
        {
            Hide();
            new LoginHistory().ShowDialog();
        }

        private void Btn_createRequest_Click(object sender, RoutedEventArgs e)
        {
            Hide();
            new GeneratingReports().ShowDialog();
        }

        private void Btn_exit_Click(object sender, RoutedEventArgs e)
        {
            Models.Timer.limiter = true;
            Hide();
            new Login().ShowDialog();
        }

        private void Btn_createOrder_Click(object sender, RoutedEventArgs e)
        {
            Hide();
            new CreateOrder().ShowDialog();
        }
    }
}
