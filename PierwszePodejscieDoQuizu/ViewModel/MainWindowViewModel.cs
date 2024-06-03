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



        private Visibility _gridVisibility;
        public Visibility GridVisibility
        {
            get { return _gridVisibility; }
            set
            {
                _gridVisibility = value;
                OnPropertyChanged(nameof(GridVisibility));
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
        private bool _isFieldsEnabled = true;
        public bool IsFieldsEnabled
        {
            get { return _isFieldsEnabled; }
            set
            {
                _isFieldsEnabled = value;
                OnPropertyChanged(nameof(IsFieldsEnabled));
            }
        }

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
            GridVisibility = Visibility.Collapsed; // Początkowa widoczność siatki
        }



        private void AddNewQuestion()
        {
            WarningText = "";
            /*OnPropertyChanged(nameof(WarningText));*/

            var sampleAnswers = new ObservableCollection<AnswerViewModel>
    {
        new AnswerViewModel { Content = NewAnswer0, IsCorrect = IsCorrect0 },
        new AnswerViewModel { Content = NewAnswer1, IsCorrect = IsCorrect1 },
        new AnswerViewModel { Content = NewAnswer2, IsCorrect = IsCorrect2 },
        new AnswerViewModel { Content = NewAnswer3, IsCorrect = IsCorrect3 }
    };
            if (!sampleAnswers.Any(answer => answer.IsCorrect))
            {
                WarningText = "At least one answer must be marked as correct.";
                OnPropertyChanged(nameof(WarningText));
                return;
            }

            var newQuestion = new QuestionViewModel
            {
                Content = NewQuestionContent,
                Answers = sampleAnswers
            };

            Quiz.Questions.Add(newQuestion);
            WarningText = "Question added";
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
                WarningText = "All fields must be filled out.";
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
                WarningText = "Quiz must have at least one question.";
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

            WarningText = "Quiz successfully saved to the database.";
            Quizzes.Clear(); // Resetujemy listę quizów
            OnPropertyChanged(nameof(Quizzes)); // Powiadamiamy o zmianie listy quizów
            ClearQuestionAndAnswers(); // Resetujemy właściwości pytania i odpowiedzi
            GridVisibility = Visibility.Collapsed;
            OnPropertyChanged(nameof(GridVisibility)); // Powiadamiamy o zmianie widoczności siatki
            IsFieldsEnabled = true;

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
