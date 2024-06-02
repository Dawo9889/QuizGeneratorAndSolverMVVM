using PierwszePodejscieDoQuizu.Database.Entities;
using PierwszePodejscieDoQuizu.ViewModel.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PierwszePodejscieDoQuizu.ViewModel.Controls
{
    public class QuestionViewModel : BaseViewModel
    {
        public string Content { get; set; }
        public List<AnswerViewModel> Answers { get; set; } = new List<AnswerViewModel>();
    }
}
