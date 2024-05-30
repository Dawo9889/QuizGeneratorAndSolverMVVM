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
        [Key]
        public int Id { get; set; }

        public string Title { get; set; }
        public List<Question> Questions { get; set; }
    }
}
