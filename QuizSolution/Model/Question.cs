namespace QuizSolution.Model
{
    public class Question
    {
        public int QuestionId { get; set; }
        public string Content { get; set; }
        public List<Answer> Answers { get; set; } = new List<Answer>();

        public int QuizId { get; set; }
        public Quiz Quiz { get; set; }
    }
}