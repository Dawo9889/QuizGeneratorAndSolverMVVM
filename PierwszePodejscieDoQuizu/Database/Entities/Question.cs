namespace PierwszePodejscieDoQuizu.Database.Entities
{
    public class Question
    {
        public int QuestionId { get; set; }
        public string Content { get; set; }
        public int QuizId { get; set; }
        public Quiz Quiz { get; set; }
        public List<Answer> Answers { get; set; } = new List<Answer>();
    }
}