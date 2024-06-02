using PierwszePodejscieDoQuizu.ViewModel.Base;
using PierwszePodejscieDoQuizu.ViewModel.Controls;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace PierwszePodejscieDoQuizu.ViewModel
{
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


        public string WarningText { get; set; }


        public ICommand AddNewQuizCommand { get; set; }
        public ICommand AddNewQuestionCommand { get; set; }
        public ICommand SaveToDatabaseCommand { get; set; }


        public MainWindowViewModel()
        {
            AddNewQuizCommand = new RelayCommand(AddNewQuiz);
            AddNewQuestionCommand = new RelayCommand(AddNewQuestion);
            SaveToDatabaseCommand = new RelayCommand(SaveToDatabase);

            Quiz = new QuizViewModel();
            Quizzes = new ObservableCollection<QuizViewModel>();
            Questions = new ObservableCollection<QuestionViewModel>();
        }

        private void SaveToDatabase()
        {
            throw new NotImplementedException();
        }

        private void AddNewQuestion()
        {
            WarningText = "";
            OnPropertyChanged(nameof(WarningText));

            var sampleAnswers = new List<AnswerViewModel>
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

            
            OnPropertyChanged(nameof(Quiz.Questions));
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
               
            };
        }
    }
}
