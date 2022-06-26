using System;
using System.Collections.Generic;
using System.Data;
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
    /// Логика взаимодействия для GeneratingReports.xaml
    /// </summary>
    public partial class GeneratingReports : Window
    {
        public Calendar calendar;
        public Button btn_generate_report1;
        public Button btn_generate_report2;
        public Button btn_generate_report3;
        ComboBox cbox_services;
        public List<Models.NumOfOrdersOrServices> num_of_orders_or_services = new List<Models.NumOfOrdersOrServices>();
        public int i = 0;
        public GeneratingReports()
        {
            InitializeComponent();
            timer_display.Text = Models.Timer.allTime.Hours.ToString() + ":" + Models.Timer.allTime.Minutes.ToString() + ":" + Models.Timer.allTime.Seconds.ToString();
            TimerMove();
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
        /// Выбор печати отчета типа "Количество оказанных услуг по дням за период времени" и динамическое создание нужных элементов для печати
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_report1_Click(object sender, RoutedEventArgs e)
        {
            tblock_none.Visibility = Visibility.Hidden;
            report.Visibility = Visibility.Visible;
            reports_field.Children.Clear();
            dgrid_report.Columns.Clear();
            dgrid_report.ItemsSource = null;
            TextBlock header_date = new TextBlock() { Text = "Выберите промежуток дат (зажав ЛКМ)", FontFamily = new FontFamily("Comic Sans MS"), Foreground = Brushes.DimGray, FontSize = 16, Margin = new Thickness(50, 20, 0, 0) };
            calendar = new Calendar() { BorderThickness = new Thickness(0, 0, 0, 0), SelectionMode = CalendarSelectionMode.SingleRange, HorizontalAlignment = HorizontalAlignment.Left, Margin = new Thickness(50, 10, 0, 0), Name = "calendar" };
            btn_generate_report1 = new Button() { Style = (Style)FindResource("button_style"), Content = "Сформировать отчет", HorizontalAlignment = HorizontalAlignment.Left, Margin = new Thickness(50, 20, 0, 0) };
            btn_generate_report1.Click += Btn_generate_report_Click;
            reports_field.Children.Add(header_date);
            reports_field.Children.Add(calendar);
            reports_field.Children.Add(btn_generate_report1);
            DataGridTextColumn dataGridTextColumn_days = new DataGridTextColumn() { Header = "День", Width = 100, Binding = new Binding("day") };
            DataGridTextColumn dataGridTextColumn_services = new DataGridTextColumn() { Header = "Кол-во оказанных услуг", Width = 250, Binding = new Binding("count") };
            dgrid_report.Columns.Add(dataGridTextColumn_days);
            dgrid_report.Columns.Add(dataGridTextColumn_services);
        }

        /// <summary>
        /// Событие нажатия, после которого формируются отчеты в зависимости от выбранного формата отчета
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Btn_generate_report_Click(object sender, RoutedEventArgs e)
        {
            if (rbtn_doc.IsChecked == true)
                printDoc(num_of_orders_or_services, 1);
            if (rbtn_graph.IsChecked == true)
                printGraph(num_of_orders_or_services, 1);
            if (rbtn_doc_and_graph.IsChecked == true)
            {
                printDoc(num_of_orders_or_services, 1);
                printGraph(num_of_orders_or_services, 1);
            }
        }

        /// <summary>
        /// Выбор печати отчета типа "Количество заказов по дням за период времени по каждой услуге" и динамическое создание нужных элементов для печати
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_report2_Click(object sender, RoutedEventArgs e)
        {
            tblock_none.Visibility = Visibility.Hidden;
            report.Visibility = Visibility.Visible;
            reports_field.Children.Clear();
            dgrid_report.Columns.Clear();
            dgrid_report.ItemsSource = null;
            TextBlock header_date = new TextBlock() { Text = "Выберите промежуток дат (зажав ЛКМ)", FontFamily = new FontFamily("Comic Sans MS"), Foreground = Brushes.DimGray, FontSize = 16, Margin = new Thickness(50, 20, 0, 0) };
            calendar = new Calendar() { BorderThickness = new Thickness(0, 0, 0, 0), SelectionMode = CalendarSelectionMode.SingleRange, HorizontalAlignment = HorizontalAlignment.Left, Margin = new Thickness(50, 10, 0, 0), Name = "calendar" };
            btn_generate_report2 = new Button() { Style = (Style)FindResource("button_style"), Content = "Сформировать отчет", HorizontalAlignment = HorizontalAlignment.Left, Margin = new Thickness(50, 20, 0, 0) };
            btn_generate_report2.Click += Btn_generate_report_Click1;
            TextBlock header_service = new TextBlock() { Text = "Выберите услугу", FontFamily = new FontFamily("Comic Sans MS"), Foreground = Brushes.DimGray, FontSize = 16, Margin = new Thickness(50, 20, 0, 0) };
            cbox_services = new ComboBox() { Style = (Style)FindResource("combobox_style"), DisplayMemberPath = "name", SelectedValuePath = "name", HorizontalAlignment = HorizontalAlignment.Left, Margin = new Thickness(50, 20, 0, 0) };
            cbox_services.ItemsSource = skiresortEntities.GetContext().services.ToList();
            reports_field.Children.Add(header_date);
            reports_field.Children.Add(calendar);
            reports_field.Children.Add(header_service);
            reports_field.Children.Add(cbox_services);
            reports_field.Children.Add(btn_generate_report2);
            DataGridTextColumn dataGridTextColumn_days = new DataGridTextColumn() { Header = "День", Width = 100, Binding = new Binding("day") };
            DataGridTextColumn dataGridTextColumn_services = new DataGridTextColumn() { Header = "Кол-во заказов", Width = 250, Binding = new Binding("count") };
            dgrid_report.Columns.Add(dataGridTextColumn_days);
            dgrid_report.Columns.Add(dataGridTextColumn_services);
        }

        /// <summary>
        /// Событие нажатия, после которого формируются отчеты в зависимости от выбранного формата отчета
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Btn_generate_report_Click1(object sender, RoutedEventArgs e)
        {
            if (rbtn_doc.IsChecked == true)
                printDoc(num_of_orders_or_services, 2);
            if (rbtn_graph.IsChecked == true)
                printGraph(num_of_orders_or_services, 2);
            if (rbtn_doc_and_graph.IsChecked == true)
            {
                printDoc(num_of_orders_or_services, 2);
                printGraph(num_of_orders_or_services, 2);
            }
        }

        /// <summary>
        /// Выбор печати отчета типа "Количество заказов по дням за период времени" и динамическое создание нужных элементов для печати
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_report3_Click(object sender, RoutedEventArgs e)
        {
            tblock_none.Visibility = Visibility.Hidden;
            report.Visibility = Visibility.Visible;
            reports_field.Children.Clear();
            dgrid_report.Columns.Clear();
            dgrid_report.ItemsSource = null;
            TextBlock header_date = new TextBlock() { Text = "Выберите промежуток дат (зажав ЛКМ)", FontFamily = new FontFamily("Comic Sans MS"), Foreground = Brushes.DimGray, FontSize = 16, Margin = new Thickness(50, 20, 0, 0) };
            calendar = new Calendar() { BorderThickness = new Thickness(0, 0, 0, 0), SelectionMode = CalendarSelectionMode.SingleRange, HorizontalAlignment = HorizontalAlignment.Left, Margin = new Thickness(50, 10, 0, 0), Name = "calendar" };
            btn_generate_report3 = new Button() { Style = (Style)FindResource("button_style"), Content = "Сформировать отчет", HorizontalAlignment = HorizontalAlignment.Left, Margin = new Thickness(50, 20, 0, 0) };
            btn_generate_report3.Click += Btn_generate_report_Click2;
            reports_field.Children.Add(header_date);
            reports_field.Children.Add(calendar);
            reports_field.Children.Add(btn_generate_report3);
            DataGridTextColumn dataGridTextColumn_days = new DataGridTextColumn() { Header = "День", Width = 100, Binding = new Binding("day") };
            DataGridTextColumn dataGridTextColumn_services = new DataGridTextColumn() { Header = "Кол-во заказов", Width = 250, Binding = new Binding("count") };
            dgrid_report.Columns.Add(dataGridTextColumn_days);
            dgrid_report.Columns.Add(dataGridTextColumn_services);
        }

        /// <summary>
        /// Событие нажатия, после которого формируются отчеты в зависимости от выбранного формата отчета
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Btn_generate_report_Click2(object sender, RoutedEventArgs e)
        {
            if (rbtn_doc.IsChecked == true)
                printDoc(num_of_orders_or_services, 3);
            if (rbtn_graph.IsChecked == true)
                printGraph(num_of_orders_or_services, 3);
            if (rbtn_doc_and_graph.IsChecked == true)
            {
                printDoc(num_of_orders_or_services, 3);
                printGraph(num_of_orders_or_services, 3);
            }
        }

        /// <summary>
        /// Формирование отчета в виде таблицы в зависимости от статуса (типа отчета)
        /// </summary>
        /// <param name="num_of_orders_or_services"></param>
        /// <param name="status"></param>
        public void printDoc(List<Models.NumOfOrdersOrServices> num_of_orders_or_services, int status)
        {
            if (status == 1)
            {
                num_of_orders_or_services.Clear();
                bool dateCheck = false;
                int count = 0;
                string currentDay = "";
                List<DateTime> days = calendar.SelectedDates.ToList();
                foreach (DateTime day in days)
                {
                    string yyyy = day.Year.ToString();
                    string mm = day.Month.ToString();
                    if (mm.Length == 1)
                        mm = "0" + mm;
                    string dd = day.Day.ToString();
                    if (dd.Length == 1)
                        dd = "0" + dd;
                    currentDay = yyyy + "-" + mm + "-" + dd;
                    foreach (order order in skiresortEntities.GetContext().orders)
                    {
                        if (currentDay == order.date_start)
                        {
                            dateCheck = true;
                            for (i = 0; i < order.services.Length; i++)
                            {
                                if (order.services[i] == ' ')
                                {
                                    count++;
                                }
                            }
                            count++;
                        }
                    }
                    if (dateCheck)
                        num_of_orders_or_services.Add(new Models.NumOfOrdersOrServices { day = currentDay, count = count });
                    else
                        num_of_orders_or_services.Add(new Models.NumOfOrdersOrServices { day = currentDay, count = 0 });

                    dateCheck = false;
                    count = 0;
                }
                dgrid_report.ItemsSource = num_of_orders_or_services.ToList();
                StackPanel doc = new StackPanel();
                doc.Orientation = Orientation.Vertical;
                TextBlock headline = new TextBlock() { Text = "Количество оказанных услуг по дням за период времени", FontFamily = new FontFamily("Times New Roman"), FontSize = 21, Margin = new Thickness(80, 80, 0, 0) };
                headline.FontWeight = FontWeights.Bold;
                doc.Children.Add(headline);
                TextBlock report = new TextBlock();
                report.FontFamily = new FontFamily("Times New Roman");
                report.FontSize = 16;
                report.Text = "День" + "\t\t" + "Кол-во оказанных услуг" + "\n\n";
                report.Margin = new Thickness(80, 50, 0, 0);
                foreach (Models.NumOfOrdersOrServices data in num_of_orders_or_services)
                {
                    report.Text += data.day + "\t" + data.count + "\n";
                }
                doc.Children.Add(report);
                PrintDialog print = new PrintDialog();
                if (print.ShowDialog() == true)
                {
                    print.PrintVisual(doc, "Количество оказанных услуг по дням за период времени");
                }
            }
            else if (status == 2)
            {
                num_of_orders_or_services.Clear();
                bool dateCheck = false;
                int count = 0;
                string currentDay = "";
                string selectedService = cbox_services.SelectedValue.ToString();
                int id_service = 0;
                foreach (service service in skiresortEntities.GetContext().services)
                {
                    if (service.name == selectedService)
                        id_service = service.id;
                }
                List<DateTime> days = calendar.SelectedDates.ToList();
                foreach (DateTime day in days)
                {
                    string yyyy = day.Year.ToString();
                    string mm = day.Month.ToString();
                    if (mm.Length == 1)
                        mm = "0" + mm;
                    string dd = day.Day.ToString();
                    if (dd.Length == 1)
                        dd = "0" + dd;
                    currentDay = yyyy + "-" + mm + "-" + dd;
                    foreach (order order in skiresortEntities.GetContext().orders)
                    {
                        if (currentDay == order.date_start)
                        {
                            dateCheck = true;
                            string[] services_id_array = order.services.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                            for (i = 0; i < services_id_array.Length; i++)
                            {
                                if (services_id_array[i] == id_service.ToString())
                                {
                                    count++;
                                }
                            }
                        }
                    }
                    if (dateCheck)
                        num_of_orders_or_services.Add(new Models.NumOfOrdersOrServices { day = currentDay, count = count });
                    else
                        num_of_orders_or_services.Add(new Models.NumOfOrdersOrServices { day = currentDay, count = 0 });

                    dateCheck = false;
                    count = 0;
                }
                dgrid_report.ItemsSource = num_of_orders_or_services.ToList();
                StackPanel doc = new StackPanel();
                doc.Orientation = Orientation.Vertical;
                TextBlock headline = new TextBlock() { Text = "Количество заказов по дням за период времени по услуге:\n", FontFamily = new FontFamily("Times New Roman"), FontSize = 21, Margin = new Thickness(80, 80, 0, 0) };
                headline.Text += selectedService;
                headline.FontWeight = FontWeights.Bold;
                doc.Children.Add(headline);
                TextBlock report = new TextBlock();
                report.FontFamily = new FontFamily("Times New Roman");
                report.FontSize = 16;
                report.Margin = new Thickness(80, 50, 0, 0);
                report.Text += "День" + "\t\t" + "Кол-во заказов" + "\n\n";
                foreach (Models.NumOfOrdersOrServices data in num_of_orders_or_services)
                {
                    report.Text += data.day + "\t" + data.count + "\n";
                }
                doc.Children.Add(report);
                PrintDialog print = new PrintDialog();
                if (print.ShowDialog() == true)
                {
                    print.PrintVisual(doc, $"Количество заказов по дням за период времени по услуге: {selectedService}");
                }
            }
            else if (status == 3)
            {
                num_of_orders_or_services.Clear();
                bool dateCheck = false;
                int count = 0;
                string currentDay = "";
                List<DateTime> days = calendar.SelectedDates.ToList();
                foreach (DateTime day in days)
                {
                    string yyyy = day.Year.ToString();
                    string mm = day.Month.ToString();
                    if (mm.Length == 1)
                        mm = "0" + mm;
                    string dd = day.Day.ToString();
                    if (dd.Length == 1)
                        dd = "0" + dd;
                    currentDay = yyyy + "-" + mm + "-" + dd;
                    foreach (order order in skiresortEntities.GetContext().orders)
                    {
                        if (currentDay == order.date_start)
                        {
                            dateCheck = true;
                            count++;
                        }
                    }
                    if (dateCheck)
                        num_of_orders_or_services.Add(new Models.NumOfOrdersOrServices { day = currentDay, count = count });
                    else
                        num_of_orders_or_services.Add(new Models.NumOfOrdersOrServices { day = currentDay, count = 0 });

                    dateCheck = false;
                    count = 0;
                }
                dgrid_report.ItemsSource = num_of_orders_or_services.ToList();
                StackPanel doc = new StackPanel();
                doc.Orientation = Orientation.Vertical;
                TextBlock headline = new TextBlock() { Text = "Количество заказов по дням за период времени", FontFamily = new FontFamily("Times New Roman"), FontSize = 21, Margin = new Thickness(80, 80, 0, 0) };
                headline.FontWeight = FontWeights.Bold;
                doc.Children.Add(headline);
                TextBlock report = new TextBlock();
                report.FontFamily = new FontFamily("Times New Roman");
                report.FontSize = 16;
                report.Margin = new Thickness(80, 50, 0, 0);
                report.Text += "День" + "\t\t" + "Кол-во оказанных услуг" + "\n\n";
                foreach (Models.NumOfOrdersOrServices data in num_of_orders_or_services)
                {
                    report.Text += data.day + "\t" + data.count + "\n";
                }
                doc.Children.Add(report);
                PrintDialog print = new PrintDialog();
                if (print.ShowDialog() == true)
                {
                    print.PrintVisual(doc, "Количество заказов по дням за период времени");
                }
            }
        }

        /// <summary>
        /// Формирование отчета в виде графика в зависимости от статуса (типа отчета)
        /// </summary>
        /// <param name="num_of_orders_or_services"></param>
        /// <param name="status"></param>
        public void printGraph(List<Models.NumOfOrdersOrServices> num_of_orders_or_services, int status)
        {
            if (status == 1)
            {
                num_of_orders_or_services.Clear();
                bool dateCheck = false;
                int count = 0;
                string currentDay = "";
                List<DateTime> days = calendar.SelectedDates.ToList();
                foreach (DateTime day in days)
                {
                    string yyyy = day.Year.ToString();
                    string mm = day.Month.ToString();
                    if (mm.Length == 1)
                        mm = "0" + mm;
                    string dd = day.Day.ToString();
                    if (dd.Length == 1)
                        dd = "0" + dd;
                    currentDay = yyyy + "-" + mm + "-" + dd;
                    foreach (order order in skiresortEntities.GetContext().orders)
                    {
                        if (currentDay == order.date_start)
                        {
                            dateCheck = true;
                            for (i = 0; i < order.services.Length; i++)
                            {
                                if (order.services[i] == ' ')
                                {
                                    count++;
                                }
                            }
                            count++;
                        }
                    }
                    if (dateCheck)
                        num_of_orders_or_services.Add(new Models.NumOfOrdersOrServices { day = currentDay, count = count });
                    else
                        num_of_orders_or_services.Add(new Models.NumOfOrdersOrServices { day = currentDay, count = 0 });

                    dateCheck = false;
                    count = 0;
                }
                dgrid_report.ItemsSource = num_of_orders_or_services.ToList();
            }
            else if (status == 2)
            {
                num_of_orders_or_services.Clear();
                bool dateCheck = false;
                int count = 0;
                string currentDay = "";
                string selectedService = cbox_services.SelectedValue.ToString();
                int id_service = 0;
                foreach (service service in skiresortEntities.GetContext().services)
                {
                    if (service.name == selectedService)
                        id_service = service.id;
                }
                List<DateTime> days = calendar.SelectedDates.ToList();
                foreach (DateTime day in days)
                {
                    string yyyy = day.Year.ToString();
                    string mm = day.Month.ToString();
                    if (mm.Length == 1)
                        mm = "0" + mm;
                    string dd = day.Day.ToString();
                    if (dd.Length == 1)
                        dd = "0" + dd;
                    currentDay = yyyy + "-" + mm + "-" + dd;
                    foreach (order order in skiresortEntities.GetContext().orders)
                    {
                        if (currentDay == order.date_start)
                        {
                            dateCheck = true;
                            string[] services_id_array = order.services.Split(new char[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
                            for (i = 0; i < services_id_array.Length; i++)
                            {
                                if (services_id_array[i] == id_service.ToString())
                                {
                                    count++;
                                }
                            }
                        }
                    }
                    if (dateCheck)
                        num_of_orders_or_services.Add(new Models.NumOfOrdersOrServices { day = currentDay, count = count });
                    else
                        num_of_orders_or_services.Add(new Models.NumOfOrdersOrServices { day = currentDay, count = 0 });

                    dateCheck = false;
                    count = 0;
                }
                dgrid_report.ItemsSource = num_of_orders_or_services.ToList();
            }
            else if (status == 3)
            {
                num_of_orders_or_services.Clear();
                bool dateCheck = false;
                int count = 0;
                string currentDay = "";
                List<DateTime> days = calendar.SelectedDates.ToList();
                foreach (DateTime day in days)
                {
                    string yyyy = day.Year.ToString();
                    string mm = day.Month.ToString();
                    if (mm.Length == 1)
                        mm = "0" + mm;
                    string dd = day.Day.ToString();
                    if (dd.Length == 1)
                        dd = "0" + dd;
                    currentDay = yyyy + "-" + mm + "-" + dd;
                    foreach (order order in skiresortEntities.GetContext().orders)
                    {
                        if (currentDay == order.date_start)
                        {
                            dateCheck = true;
                            count++;
                        }
                    }
                    if (dateCheck)
                        num_of_orders_or_services.Add(new Models.NumOfOrdersOrServices { day = currentDay, count = count });
                    else
                        num_of_orders_or_services.Add(new Models.NumOfOrdersOrServices { day = currentDay, count = 0 });

                    dateCheck = false;
                    count = 0;
                }
                dgrid_report.ItemsSource = num_of_orders_or_services.ToList();
            }

            bool checkGraph = true;
            StackPanel graph = new StackPanel();
            graph.Orientation = Orientation.Vertical;
            TextBlock headline = new TextBlock() { FontFamily = new FontFamily("Times New Roman"), FontSize = 17, Margin = new Thickness(50, 30, 0, 0) };
            if (status == 1)
                headline.Text = "Количество оказанных услуг по дням за период времени";
            if (status == 2)
            {
                string selectedService = cbox_services.SelectedValue.ToString();
                headline.Text = "Количество заказов по дням за период времени по услуге:\n";
                headline.Text += selectedService;
            }
            if (status == 3)
                headline.Text = "Количество заказов по дням за период времени";
            headline.FontWeight = FontWeights.Bold;
            graph.Children.Add(headline);
            foreach (Models.NumOfOrdersOrServices data in num_of_orders_or_services)
            {
                Label date;
                if (checkGraph)
                {
                    date = new Label() { Content = data.day + " | " + data.count.ToString(), HorizontalAlignment = HorizontalAlignment.Left, FontFamily = new FontFamily("Times New Roman"), FontSize = 13, Margin = new Thickness(50, 30, 0, 0) };
                    checkGraph = false;
                }
                else
                {
                    date = new Label() { Content = data.day + " | " + data.count.ToString(), HorizontalAlignment = HorizontalAlignment.Left, FontFamily = new FontFamily("Times New Roman"), FontSize = 13, Margin = new Thickness(50, 7, 0, 0) };
                }
                Rectangle graph_elem = new Rectangle() { Width = data.count * 27, Height = 20, Fill = Brushes.Green, Margin = new Thickness(155, -23, 0, 0), HorizontalAlignment = HorizontalAlignment.Left };
                graph.Children.Add(date);
                graph.Children.Add(graph_elem);
            }
            PrintDialog print = new PrintDialog();
            if (print.ShowDialog() == true)
            {
                print.PrintVisual(graph, "Количество оказанных услуг по дням за период времени");
            }
        }

        private void btn_back_Click(object sender, RoutedEventArgs e)
        {
            Hide();
            new UserWin(Login.currentUser).ShowDialog();
        }
    }
}
