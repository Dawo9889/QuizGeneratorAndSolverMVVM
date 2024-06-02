using PierwszePodejscieDoQuizu.Database;
using PierwszePodejscieDoQuizu.Database.Entities;
using System.Configuration;
using System.Data;
using System.Windows;

namespace PierwszePodejscieDoQuizu
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            var database = new QuizDbContext();
            database.Database.EnsureCreated();

            // Check if there are any quizzes already in the database
            if (database.Quizzes.Any())
            {
                return;   // Database has been seeded
            }

            var quizzes = new List<Quiz>
        {
            new Quiz
            {
                Title = "General Knowledge",
                Questions = new List<Question>
                {
                    new Question
                    {
                        Content = "What is the capital of France?",
                        Answers = new List<Answer>
                        {
                            new Answer { Content = "Paris", isCorrect = true },
                            new Answer { Content = "London",isCorrect = false },
                            new Answer { Content = "Berlin",isCorrect = false }
                        }
                    },
                     new Question
                    {
                        Content = "What is the capital of Poland?",
                        Answers = new List<Answer>
                        {
                            new Answer { Content = "Warsaw", isCorrect = true },
                            new Answer { Content = "London",isCorrect = false },
                            new Answer { Content = "Berlin",isCorrect = false }
                        }
                    },
                }
            }
        };

            database.Quizzes.AddRange(quizzes);
            database.SaveChanges();
        }
    }
    
}
