using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace pm_exam
{
    public partial class GeneratePasswordView : UserControl
    {
        public GeneratePasswordView()
        {
            InitializeComponent();
        }

        private void GeneratePassword_Click(object sender, RoutedEventArgs e)
        {
            if (!int.TryParse(passwordLengthTextBox.Text, out int length) || (length < 8 || length > 128))
            {
                MessageBox.Show("Поле длина пароля должна быть числом и в области от 8 до 128 знаков");
                passwordLengthTextBox.Text = "8";
            }
            else
            {
                const string uppercaseChars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ";
                const string lowercaseChars = "abcdefghijklmnopqrstuvwxyz";
                const string digitChars = "0123456789";
                const string specialChars = "!@#$%^&*()_+{}[]";

                var includeUppercase = uppercaseCheckBox.IsChecked ?? false;
                var includeLowercase = lowercaseCheckBox.IsChecked ?? false;
                var includeDigits = digitsCheckBox.IsChecked ?? false;
                var includeSpecialChars = specialCharsCheckBox.IsChecked ?? false;
                var passwordLength = int.Parse(passwordLengthTextBox.Text);

                var allowedChars = new StringBuilder();
                if (includeUppercase)
                    allowedChars.Append(uppercaseChars);
                if (includeLowercase)
                    allowedChars.Append(lowercaseChars);
                if (includeDigits)
                    allowedChars.Append(digitChars);
                if (includeSpecialChars)
                    allowedChars.Append(specialChars);

                if (allowedChars.Length == 0)
                {
                    MessageBox.Show("Выберите хотя бы один пункт");
                    return;
                }
                //The minimum number of non-alphanumeric characters - переменная
                var random = new Random();
                var password = new StringBuilder();
                for (int i = 0; i < passwordLength; i++)
                {
                    var index = random.Next(0, allowedChars.Length);
                    password.Append(allowedChars[index]);
                }
                //check 
                //while(percent_of_mininum)//для каждого недостающего
                //{
                //    var rand_index = random.Next(0, passwordLength);
                //    password[rand_index] = specialChars[random.Next(0, specialChars.Length)];
                //}

                generatedPasswordTextBox.Text = password.ToString();
            }
        }
    }
}
