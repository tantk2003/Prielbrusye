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
    /// Логика взаимодействия для CreateOrder.xaml
    /// </summary>
    public partial class CreateOrder : Window
    {
        public int i = 0;
        public order lastOrder = null;
        public Random rnd = new Random();
        private string numBaracode = "";
        public int counter = 0;
        private string date_start = "", time_start = "";
        private string order_code = "";
        private int time_rental = 0;
        public CreateOrder()
        {
            InitializeComponent();
            timer_display.Text = Models.Timer.allTime.Hours.ToString() + ":" + Models.Timer.allTime.Minutes.ToString() + ":" + Models.Timer.allTime.Seconds.ToString();
            TimerMove();
            getLastOrder();
        }

        /// <summary>
        /// Подключение в качестве источников данных элементов таблицы из БД
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Window_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            cbox_services.ItemsSource = skiresortEntities.GetContext().services.ToList();
            lbox_services.ItemsSource = skiresortEntities.GetContext().services.ToList();
            cbox_clients.ItemsSource = skiresortEntities.GetContext().clients.OrderBy(x => x.snp).ToList();
        }

        /// <summary>
        /// Вывод код последнего заказа из БД в текстовое поле
        /// </summary>
        public void getLastOrder()
        {
            using (skiresortEntities db = new skiresortEntities())
            {
                foreach (order order in db.orders)
                {
                    lastOrder = order;
                }
            }
            string orderCode = lastOrder.order_code;
            tbox_baraCode.Text = orderCode;
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

        /// <summary>
        /// Событие нажатия на кнопку, после чего сгенерируется штрих-код
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tbox_baraCode_KeyUp(object sender, KeyEventArgs e)
        {
            if (e.Key == Key.Enter)
            {
                numBaracode = "";
                StringBuilder errors = new StringBuilder();
                if (tbox_timeRental.Text.Length == 0)
                    errors.AppendLine("Вы не ввели время проката");
                if (tbox_baraCode.Text.Length == 0)
                    errors.AppendLine("Вы не ввели код заказа");
                if (checkString(tbox_baraCode.Text) == false)
                    errors.AppendLine("Код заказа должен состоять только из цифр");
                if (checkString(tbox_timeRental.Text) == false)
                    errors.AppendLine("Вы ввели время проката некорректно");
                if (tbox_baraCode.Text.Length != 7)
                    errors.AppendLine("Код заказа должен состоять из 7 символов");

                if(errors.Length > 0)
                {
                    MessageBox.Show(errors.ToString(), "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                    return;
                }
                baracode_field.Children.Clear();
                getNumBaracode(tbox_baraCode.Text);
                generateBaracode(numBaracode);
            }
        }

        /// <summary>
        /// Состоит ли строка только из чисел?
        /// </summary>
        /// <param name="str"></param>
        /// <returns></returns>
        public bool checkString(string str)
        {
            int checkId = 0;
            for (i = 0; i < str.Length; i++)
            {
                if (str[i] != '1' && str[i] != '2' && str[i] != '3' && str[i] != '4' && str[i] != '5' &&
                    str[i] != '6' && str[i] != '7' && str[i] != '8' && str[i] != '9' && str[i] != '0')
                {
                    checkId = 1;
                    break;
                }
            }
            if (checkId == 0)
                return true;
            else
                return false;
        }

        /// <summary>
        /// Формирование номера штрих-кода
        /// </summary>
        /// <param name="orderCode"></param>
        public void getNumBaracode(string orderCode)
        {
            string date = "";
            string time = "";
            string month = "", day = "";

            if(DateTime.Now.Month.ToString().Length == 1)
                month = "0" + DateTime.Now.Month.ToString();
            else
                month = DateTime.Now.Month.ToString();
            if(DateTime.Now.Day.ToString().Length == 1)
                day = "0" + DateTime.Now.Day.ToString();
            else
                day = DateTime.Now.Day.ToString();

            date_start = DateTime.Now.Year.ToString() + "-" + month + "-" + day;
            time_start = DateTime.Now.ToShortTimeString();   
            for(i = 0; i < date_start.Length; i++)
            {
                if (date_start[i] != '-')
                    date += date_start[i];
            }
            for (i = 0; i < time_start.Length; i++)
            {
                if (time_start[i] != ':')
                    time += time_start[i];
            }

            string timeRental = tbox_timeRental.Text;
            time_rental = Convert.ToInt32(tbox_timeRental.Text);
            order_code = tbox_baraCode.Text;
            numBaracode += orderCode + date + time + timeRental + rnd.Next(100000, 999999).ToString();
        }

        /// <summary>
        /// Генерация штрих-кода
        /// </summary>
        /// <param name="code"></param>
        public void generateBaracode(string code)
        {
            double marginLeft = 0;
            for (i = 0; i < code.Length; i++)
            {
                Rectangle streak = new Rectangle();
                streak.VerticalAlignment = VerticalAlignment.Top;
                streak.HorizontalAlignment = HorizontalAlignment.Left;
                if (i == 0 || i == 1 || i == code.Length / 2 + 1 || i == code.Length / 2 || i == code.Length - 1 || i == code.Length - 2)
                    streak.Height = 110.5 + 20.7;
                else
                    streak.Height = 110.5;

                if (code[i] != '0')
                    streak.Fill = Brushes.Black;
                else
                    streak.Fill = Brushes.White;

                switch (code[i])
                {
                    case '1': streak.Width = 1 * 1; break;
                    case '2': streak.Width = 1 * 2; break;
                    case '3': streak.Width = 1 * 3; break;
                    case '4': streak.Width = 1 * 4; break;
                    case '5': streak.Width = 1 * 5; break;
                    case '6': streak.Width = 1 * 6; break;
                    case '7': streak.Width = 1 * 7; break;
                    case '8': streak.Width = 1 * 8; break;
                    case '9': streak.Width = 1 * 9; break;
                    case '0': streak.Width = 4; break;
                }
                if (i == 0)
                {
                    marginLeft = 100;
                    streak.Margin = new Thickness(marginLeft, 30, 0, 0);
                }
                else
                {
                    marginLeft = 7;
                    streak.Margin = new Thickness(marginLeft, 30, 0, 0);
                }
                baracode_field.Children.Add(streak);
            }
            string first = code[0].ToString();
            string second = "";
            string third = "";
            int half = (code.Length + 3) / 2;
            for (i = 1; i < half; i++)
            {
                second += code[i];
            }
            for (i = half; i < code.Length; i++)
            {
                third += code[i];
            }

            TextBlock firstLetterBaracode = new TextBlock() { Text = first, FontFamily = new FontFamily("Comic Sans MS"), FontSize = 13, Margin = new Thickness(-270, 160, 0, 0) };
            TextBlock leftHalfBaracode = new TextBlock() { Text = second, FontFamily = new FontFamily("Comic Sans MS"), FontSize = 13, Margin = new Thickness(-236, 160, 0, 0) };
            TextBlock rightHalfBaracode = new TextBlock() { Text = third, FontFamily = new FontFamily("Comic Sans MS"), FontSize = 13, Margin = new Thickness(-110, 160, 0, 0) };
            baracode_field.Children.Add(firstLetterBaracode);
            baracode_field.Children.Add(leftHalfBaracode);
            baracode_field.Children.Add(rightHalfBaracode);
        }      

        /// <summary>
        /// Печать штрих-кода
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_printBaracode_Click(object sender, RoutedEventArgs e)
        {
            PrintDialog printDialog = new PrintDialog();
            if(printDialog.ShowDialog() == true)
            {
                printDialog.PrintVisual(baracode_field, "Штрих-код заказа");
                selection_panel.Visibility = Visibility.Visible;
                btn_create_order.Visibility = Visibility.Visible;
            }
        }

        private void btn_add_client_Click(object sender, RoutedEventArgs e)
        {
            new AddClient().ShowDialog();
        }

        /// <summary>
        /// Добавление нескольких услуг
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_add_service_Click(object sender, RoutedEventArgs e)
        {
            counter++;
            if(counter == 1)
                lbox_services.Visibility = Visibility.Visible;
            else
            {
                lbox_services.Visibility = Visibility.Hidden;
                counter = 0;
            }
        }

        /// <summary>
        /// Обновление списка клиентов в ComboBox
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_refresh_clients_list_Click(object sender, RoutedEventArgs e)
        {
            cbox_clients.ItemsSource = skiresortEntities.GetContext().clients.OrderBy(x => x.snp).ToList();
        }

        private void btn_back_Click(object sender, RoutedEventArgs e)
        {
            Hide();
            new UserWin(Login.currentUser).ShowDialog();
        }

        /// <summary>
        /// Поиск клиента по ФИО
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tbox_clients_search_TextChanged(object sender, TextChangedEventArgs e)
        {
            var clients = skiresortEntities.GetContext().clients.OrderBy(x=>x.snp).ToList();           
            clients = clients.Where(x => x.snp.ToLower().Contains(tbox_clients_search.Text.ToLower())).ToList();
            cbox_clients.ItemsSource = clients.ToList();
        }

        /// <summary>
        /// Поиск услуги по ее названию
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tbox_services_search_TextChanged(object sender, TextChangedEventArgs e)
        {
            var services = skiresortEntities.GetContext().services.ToList();
            services = services.Where(x => x.name.ToLower().Contains(tbox_services_search.Text.ToLower())).ToList();
            cbox_services.ItemsSource = services.ToList();
            lbox_services.ItemsSource = services.ToList();
        }

        /// <summary>
        /// Добавление новой услуги в БД и печать электронного чека
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_create_order_Click(object sender, RoutedEventArgs e)
        {
            StringBuilder errors = new StringBuilder();
            if (cbox_clients.SelectedItem == null)
                errors.AppendLine("Вы не выбрали клиента");

            if (cbox_services.SelectedItem == null)
                errors.AppendLine("Вы не выбрали услугу");

            if(errors.Length > 0)
            {
                MessageBox.Show(errors.ToString(), "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            List<string> services = new List<string>();
            string client_snp = cbox_clients.SelectedValue.ToString();
            services.Add(cbox_services.SelectedValue.ToString());
            int id_client = 0;
            string client_address = "";
            int total_price = 0;
            string services_list_N = "", services_list = "";
            var more_services = lbox_services.SelectedItems.Cast<service>().ToList();

            if (more_services.Count > 0)
            {
                foreach (var service in more_services)
                {
                    services.Add(service.name);
                }
            }

            using(skiresortEntities db = new skiresortEntities())
            {
                foreach(client client in db.clients)
                {
                    if(client.snp == client_snp)
                    {
                        id_client = client.id;
                        client_address = client.address;
                    }
                }

                foreach(string name in services)
                {
                    foreach(service service in db.services)
                    {
                        if(name == service.name)
                        {
                            services_list_N += service.id + " ";
                        }
                    }
                }

                for (i = 0; i < services_list_N.Length - 1; i++)
                    services_list += services_list_N[i];

                order new_order = new order();
                new_order.order_code = order_code;
                new_order.date_start = date_start;
                new_order.time = time_start;
                new_order.id_client = id_client;
                new_order.services = services_list;
                new_order.time_rental = time_rental;
                new_order.status = 1;
                db.orders.Add(new_order);
                try
                {
                    db.SaveChanges();
                    MessageBox.Show("Заказ оформлен", "Успешно", MessageBoxButton.OK, MessageBoxImage.None);
                }   
                catch(Exception ex)
                {
                    MessageBox.Show(ex.Message.ToString(), "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
                StackPanel electronic_receipt = new StackPanel();
                electronic_receipt.Orientation = Orientation.Vertical;
                TextBlock headline = new TextBlock() { Text = "Электронный чек", FontFamily = new FontFamily("Times New Roman"), FontSize = 19, Margin = new Thickness(80, 80, 0, 0) };
                headline.FontWeight = FontWeights.Bold;
                TextBlock tblock_date_order = new TextBlock() { Text = $"Дата заказа: {date_start} {time_start}", FontFamily = new FontFamily("Times New Roman"), FontSize = 15, Margin = new Thickness(80, 40, 0, 0) };
                TextBlock tblock_id_client = new TextBlock() { Text = $"Код клиента: {id_client}", FontFamily = new FontFamily("Times New Roman"), FontSize = 15, Margin = new Thickness(80, 10, 0, 0) };
                TextBlock tblock_order_code = new TextBlock() { Text = $"Код заказа: {order_code}", FontFamily = new FontFamily("Times New Roman"), FontSize = 15, Margin = new Thickness(80, 10, 0, 0) };
                TextBlock tblock_client_snp = new TextBlock() { Text = $"ФИО клиента: {client_snp}", FontFamily = new FontFamily("Times New Roman"), FontSize = 15, Margin = new Thickness(80, 10, 0, 0) };
                TextBlock tblock_client_address = new TextBlock() { Text = $"Адрес проживания клиента: {client_address}", FontFamily = new FontFamily("Times New Roman"), FontSize = 15, Margin = new Thickness(80, 10, 0, 0) };
                TextBlock tblock_services = new TextBlock() { Text = "Перечень услуг:\n", FontFamily = new FontFamily("Times New Roman"), FontSize = 15, Margin = new Thickness(80, 10, 0, 0) };
                string[] services_array = services_list.Split(new char[] {' '}, StringSplitOptions.RemoveEmptyEntries);
                for(i = 0; i < services_array.Length; i++)
                {
                    foreach(service service in skiresortEntities.GetContext().services)
                    {
                        if(Convert.ToInt32(services_array[i]) == service.id)
                        {
                            tblock_services.Text += service.name + "\n";
                            total_price += service.price;
                        }
                    }
                }
                TextBlock tblock_time_rental = new TextBlock() { Text = $"Время проката: {time_rental}", FontFamily = new FontFamily("Times New Roman"), FontSize = 15, Margin = new Thickness(80, 10, 0, 0) };
                TextBlock tblock_total_price = new TextBlock() { Text = $"ИТОГО: {total_price*time_rental}Р", FontFamily = new FontFamily("Times New Roman"), FontSize = 15, Margin = new Thickness(80, 40, 0, 0) };
                tblock_total_price.FontWeight = FontWeights.Bold;
                electronic_receipt.Children.Add(headline);
                electronic_receipt.Children.Add(tblock_date_order);
                electronic_receipt.Children.Add(tblock_id_client);
                electronic_receipt.Children.Add(tblock_order_code);
                electronic_receipt.Children.Add(tblock_client_snp);
                electronic_receipt.Children.Add(tblock_client_address);
                electronic_receipt.Children.Add(tblock_services);
                electronic_receipt.Children.Add(tblock_time_rental);
                electronic_receipt.Children.Add(tblock_total_price);
                PrintDialog print = new PrintDialog();
                if(print.ShowDialog() == true)
                {
                    print.PrintVisual(electronic_receipt, "Электронный чек");
                }
            }
        }
    }
}
