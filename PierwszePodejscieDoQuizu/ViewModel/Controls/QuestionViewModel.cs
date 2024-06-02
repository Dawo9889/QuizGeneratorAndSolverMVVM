using PierwszePodejscieDoQuizu.Database.Entities;
using PierwszePodejscieDoQuizu.ViewModel.Base;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PierwszePodejscieDoQuizu.ViewModel.Controls
{
    public class QuestionViewModel : BaseViewModel
    {
        public string Content { get; set; }
        public ObservableCollection<AnswerViewModel> Answers { get; set; } = new ObservableCollection<AnswerViewModel>();
    }
}
