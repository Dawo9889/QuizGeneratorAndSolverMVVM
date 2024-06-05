using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PierwszePodejscieDoQuizu.Database.Entities
{
    public class Quiz
    {
        public int QuizId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Category { get; set; }
        public List<Question> Questions { get; set; } = new List<Question>();
    }
}
