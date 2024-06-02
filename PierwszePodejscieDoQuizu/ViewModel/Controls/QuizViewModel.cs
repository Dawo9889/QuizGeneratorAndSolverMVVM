using PierwszePodejscieDoQuizu.ViewModel.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PierwszePodejscieDoQuizu.ViewModel.Controls
{
    public class QuizViewModel : BaseViewModel
    {
        public string Title { get; set; }
        public string Description { get; set; }
        public string Category { get; set; }
        public List<QuestionViewModel> Questions { get; set; } = new List<QuestionViewModel>();
    }
}
