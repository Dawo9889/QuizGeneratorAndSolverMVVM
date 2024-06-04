using PierwszePodejscieDoQuizu.Database;
using PierwszePodejscieDoQuizu.Database.Entities;
using PierwszePodejscieDoQuizu.ViewModel.Base;
using PierwszePodejscieDoQuizu.ViewModel.Controls;
using PropertyChanged;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Input;

namespace PierwszePodejscieDoQuizu.ViewModel
{
    [AddINotifyPropertyChangedInterface]
    public class MainWindowViewModel : Base.BaseViewModel
    {
        public ObservableCollection<QuizViewModel> Quizzes { get; set; }
        public ObservableCollection<QuestionViewModel> Questions { get; set; }

        public QuizViewModel Quiz { get; set; }
        public string NewQuizTitle { get; set; }
        public string NewQuizDescription { get; set; }
        public string NewQuizCategory { get; set; }
        public string NewQuestionContent { get; set; }
        public string NewAnswer0 { get; set; }
        public string NewAnswer1 { get; set; }
        public string NewAnswer2 { get; set; }
        public string NewAnswer3 { get; set; }

        public bool IsCorrect0 { get; set; }
        public bool IsCorrect1 { get; set; }
        public bool IsCorrect2 { get; set; }
        public bool IsCorrect3 { get; set; }
        public bool IsCorrect4 { get; set; }
        public bool IsFieldsEnabled { get; set; } = true;
        public Visibility GridVisibility { get; set; }
        public string WarningText { get; set; }
      
        public ICommand AddNewQuizCommand { get; set; }
        public ICommand AddNewQuestionCommand { get; set; }
        public ICommand SaveToDatabaseCommand { get; set; }
        public ICommand ToggleGridVisibilityCommand { get; set; }


        public MainWindowViewModel()
        {
            AddNewQuizCommand = new RelayCommand(AddNewQuiz);
            AddNewQuestionCommand = new RelayCommand(AddNewQuestion);
            SaveToDatabaseCommand = new RelayCommand(SaveToDatabase);

            Quiz = new QuizViewModel();
            Quizzes = new ObservableCollection<QuizViewModel>();
            Questions = new ObservableCollection<QuestionViewModel>();

            ToggleGridVisibilityCommand = new RelayCommand(ToggleGridVisibility);
            GridVisibility = Visibility.Collapsed; 
        }



        private void AddNewQuestion()
        {
            WarningText = "";

            // Sprawdzenie, czy wszystkie pola odpowiedzi są wypełnione
            if (string.IsNullOrWhiteSpace(NewAnswer0) || string.IsNullOrWhiteSpace(NewAnswer1) ||
                string.IsNullOrWhiteSpace(NewAnswer2) || string.IsNullOrWhiteSpace(NewAnswer3))
            {
                WarningText = "Pytanie musi posiadać 4 odpowiedzi!";
                OnPropertyChanged(nameof(WarningText));
                return;
            }

            var sampleAnswers = new ObservableCollection<AnswerViewModel>
    {
        new AnswerViewModel { Content = NewAnswer0, IsCorrect = IsCorrect0 },
        new AnswerViewModel { Content = NewAnswer1, IsCorrect = IsCorrect1 },
        new AnswerViewModel { Content = NewAnswer2, IsCorrect = IsCorrect2 },
        new AnswerViewModel { Content = NewAnswer3, IsCorrect = IsCorrect3 }
    };

    
            if (!sampleAnswers.Any(answer => answer.IsCorrect))
            {
                WarningText = "Przynajmniej jedna odpowiedz musi być oznaczona jako poprawna!";
                OnPropertyChanged(nameof(WarningText));
                return;
            }

            var newQuestion = new QuestionViewModel
            {
                Content = NewQuestionContent,
                Answers = sampleAnswers
            };

            Quiz.Questions.Add(newQuestion);
            WarningText = "Pytanie dodane";
            OnPropertyChanged(nameof(WarningText));
            OnPropertyChanged(nameof(Quiz.Questions));
            ClearQuestionAndAnswers();
        }

        private void AddNewQuiz()
        {
            WarningText = "";
            OnPropertyChanged(nameof(WarningText));
            if (string.IsNullOrWhiteSpace(NewQuizTitle) ||
                string.IsNullOrWhiteSpace(NewQuizDescription) ||
                string.IsNullOrWhiteSpace(NewQuizCategory))
            {
                WarningText = "Wszystkie pola musza być wypełnione!";
                OnPropertyChanged(nameof(WarningText));
                return;
            }
            Quiz = new QuizViewModel
            {
                Title = NewQuizTitle,
                Description = NewQuizDescription,
                Category = NewQuizCategory,
                Questions = new ObservableCollection<QuestionViewModel>()
            };

            Quizzes.Add(Quiz);
            OnPropertyChanged(nameof(Quizzes));
            GridVisibility = Visibility.Visible;
            IsFieldsEnabled = false;
        }




        private void SaveToDatabase()
        {
            if (Quiz == null || !Quiz.Questions.Any())
            {
                WarningText = "Quiz musi mieć przynajmniej jedno pytanie.";
                return;
            }

            using (var context = new QuizDbContext())
            {
                var quiz = new Quiz
                {
                    Title = Quiz.Title,
                    Description = Quiz.Description,
                    Category = Quiz.Category,
                    Questions = Quiz.Questions.Select(q => new Question
                    {
                        Content = q.Content,
                        Answers = q.Answers.Select(a => new Answer
                        {
                            Content = a.Content,
                            IsCorrect = a.IsCorrect
                        }).ToList()
                    }).ToList()
                };

                context.Quizzes.Add(quiz);
                context.SaveChanges();
            }

            WarningText = "Pomyślnie zapisano quiz do bazy danych";
            Quizzes.Clear(); 
            OnPropertyChanged(nameof(Quizzes)); 
            ClearQuestionAndAnswers(); 
            GridVisibility = Visibility.Collapsed;
            OnPropertyChanged(nameof(GridVisibility)); 
            IsFieldsEnabled = true;
            ClearQuizInputs();


        }

        private void ToggleGridVisibility()
        {
            if (GridVisibility == Visibility.Visible)
            {
                GridVisibility = Visibility.Collapsed;
            }
            else
            {
                GridVisibility = Visibility.Visible;
            }
        }
        public void ClearQuizInputs()
        {
            NewQuizCategory = string.Empty;
            NewQuizTitle = string.Empty;
            NewQuizDescription = string.Empty;
        }
        public void ClearQuestionAndAnswers()
        {
            NewQuestionContent = string.Empty;
            NewAnswer0 = string.Empty;
            IsCorrect0 = false;
            IsCorrect1 = false;
            IsCorrect2 = false;
            IsCorrect3 = false;
            NewAnswer1 = string.Empty;
            NewAnswer2 = string.Empty;
            NewAnswer3 = string.Empty;
        }

    }
}
