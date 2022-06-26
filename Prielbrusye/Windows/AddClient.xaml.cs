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
    /// Логика взаимодействия для AddClient.xaml
    /// </summary>
    public partial class AddClient : Window
    {
        public client new_client = new client();
        public int i = 0;
        public AddClient()
        {
            InitializeComponent();
            DataContext = new_client;
        }

        /// <summary>
        /// Является ли символ числом?
        /// </summary>
        /// <param name="ch"></param>
        /// <returns></returns>
        public bool isDigit(char ch)
        {
            if(ch == '0' || ch == '1' || ch == '2' || ch == '3' 
                || ch == '4' || ch == '5' || ch == '6' || ch == '7' 
                || ch == '8' || ch == '9')
                return true;
            else 
                return false;
        }

        /// <summary>
        /// Добавление нового клиента в БД
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btn_add_client_Click(object sender, RoutedEventArgs e)
        {
            StringBuilder errors = new StringBuilder();
            int ident = 0;
            bool checkPassport = true;

            if (string.IsNullOrWhiteSpace(new_client.snp))
                errors.AppendLine("Вы не ввели ФИО");
            if (string.IsNullOrWhiteSpace(new_client.birthday.ToString()))
                errors.AppendLine("Вы не выбрали дату рождения");
            if (string.IsNullOrWhiteSpace(new_client.email))
                errors.AppendLine("Вы не ввели эл. почту");
            if (string.IsNullOrWhiteSpace(new_client.phone))
                errors.AppendLine("Вы не ввели номер телефона");
            if (string.IsNullOrWhiteSpace(new_client.passport))
                errors.AppendLine("Вы не ввели паспортные данные");

            if (!(string.IsNullOrWhiteSpace(new_client.snp)))
            {
                for (i = 0; i < new_client.snp.Length; i++)
                {
                    if (new_client.snp[i] == ' ' && new_client.snp[i + 1] != ' ')
                        ident++;
                }
                if (ident != 2)
                    errors.AppendLine("Вы ввели ФИО некорректно");
            }

            if (!(string.IsNullOrWhiteSpace(new_client.passport)))
            {
                if (new_client.passport.Length == 11)
                {
                    for (i = 0; i < 4; i++)
                    {
                        if (!(isDigit(new_client.passport[i])))
                            checkPassport = false;
                    }
                    if (new_client.passport[4] != ' ')
                        checkPassport = false;
                    for (i = 5; i < 11; i++)
                    {
                        if (!(isDigit(new_client.passport[i])))
                            checkPassport = false;
                    }
                    if (!checkPassport)
                        errors.AppendLine("Вы ввели паспортные данные некорректно");
                }
                else
                    errors.AppendLine("Вы ввели паспортные данные некорректно");
            }

            if(errors.Length > 0)
            {
                MessageBox.Show(errors.ToString(), "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            
            skiresortEntities.GetContext().clients.Add(new_client);

            try
            {
                skiresortEntities.GetContext().SaveChanges();
                MessageBox.Show("Клиент добавлен", "Успешно", MessageBoxButton.OK, MessageBoxImage.None);
                Hide();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message.ToString(), "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
