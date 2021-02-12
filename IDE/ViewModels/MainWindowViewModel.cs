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
            set 
            {
                OnPropertyChanged(ref _currentState, value);
                if (value == WindowState.Maximized)
                    Thickness = new Thickness(SystemParameters.WindowResizeBorderThickness.Left +4, SystemParameters.WindowResizeBorderThickness.Top+4, SystemParameters.WindowResizeBorderThickness.Right+4, SystemParameters.WindowResizeBorderThickness.Bottom+4);
                else
                    Thickness = new Thickness(1);                
            }
        }

        private Thickness _thickness = new Thickness(1);

        /// <summary>
        /// Determines the thickness of the window frame depending on the state of the window
        /// </summary>
        public Thickness Thickness
        {
            get { return _thickness; }
            set { OnPropertyChanged(ref _thickness, value); }
        }


        public MainWindowViewModel()
        {
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
