using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pm_exam
{
    class SignInOrRegistrationViewModel : ObservableObject
    {
        public RelayCommand SignInViewCommand { get; set; }
        public RelayCommand RegistrationViewCommand { get; set; }

        public SignIn SignIn_ { get; set; }
        public Registration Registration_ { get; set; }

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

        public SignInOrRegistrationViewModel()
        {
            SignIn_ = new SignIn();
            Registration_ = new Registration();

            CurrentView = SignIn_;

            SignInViewCommand = new RelayCommand(o =>
            {
                CurrentView = SignIn_;
            });

            RegistrationViewCommand = new RelayCommand(o =>
            {
                CurrentView = Registration_;
            });
        }
    }
}
