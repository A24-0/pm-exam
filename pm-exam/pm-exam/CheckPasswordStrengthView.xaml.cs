using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
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
    public partial class CheckPasswordStrengthView : UserControl
    {
        public CheckPasswordStrengthView()
        {
            InitializeComponent();
        }

        private void CheckPassword_Click(object sender, RoutedEventArgs e)
        {
            resultText.Text = "";

            if (string.IsNullOrEmpty(passwordTextBox.Text))
            {
                resultText.Text = "Введите пароль";
            }

            if (string.IsNullOrEmpty(resultText.Text))
            {
                if (passwordTextBox.Text.Length < 8)
                    resultText.Text += "Пароль слишком короткий\n";

                if (passwordTextBox.Text.Count(char.IsUpper) == 0)
                    resultText.Text += "Пароль должен содержать хотя бы одну заглавную букву\n";

                if (passwordTextBox.Text.Count(char.IsLower) == 0)
                    resultText.Text += "Пароль должен содержать хотя бы одну строчную букву\n";

                if (passwordTextBox.Text.Count(char.IsDigit) == 0)
                    resultText.Text += "Пароль должен содержать хотя бы одну цифру\n";

                if (passwordTextBox.Text.Count(c => !char.IsLetterOrDigit(c)) == 0)
                    resultText.Text += "Пароль должен содержать хотя бы один специальный символ\n";

                if (Regex.IsMatch(passwordTextBox.Text, @"(?:(\d)\1+)|(?:(\p{L})\2+)"))
                    resultText.Text += "Пароль содержит повторяющиеся символы\n";

                // чтобы в строке не шли подряд буквы из алфавита и цифры
                if (ContainsConsecutiveLettersOrDigits(passwordTextBox.Text))
                    resultText.Text += "Пароль содержит ряд последовательных букв/цифр\n";

                if (string.IsNullOrEmpty(resultText.Text)) resultText.Text = "Пароль надежный\n";
            }
        }
        public bool ContainsConsecutiveLettersOrDigits(string input)
        {
            for (int i = 0; i < input.Length - 1; i++)
            {
                char currentChar = input[i];
                char nextChar = input[i + 1];

                // Проверяем, если текущий и следующий символы буквы или цифры
                if ((char.IsLetter(currentChar) && char.IsLetter(nextChar)) ||
                    (char.IsDigit(currentChar) && char.IsDigit(nextChar)))
                {
                    if (currentChar + 1 == nextChar) return true;
                }
            }

            return false;
        }
    }
}
