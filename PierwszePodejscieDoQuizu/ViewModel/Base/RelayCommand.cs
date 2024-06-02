using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace PierwszePodejscieDoQuizu.ViewModel.Base
{
    public class RelayCommand : ICommand
    {
        private Action mAction;
        private ICommand? addNewQuestionCommand;

        public RelayCommand(Action action)
        {
            mAction = action;
        }

        public RelayCommand(ICommand? addNewQuestionCommand)
        {
            this.addNewQuestionCommand = addNewQuestionCommand;
        }

        public event EventHandler? CanExecuteChanged;

        public bool CanExecute(object? parameter)
        {
            return true;
        }

        public void Execute(object? parameter)
        {
            mAction();
        }
    }
}
