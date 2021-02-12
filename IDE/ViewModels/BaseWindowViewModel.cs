using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace IDE.ViewModels
{
    public abstract class BaseWindowViewModel : ObservableObject
    {
        public ICommand MinimizeCommand { get; }
        public ICommand MaximizeCommand { get; }
        public ICommand NormalCommand { get; }
        public ICommand CloseCommand { get; }

        protected WindowState _currentState;

        public WindowState CurrentState
        {
            get { return _currentState; }
            set { OnPropertyChanged(ref _currentState, value); }
        }

        protected BaseWindowViewModel()
        {
            MinimizeCommand = new RelayCommand(Minimize);
            MaximizeCommand = new RelayCommand(Maximize);
            NormalCommand = new RelayCommand(Normal);
            CloseCommand = new RelayCommand(Close);
        }

        protected abstract void Minimize();

        protected abstract void Maximize();

        protected abstract void Normal();

        protected abstract void Close();
    }
}
