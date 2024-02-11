using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Net;
using System.Runtime.Remoting.Contexts;
using System.Security;
using System.Security.Cryptography;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using System.Web.Security;
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
    public static class PasswordBoxHelper
    {
        public static readonly DependencyProperty PasswordProperty =
            DependencyProperty.RegisterAttached("Password",
            typeof(string), typeof(PasswordBoxHelper),
            new FrameworkPropertyMetadata(string.Empty, OnPasswordPropertyChanged));

        public static readonly DependencyProperty AttachProperty =
            DependencyProperty.RegisterAttached("Attach",
            typeof(bool), typeof(PasswordBoxHelper), new PropertyMetadata(false, Attach));

        private static readonly DependencyProperty IsUpdatingProperty =
            DependencyProperty.RegisterAttached("IsUpdating", typeof(bool), typeof(PasswordBoxHelper));

        public static string GetPassword(DependencyObject dp)
        {
            return (string)dp.GetValue(PasswordProperty);
        }

        public static void SetPassword(DependencyObject dp, string value)
        {
            dp.SetValue(PasswordProperty, value);
        }

        public static bool GetAttach(DependencyObject dp)
        {
            return (bool)dp.GetValue(AttachProperty);
        }

        public static void SetAttach(DependencyObject dp, bool value)
        {
            dp.SetValue(AttachProperty, value);
        }

        private static bool GetIsUpdating(DependencyObject dp)
        {
            return (bool)dp.GetValue(IsUpdatingProperty);
        }

        private static void SetIsUpdating(DependencyObject dp, bool value)
        {
            dp.SetValue(IsUpdatingProperty, value);
        }

        private static void OnPasswordPropertyChanged(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            PasswordBox passwordBox = sender as PasswordBox;
            passwordBox.PasswordChanged -= PasswordChanged;

            if (!(bool)GetIsUpdating(passwordBox))
            {
                passwordBox.Password = (string)e.NewValue;
            }
            passwordBox.PasswordChanged += PasswordChanged;
        }

        private static void Attach(DependencyObject sender, DependencyPropertyChangedEventArgs e)
        {
            PasswordBox passwordBox = sender as PasswordBox;

            if (passwordBox == null)
                return;

            if ((bool)e.OldValue)
            {
                passwordBox.PasswordChanged -= PasswordChanged;
            }

            if ((bool)e.NewValue)
            {
                passwordBox.PasswordChanged += PasswordChanged;
            }
        }

        private static void PasswordChanged(object sender, RoutedEventArgs e)
        {
            PasswordBox passwordBox = sender as PasswordBox;
            SetIsUpdating(passwordBox, true);
            SetPassword(passwordBox, passwordBox.Password);
            SetIsUpdating(passwordBox, false);
        }
    }
    public partial class MainView : UserControl
    {
        private readonly int _userID;
        public MainView(int userID)
        {
            InitializeComponent();
            _userID = userID;
            LoadData();
        }

        private void LoadData()
        {
            using (pm_Model db = new pm_Model())
            {
                var items = db.EncryptedData.Where(item => item.PasswordUserID == _userID)
                         .Select(item => new
                         {
                            ServiceName = item.ServiceName,
                            EncryptedText = item.EncryptedText,
                            Priority = item.Priority,
                            PasswordKey = item.PasswordKey,
                            IV = item.IV
                         })
                         .ToList();
                List<OutputData> outputData = new List<OutputData>();
                
                foreach(var item in items )
                {   
                    int passwordLength = new Random().Next(8, 127);
                    outputData.Add(new OutputData { ServiceName = item.ServiceName,
                        Password = Membership.GeneratePassword(passwordLength, new Random().Next(1, passwordLength)),
                        SecretPassword = new NetworkCredential("", DecryptAES(item.EncryptedText, item.PasswordKey, item.IV)).SecurePassword,
                        Priority = item.Priority
                    });
                }
                MainDataGrid.ItemsSource = outputData;
            }
        }

        private void AddButton_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(NameTextBox.Text)
                && !string.IsNullOrEmpty(PasswordTextBox.Text))
            {
                using (pm_Model db = new pm_Model())
                {
                    byte[] key = GenerateRandomKey();
                    byte[] iv = GenerateRandomIV();

                    byte[] encryptedBytes = EncryptAES(PasswordTextBox.Text.ToString(), key, iv);
                    var newPassword = new EncryptedData
                    {
                        PasswordUserID = _userID,
                        ServiceName = NameTextBox.Text.ToString(),
                        EncryptedText = encryptedBytes,
                        PasswordKey = key,
                        IV = iv,
                        Priority = Convert.ToInt32(PriorityComboBox.Text.ToString())
                    };
                    db.EncryptedData.Add(newPassword);
                    db.SaveChanges();
                    LoadData();
                }
            }
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            if (MainDataGrid.SelectedItem != null)
            {
                OutputData selectedItem = (OutputData)MainDataGrid.SelectedItem;

                using (pm_Model db = new pm_Model())
                {
                    Expression<Func<EncryptedData, bool>> ex = item => DecryptAES(item.EncryptedText, item.PasswordKey, item.IV)
                    == new NetworkCredential("", selectedItem.SecretPassword).Password;
                    var itemToDelete = db.EncryptedData.Where(ex.Compile()).FirstOrDefault();

                    if (itemToDelete != null)
                    {
                        db.EncryptedData.Remove(itemToDelete);
                        db.SaveChanges();
                        LoadData();
                    }
                    
                }
            }
        }

        private void CopyButton_Click(object sender, RoutedEventArgs e)
        {
            OutputData selectedItem = (OutputData)MainDataGrid.SelectedItem;
            using (pm_Model db = new pm_Model())
            {
                //Expression<Func<EncryptedData, bool>> ex = item => DecryptAES(item.EncryptedText, item.PasswordKey, item.IV) 
                //== DecryptAES(selectedItem.EncryptedText, item.PasswordKey, item.IV);
                Expression<Func<EncryptedData, bool>> ex = item => DecryptAES(item.EncryptedText, item.PasswordKey, item.IV)
                == new NetworkCredential("", selectedItem.SecretPassword).Password;

                var copyPassword = db.EncryptedData.Where(ex.Compile()).FirstOrDefault();

                if (copyPassword != null)
                {
                    Clipboard.SetText(DecryptAES(copyPassword.EncryptedText, copyPassword.PasswordKey, copyPassword.IV));
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
        static string DecryptAES(byte[] cipherText, byte[] key, byte[] iv)
        {
            using (Aes aesAlg = Aes.Create())
            {
                aesAlg.Key = key;
                aesAlg.IV = iv;

                ICryptoTransform decryptor = aesAlg.CreateDecryptor(aesAlg.Key, aesAlg.IV);

                using (MemoryStream msDecrypt = new MemoryStream(cipherText))
                {
                    using (CryptoStream csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read))
                    {
                        using (StreamReader srDecrypt = new StreamReader(csDecrypt))
                        {
                             return srDecrypt.ReadToEnd();
                        }
                    }
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
