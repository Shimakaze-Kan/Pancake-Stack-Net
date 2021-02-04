using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace IDE.ViewModels
{
    public class MainWindowViewModel : ObservableObject
    {
        public ICommand MinimizeCommand { get; }       
        public ICommand MaximizeCommand { get; }       
        public ICommand NormalCommand { get; }
        public ICommand CloseCommand { get; }

        private WindowState _currentState;

        public WindowState CurrentState
        {
            get { return _currentState; }
            set { OnPropertyChanged(ref _currentState, value); }
        }

        public MainWindowViewModel()
        {
            //CurrentState = WindowState.Maximized;

            MinimizeCommand = new RelayCommand(Minimize);
            MaximizeCommand = new RelayCommand(Maximize);
            NormalCommand = new RelayCommand(Normal);
            CloseCommand = new RelayCommand(Close);
        }

        private void Minimize()
        {
            CurrentState = WindowState.Minimized;
        }

        private void Maximize()
        {
            CurrentState = WindowState.Maximized;
        }

        private void Normal()
        {
            CurrentState = WindowState.Normal;
        }

        private void Close()
        {
            Application.Current.Shutdown();
        }
    }
}
