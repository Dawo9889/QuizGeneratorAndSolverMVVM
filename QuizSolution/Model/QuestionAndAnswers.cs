using System.Collections.ObjectModel;

namespace QuizSolution.Model
{
    public class QuestionAndAnswers
    {
        public Question Question { get; set; }
        public ObservableCollection<Answer> Answers { get; set; }
    }
}
