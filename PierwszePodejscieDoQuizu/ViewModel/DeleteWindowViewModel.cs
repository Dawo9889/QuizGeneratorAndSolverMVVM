using PierwszePodejscieDoQuizu.Database;
using PierwszePodejscieDoQuizu.Database.Entities;
using PierwszePodejscieDoQuizu.ViewModel.Base;
using PropertyChanged;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Input;

namespace PierwszePodejscieDoQuizu.ViewModel
{
    [AddINotifyPropertyChangedInterface]
    public class DeleteWindowViewModel : BaseViewModel
    {
        private string _quizIdHelper;

        public string QuizIdHelper
        {
            get => _quizIdHelper;
            set
            {
                // Użyj wyrażenia regularnego, aby sprawdzić, czy wprowadzony tekst zawiera tylko cyfry
                if (Regex.IsMatch(value, "^[0-9]*$"))
                {
                    _quizIdHelper = value;
                    OnPropertyChanged(nameof(QuizIdHelper));
                }
                else
                {
                    WarningText = "Wprowadz liczbe!";
                }
            }
        }
        private string _warningText;

        public string WarningText
        {
            get { return _warningText; }
            set
            {
                _warningText = value;
                OnPropertyChanged(nameof(WarningText));
            }
        }

        public ObservableCollection<Quiz> Quizzes { get; set; } = new ObservableCollection<Quiz>();
        public ICommand DeleteQuizCommand { get; set; }
        public DeleteWindowViewModel()
        {
            DeleteQuizCommand = new RelayCommand(DeleteQuizFromDatabase);
            GetQuizzesFromDatabase();
        }

        private void DeleteQuizFromDatabase()
        {
           
            int quizIdToDelete;
            if (!int.TryParse(QuizIdHelper, out quizIdToDelete))
            {
                WarningText= "Nieprawidłowy identyfikator quizu.";
                return;
            }
            QuizIdHelper = string.Empty;
            using (var context = new QuizDbContext())
            {
                
                var quizToDelete = context.Quizzes.FirstOrDefault(q => q.QuizId == quizIdToDelete);
                if (quizToDelete == null)
                {
                    WarningText = "Quiz o podanym identyfikatorze nie istnieje.";
                    return;
                }

               
                context.Quizzes.Remove(quizToDelete);
                context.SaveChanges();

                Console.WriteLine($"Quiz o identyfikatorze {quizIdToDelete} został pomyślnie usunięty z bazy danych.");
                
                GetQuizzesFromDatabase();
            }
        }

        public void GetQuizzesFromDatabase()
        {
            Quizzes = new ObservableCollection<Quiz>();
           
            using (var context = new QuizDbContext())
            {
                
                var quizzesFromDb = context.Quizzes.ToList();

                
                foreach (var quiz in quizzesFromDb)
                {
                    
                    context.Entry(quiz)
                           .Collection(q => q.Questions)
                           .Load();

                    foreach (var question in quiz.Questions)
                    {
                        context.Entry(question)
                               .Collection(q => q.Answers)
                               .Load();
                    }

                   
                    Quizzes.Add(quiz);
                }
            }
        }

    }
}
