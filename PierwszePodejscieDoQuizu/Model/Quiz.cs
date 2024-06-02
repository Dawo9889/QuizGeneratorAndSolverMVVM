using PierwszePodejscieDoQuizu.Database.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PierwszePodejscieDoQuizu.Model
{
    public class Quiz
    {
        public string Title { get; set; }
        public string Category { get; set; }
        public List<Question> Questions { get; set; }
    }
}
