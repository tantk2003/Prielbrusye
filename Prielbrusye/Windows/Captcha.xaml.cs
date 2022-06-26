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
    /// Логика взаимодействия для Captcha.xaml
    /// </summary>
    public partial class Captcha : Window
    {
        private char[] letters_list = new char[] { 'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l', 
            'm', 'n', 'o', 'p', 'q', 'r', 's', 't', 'u', 'v', 'w', 'x', 'y', 'z', 'A', 'B', 'C', 'D', 'E', 
            'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X', 'Y', 'Z' };
        public Random rnd = new Random();
        public int i = 0;
        public Captcha()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Случайный выбор букв, расцветок букв и расцветки линии
        /// </summary>
        public void genetate_captcha()
        {          
            int count_letters = letters_list.Length;
            for (i = 0; i < 5; i++)
            {
                int num_letter = rnd.Next(0, count_letters);
                char letter = letters_list[num_letter];
                Brush brush = new SolidColorBrush(Color.FromRgb((byte)rnd.Next(1, 255), (byte)rnd.Next(1, 255), (byte)rnd.Next(1, 233)));
                switch (i)
                {
                    case 0: captcha_letter1.Content = letter; captcha_letter1.Foreground = brush; break;
                    case 1: captcha_letter2.Content = letter; captcha_letter2.Foreground = brush; break;
                    case 2: captcha_letter3.Content = letter; captcha_letter3.Foreground = brush; break;
                    case 3: captcha_letter4.Content = letter; captcha_letter4.Foreground = brush; break;
                    case 4: captcha_letter5.Content = letter; captcha_letter5.Foreground = brush; break;
                    default: break;
                }
                brush = new SolidColorBrush(Color.FromRgb((byte)rnd.Next(1, 255),
                (byte)rnd.Next(1, 255), (byte)rnd.Next(1, 233)));
                line.Stroke = brush;
            }                    
        }

        private void Window_IsVisibleChanged(object sender, DependencyPropertyChangedEventArgs e)
        {
            genetate_captcha();
        }

        private void btn_reload_captcha_Click(object sender, RoutedEventArgs e)
        {
            genetate_captcha();
        }

        /// <summary>
        /// Проверка введенной капчи на совпадение
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tbox_captcha_KeyUp(object sender, KeyEventArgs e)
        {
            if(e.Key == Key.Enter)
            {
                if(tbox_captcha.Text[0] == (Char)captcha_letter1.Content && tbox_captcha.Text[1] == (Char)captcha_letter2.Content
                    && tbox_captcha.Text[2] == (Char)captcha_letter3.Content && tbox_captcha.Text[3] == (Char)captcha_letter4.Content
                    && tbox_captcha.Text[4] == (Char)captcha_letter5.Content){
                    Hide();
                    new Login(1).ShowDialog();
                }
                else
                {
                    genetate_captcha();
                    tbox_captcha.Text = "";
                }
            }
        }
    }
}
