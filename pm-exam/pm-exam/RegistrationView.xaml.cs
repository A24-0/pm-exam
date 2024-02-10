using System;
using System.Collections.Generic;
using System.IO;
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
    public partial class RegistrationView : UserControl
    {
        public RegistrationView()
        {
            InitializeComponent();
        }

        private void RegisterClick(object sender, RoutedEventArgs e)
        {
            if (!String.IsNullOrEmpty(userName.Text.ToString()) && !String.IsNullOrEmpty(email.Text.ToString()) &&
                !String.IsNullOrEmpty(phoneNumber.Text.ToString()) && !String.IsNullOrEmpty(userPassword.Text.ToString()))
            {

                byte[] key = GenerateRandomKey();
                byte[] iv = GenerateRandomIV();

                byte[] encryptedBytes = EncryptAES(userPassword.Text.ToString(), key, iv);

                using (pm_Model db = new pm_Model())
                {
                    var newUser = new Users
                    {
                        Username = userName.Text.ToString(),
                        Email = email.Text.ToString(),
                        PhoneNumber = phoneNumber.Text.ToString(),
                        EncryptedText = encryptedBytes,
                        PasswordKey = key,
                        IV = iv
                    };
                    db.Users.Add(newUser);
                    db.SaveChanges();

                    MainWindow mw = new MainWindow(newUser.UserID);
                    mw.Show();
                    Window.GetWindow(this).Close();
                }
            }
        }

        static byte[] EncryptAES(string plainText, byte[] key, byte[] iv)
        {
            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.Key = key;
                aesAlg.IV = iv;

                ICryptoTransform encryptor = aesAlg.CreateEncryptor(aesAlg.Key, aesAlg.IV);

                using (MemoryStream msEncrypt = new MemoryStream())
                {
                    using (CryptoStream csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                    {
                        using (StreamWriter swEncrypt = new StreamWriter(csEncrypt))
                        {
                            swEncrypt.Write(plainText);
                        }
                    }

                    return msEncrypt.ToArray();
                }
            }
        }

        static byte[] GenerateRandomKey()
        {
            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.GenerateKey();
                return aesAlg.Key;
            }
        }

        static byte[] GenerateRandomIV()
        {
            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.GenerateIV();
                return aesAlg.IV;
            }
        }
    }
}
