using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pm_exam
{
    class MainWindowViewModel : ObservableObject
    {
        public RelayCommand MainViewCommand { get; set; }
        public RelayCommand GeneratePasswordViewCommand { get; set; }
        public RelayCommand CheckPasswordStrengthViewCommand { get; set; }

        public MainView MV { get; set; }
        public CheckPasswordStrengthView CPSV { get; set; }
        public GeneratePasswordView GPV { get; set; }

        private object _currentView;
        public object CurrentView
        {
            get { return _currentView; }
            set
            {
                _currentView = value;
                OnPropertyChanged();
            }
        }

        public MainWindowViewModel(int UserID)
        {
            MV = new MainView(UserID);
            CPSV = new CheckPasswordStrengthView();
            GPV = new GeneratePasswordView();

            CurrentView = MV;

            MainViewCommand = new RelayCommand(o =>
            {
                CurrentView = MV;
            });

            CheckPasswordStrengthViewCommand = new RelayCommand(o =>
            {
                CurrentView = CPSV;
            });

            GeneratePasswordViewCommand = new RelayCommand(o =>
            {
                CurrentView = GPV;
            });
        }
    }
}
