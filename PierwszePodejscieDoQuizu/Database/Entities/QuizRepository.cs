using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PierwszePodejscieDoQuizu.Database.Entities
{
    public class QuizRepository
    {
        private QuizDbContext _context;
        public QuizRepository()
        {
            _context = new QuizDbContext();
            _context.Database.EnsureCreated();
        }

        public List<Quiz> GetQuizzes()
        {
            return _context.Quizzes
                .Include(q => q.Questions)
                .ThenInclude(q => q.Answers)
                .ToList();
        }
        public bool TryGetQuizzes(out List<Quiz> quizzes)
        {
            try
            {
                quizzes = GetQuizzes();
                return quizzes.Count > 0;
            }
            catch
            {
                quizzes = null;
                return false;
            }
        }
    }
}
