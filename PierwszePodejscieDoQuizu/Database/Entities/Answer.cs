namespace PierwszePodejscieDoQuizu.Database.Entities
{
    public class Answer
    {
        public int Id { get; set; }
        public string Content { get; set; }
        public bool isCorrect { get; set; }
    }
}