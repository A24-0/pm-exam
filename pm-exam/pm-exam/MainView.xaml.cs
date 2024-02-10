using System;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.IO;
using System.Linq;
using System.Runtime.Remoting.Contexts;
using System.Security.Cryptography;
using System.Security.Policy;
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
                            Password = item.EncryptedText,
                            Priority = item.Priority
                         })
                         .ToList();

                MainDataGrid.ItemsSource = items;
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
                EncryptedData selectedItem = (EncryptedData)MainDataGrid.SelectedItem;

                using (pm_Model db = new pm_Model())
                {
                    var itemToDelete = db.EncryptedData.FirstOrDefault(item => item.PasswordID == selectedItem.PasswordID);

                    if (itemToDelete != null)
                    {
                        db.EncryptedData.Remove(itemToDelete);
                    }
                }
            }
        }

        private void CopyButton_Click(object sender, RoutedEventArgs e)
        {
            using (pm_Model db = new pm_Model())
            {
                EncryptedData password = (EncryptedData)MainDataGrid.SelectedItem;
                var copyPassword = db.EncryptedData.FirstOrDefault(item => item.PasswordID == password.PasswordID);

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
