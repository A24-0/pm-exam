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
using System.Security.Cryptography;

namespace pm_exam
{
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
        }

        //геренация пароля
        private string GeneratePassword()
        {
            int length = 14;
            const string validChars = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890!@#$%^&*()-_";

            using (RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider())
            {
                byte[] randomBytes = new byte[length];
                rng.GetBytes(randomBytes);

                char[] chars = new char[length];
                for (int i = 0; i < length; i++)
                {
                    chars[i] = validChars[randomBytes[i] % validChars.Length];
                }

                return new string(chars);
            }
        }

        //проверка пароля на надежность
        private string CheckPasswordStrength(string password)
        {
            string resultMessage = "";

            if (password.Length < 8)
                resultMessage += "Минимальная длина пароля - 8\n";

            if (!password.Any(char.IsLower))
                resultMessage += "Должна быть хотя бы одна строчная буква\n";

            if (!password.Any(char.IsUpper))
                resultMessage += "Должна быть хотя бы одна заглавная буква\n";

            if (!password.Any(char.IsDigit))
                resultMessage += "Должна быть хотя бы одна цифра\n";

            if (!password.Any(c => !char.IsLetterOrDigit(c)))
                resultMessage += "Должен быть хотя бы один спец.символ\n";

            if (resultMessage == string.Empty) resultMessage += "Пароль надежный!";

            return resultMessage;
        }

        //
        //static bool SignIn(string userName, string userMail, string phoneNumber, string userPassword)
        //{
        //    using (MWDB_Model db = new MWDB_Model())
        //    {
        //        Users user = db.Users.FirstOrDefault(u => u.PhoneNumber == phoneNumber && u.Username == userName && u.Email == userMail);

        //        if (user != null)
        //        {
        //            byte[] userPasswordBytes = Encoding.UTF8.GetBytes(userPassword);

        //            byte[] salt = Convert.FromBase64String(user.Salt);

        //            byte[] saltedPassword = new byte[salt.Length + userPasswordBytes.Length];
        //            Buffer.BlockCopy(salt, 0, saltedPassword, 0, salt.Length);
        //            Buffer.BlockCopy(userPasswordBytes, 0, saltedPassword, salt.Length, userPasswordBytes.Length);

        //            using (SHA256 sha256 = SHA256.Create())
        //            {
        //                //хеш от соленого пароля
        //                byte[] enteredHash = sha256.ComputeHash(saltedPassword);

        //                //байты хеша в строку
        //                string enteredHashedPassword = Convert.ToBase64String(enteredHash);

        //                //сравниваем полученный хеш с сохраненным хэшем в бд
        //                if (enteredHashedPassword == user.PasswordHash)
        //                {
        //                    Console.WriteLine("Пароль верный. Вход разрешен.");
        //                }
        //                else
        //                {
        //                    Console.WriteLine("Неверный пароль. Вход запрещен.");
        //                }
        //            }

        //            return true;
        //        }
        //        else
        //        {
        //            return false;
        //        }
        //    }
        //}

        //static bool Registration(string userName, string userMail, string phoneNumber, string userPassword)
        //{
        //    if (!String.IsNullOrEmpty(userName) && !String.IsNullOrEmpty(userMail) &&
        //        !String.IsNullOrEmpty(phoneNumber) && !String.IsNullOrEmpty(userPassword))
        //    {
        //        //генерируем случайную соль
        //        byte[] salt = GenerateSalt();

        //        //пароль в байты
        //        byte[] passwordBytes = Encoding.UTF8.GetBytes(userPassword);

        //        //соль с паролем
        //        byte[] saltedPassword = new byte[salt.Length + passwordBytes.Length];
        //        Buffer.BlockCopy(salt, 0, saltedPassword, 0, salt.Length);
        //        Buffer.BlockCopy(passwordBytes, 0, saltedPassword, salt.Length, passwordBytes.Length);

        //        //cоздаем объект хэширования
        //        using (SHA256 sha256 = SHA256.Create())
        //        {
        //            //хеш от соленого пароля
        //            byte[] hash = sha256.ComputeHash(saltedPassword);

        //            // байты хеша в строку
        //            string hashedPassword = Convert.ToBase64String(hash);

        //            using (MWDB_Model db = new MWDB_Model())
        //            {
        //                db.Users.Add(new Users
        //                {
        //                    Username = userName,
        //                    Email = userMail,
        //                    PhoneNumber = phoneNumber,
        //                    PasswordHash = hashedPassword,
        //                    Salt = Convert.ToBase64String(salt)
        //                });
        //                db.SaveChanges();

        //                return true;
        //            }
        //        }
        //    }
        //    else { return false; }
        //}

        //генерация случайной соли
        static byte[] GenerateSalt()
        {
            byte[] salt = new byte[32]; // 32 байта для соли
            using (RandomNumberGenerator rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(salt);
            }
            return salt;
        }
    }
}
