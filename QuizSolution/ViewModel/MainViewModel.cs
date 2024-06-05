using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using QuizSolution.Commands;
using System.Windows.Input;
using QuizSolution.Database;
using QuizSolution.Model;
using System.Windows;

namespace QuizSolution.ViewModel
{
    public class MainViewModel : INotifyPropertyChanged
    {
        private QuizRepository _quizRepository;
        private ObservableCollection<QuestionAndAnswers> _questionAndAnswersList;
        private List<Answer> SelectedAnswers { get; set; } = new List<Answer>();
        private ObservableCollection<Quiz> _quizzes;
        private Quiz _selectedQuiz;
        private QuestionAndAnswers _currentQuestionAndAnswers;
        private int _currentQuestionIndex;
        private int Score;
        private int _numberOfQuestions;
        public int QuestionNumber { get; set; }
        public Action OnQuizCompletedOrQuizNotSelected { get; set; }
        public Action BeforeQuizCompleted { get; set; }
        public Action ResetQuiz { get; set; }
        public Action AnswerButtonVisibilityCollapsed { get; set; }
        public Action AnswerButtonVisibility { get; set; }
        public MainViewModel()
        {
            ResetButtonColors();
            _quizRepository = new QuizRepository();
            LoadQuizzes();
            CurrentQuestionForFrontIndex = 1;
        }
        private TimeSpan _elapsedTime;
        public TimeSpan ElapsedTime
        {
            get { return _elapsedTime; }
            set
            {
                _elapsedTime = value;
                OnPropertyChanged(nameof(ElapsedTime));
            }
        }
        public int NumberOfQuestions
        {
            get => _numberOfQuestions;
            set
            {
                _numberOfQuestions = value;
                OnPropertyChanged();
            } 
        }

        public ObservableCollection<QuestionAndAnswers> QuestionAndAnswersList
        {
            get => _questionAndAnswersList;
            set
            {
                _questionAndAnswersList = value;
                OnPropertyChanged();
            }
        }

        public QuestionAndAnswers CurrentQuestionAndAnswers
        {
            get => _currentQuestionAndAnswers;
            set
            {
                _currentQuestionAndAnswers = value;
                OnPropertyChanged();
            }
        }
        public int CurrentQuestionIndex
        {
            get => _currentQuestionIndex;
            set
            {
                _currentQuestionIndex = value;
                OnPropertyChanged();
            }
        }
        public int CurrentQuestionForFrontIndex
        {
            get => _currentQuestionForFrontIndex;
            set
            {
                _currentQuestionForFrontIndex = value;
                OnPropertyChanged();
            }
        }
        private ICommand _selectQuizCommand;

        public ICommand SelectQuizCommand
        {
            get
            {
                if (_selectQuizCommand == null)
                {
                    _selectQuizCommand = new RelayCommand<object>(SelectQuiz);
                    AnswerButtonVisibilityCollapsed?.Invoke();
                }
                return _selectQuizCommand;
            }
        }

