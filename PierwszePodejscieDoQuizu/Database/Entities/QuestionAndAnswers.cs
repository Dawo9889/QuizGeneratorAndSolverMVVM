
using PierwszePodejscieDoQuizu.Database.Entities;
using System.Collections.ObjectModel;

namespace PierwszePodejscieDoQuizu.Database.Entities
{
    public class QuestionAndAnswers
    {
        public Question Question { get; set; }
        public ObservableCollection<Answer> Answers { get; set; }
    }
}
