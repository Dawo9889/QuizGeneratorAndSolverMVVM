using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using QuizSolution.Model;

namespace QuizSolution.Database
{
    public class QuizRepository
    {
        private QuizContext _context;
        public QuizRepository()
        {
            _context = new QuizContext();
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
