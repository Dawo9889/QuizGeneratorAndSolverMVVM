using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PierwszePodejscieDoQuizu.Model
{
    public class Question
    {
        public string Content { get; set; }

        public List<Answer> Answers { get; set; }
    }
}
