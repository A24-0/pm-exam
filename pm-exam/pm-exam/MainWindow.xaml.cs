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
using System.IO;
using System.Security.Cryptography;
using System.Text.RegularExpressions;

namespace pm_exam
{
    public partial class MainWindow : Window
    {
        private readonly int _userID;
        public MainWindow()
        {
            InitializeComponent();
            _userID = 1;
            DataContext = new MainWindowViewModel(_userID);
        }
        public MainWindow(int userID)
        {
            InitializeComponent();
            _userID = userID;
            DataContext = new MainWindowViewModel(_userID);
        }
    }
}
