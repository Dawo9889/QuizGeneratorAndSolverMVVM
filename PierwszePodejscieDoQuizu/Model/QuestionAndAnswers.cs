using PierwszePodejscieDoQuizu.Model;
using System.Collections.ObjectModel;

namespace PierwszePodejscieDoQuizu.Model
{
    public class QuestionAndAnswers
    {
        public Question Question { get; set; }
        public ObservableCollection<Answer> Answers { get; set; }
    }
}
