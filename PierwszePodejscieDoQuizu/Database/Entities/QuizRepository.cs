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
        public void UpdateQuestionsAndAnswers(IEnumerable<QuestionAndAnswers> questionAndAnswersList)
        {
            foreach (var qa in questionAndAnswersList)
            {
                var questionInDb = _context.Questions.FirstOrDefault(q => q.QuestionId == qa.Question.QuestionId);
                if (questionInDb != null)
                {
                    questionInDb.Content = qa.Question.Content;
                    foreach (var answer in qa.Answers)
                    {
                        var answerInDb = _context.Answers.FirstOrDefault(a => a.AnswerId == answer.AnswerId);
                        if (answerInDb != null)
                        {
                            answerInDb.Content = answer.Content;
                            answerInDb.IsCorrect = answer.IsCorrect;
                        }
                    }
                }
            }
            _context.SaveChanges();
        }
    }
}
