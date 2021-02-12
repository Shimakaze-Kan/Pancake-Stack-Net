using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Input;

namespace IDE.ViewModels
{
    public class FormatWindowViewModel : BaseWindowViewModel
    {
        protected override void Close()
        {
            throw new NotImplementedException();
        }

        protected override void Maximize()
        {
            throw new NotImplementedException();
        }

        protected override void Minimize()
        {
            CurrentState = WindowState.Minimized;
        }

        protected override void Normal()
        {
            throw new NotImplementedException();
        }
    }
}