        private void SelectQuiz(object parameter)
        {
            if (_selectedQuiz != null)
            {
               Score = 0;
               QuestionNumber = 0;
               CurrentQuestionIndex = 0;
               AnswerButtonVisibility?.Invoke();
               ResetButtonColors();
               LoadQuestions();
               if (_questionAndAnswersList.Count == 1) { BeforeQuizCompleted?.Invoke(); }
            }
            else
            {
                quizNotSelected();
                OnQuizCompletedOrQuizNotSelected?.Invoke();
            }
        }
        public Quiz SelectedQuiz
        {
            get { return _selectedQuiz; }
            set
            {
                _selectedQuiz = value;
            }
        }
        public ObservableCollection<Quiz> Quizzes
        {
            get { return _quizzes; }
            set
            {
                _quizzes = value;
                OnPropertyChanged();
            }
        }
        private void LoadQuizzes()
        {
            if (_quizRepository.TryGetQuizzes(out var quizzes))
            {
                Quizzes = new ObservableCollection<Quiz>(quizzes);
            }
        }
        public ObservableCollection<QuestionAndAnswers> LoadQuestions(Quiz quiz)
        {
            QuestionAndAnswersList = new ObservableCollection<QuestionAndAnswers>();
            foreach (var question in quiz.Questions)
            {
                QuestionAndAnswersList.Add(new QuestionAndAnswers
                {
                    Question = question,
                    Answers = new ObservableCollection<Answer>(question.Answers)
                });
            }
            return QuestionAndAnswersList;
        }
        private void LoadQuestions()
        {
            if (SelectedQuiz != null)
            {
                QuestionAndAnswersList = new ObservableCollection<QuestionAndAnswers>();

                foreach (var question in SelectedQuiz.Questions)
                {
                    QuestionAndAnswersList.Add(new QuestionAndAnswers
                    {
                        Question = question,
                        Answers = new ObservableCollection<Answer>(question.Answers)
                    });
                }
                if (QuestionAndAnswersList.Any())
                {
                    CurrentQuestionIndex = 0;
                    CurrentQuestionAndAnswers = QuestionAndAnswersList[0];
                }
                NumberOfQuestions = QuestionAndAnswersList.Count;
            }
        }
        private ICommand _selectAnswerCommand;
        public ICommand SelectAnswerCommand
        {
            get
            {
                if (_selectAnswerCommand == null)
                {
                    _selectAnswerCommand = new RelayCommand<int>(SelectAnswer);
                }
                return _selectAnswerCommand;
            }
        }
        private void SelectAnswer(int answerId)
        {
            Answer selectedAnswer = CurrentQuestionAndAnswers.Answers.FirstOrDefault(a => a.AnswerId == answerId);

            if (selectedAnswer != null)
            {
                if (SelectedAnswers.Contains(selectedAnswer))
                {
                    SelectedAnswers.Remove(selectedAnswer);
                }
                else
                {
                    SelectedAnswers.Add(selectedAnswer);
                }

                UpdateButtonColors();
            }
        }
        private ICommand _nextQuestionCommand;
        public ICommand NextQuestionCommand
        {
            get
            {
                if (_nextQuestionCommand == null)
                {
                    _nextQuestionCommand = new RelayCommand<object>(NextQuestion);
                }
                return _nextQuestionCommand;
            }
        }
        private ICommand _exitQuizCommand;
        public ICommand ExitQuizCommand
        {
            get
            {
                if (_exitQuizCommand == null)
                {
                    _exitQuizCommand = new RelayCommand<object>(ExitQuiz);
                }
                return _exitQuizCommand;
            }
        }
        public void NextQuestion(object parameter)
        {
            var correctAnswers = CurrentQuestionAndAnswers.Answers.Where(a => a.IsCorrect).ToList();
            bool allCorrectSelected = correctAnswers.All(ca => SelectedAnswers.Contains(ca));
            bool onlyCorrectSelected = SelectedAnswers.All(sa => correctAnswers.Contains(sa));

            if(SelectedAnswers.Count!=0)
            {
                if (allCorrectSelected && onlyCorrectSelected)
                {
                    Score++;
                }

                SelectedAnswers.Clear();
                
                if (CurrentQuestionIndex < _questionAndAnswersList.Count - 1)
                {
                    CurrentQuestionIndex++;
                    CurrentQuestionForFrontIndex++;
                    CurrentQuestionAndAnswers = _questionAndAnswersList[CurrentQuestionIndex];
                    ResetButtonColors();
                    if (CurrentQuestionIndex == _questionAndAnswersList.Count - 1)
                    {
                        BeforeQuizCompleted?.Invoke();
                    }
                }
                else
                {
                    ExitQuiz(1);
                }
            }
            else
            {
                anyAnswerWasClicked();
            }

        }
        private void ExitQuiz(object parameter)
        {
            if (CurrentQuestionIndex < _questionAndAnswersList.Count - 1)
            {
                var Result = MessageBox.Show("Czy jestes pewny, że chcesz zakończyć ten quiz?", "Jesteś pewny?", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (Result == MessageBoxResult.Yes)
                {
                    OnQuizCompletedOrQuizNotSelected?.Invoke();
                    ShowResult();
                    ResetQuiz?.Invoke();
                    AnswerButtonVisibilityCollapsed?.Invoke();
                }
            }
            else
            {
                OnQuizCompletedOrQuizNotSelected?.Invoke();
                ShowResult();
                ResetQuiz?.Invoke();
                AnswerButtonVisibilityCollapsed?.Invoke();
            }
            
        }
        public void ExitQuizWindow()
        {
            OnQuizCompletedOrQuizNotSelected?.Invoke();
            ShowResult();
            ResetQuiz?.Invoke();
            AnswerButtonVisibilityCollapsed?.Invoke();
        }
        private void UpdateButtonColors()
        {
            Button1Color = SelectedAnswers.Contains(CurrentQuestionAndAnswers.Answers[0]) ? "Green" : "LightGray";
            Button2Color = SelectedAnswers.Contains(CurrentQuestionAndAnswers.Answers[1]) ? "Green" : "LightGray";
            Button3Color = SelectedAnswers.Contains(CurrentQuestionAndAnswers.Answers[2]) ? "Green" : "LightGray";
            Button4Color = SelectedAnswers.Contains(CurrentQuestionAndAnswers.Answers[3]) ? "Green" : "LightGray";
        }
        private void ShowResult()
        {
            string message = $"Twój wynik to: {Score}.\nA Twój czas wykonywania testu wynosił: {ElapsedTime.ToString(@"mm\:ss\:ff")}";
;
            MessageBox.Show(message, "Wynik Quizu", MessageBoxButton.OK, MessageBoxImage.Information);
            OnQuizCompletedOrQuizNotSelected?.Invoke();
        }
        private void anyAnswerWasClicked()
        {
            string message ="Musisz zaznaczyć przynajmniej jedną odpowiedź";
;
            MessageBox.Show(message, "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
        }
        private void quizNotSelected()
        {
            string message ="Zaznacz Quiz który chcesz rozwiązać";
;
            MessageBox.Show(message, "Błąd", MessageBoxButton.OK, MessageBoxImage.Exclamation);
        }
        private void ResetButtonColors()
        {
            Button1Color = "LightGray";
            Button2Color = "LightGray";
            Button3Color = "LightGray";
            Button4Color = "LightGray";
        }
        private string _button1Color;
        public string Button1Color
        {
            get { return _button1Color; }
            set { _button1Color = value; OnPropertyChanged(nameof(Button1Color)); }
        }

        private string _button2Color;
        public string Button2Color
        {
            get { return _button2Color; }
            set { _button2Color = value; OnPropertyChanged(nameof(Button2Color)); }
        }

        private string _button3Color;
        public string Button3Color
        {
            get { return _button3Color; }
            set { _button3Color = value; OnPropertyChanged(nameof(Button3Color)); }
        }

        private string _button4Color;
        private int _currentQuestionForFrontIndex;

        public string Button4Color
        {
            get { return _button4Color; }
            set { _button4Color = value; OnPropertyChanged(nameof(Button4Color)); }
        }
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
