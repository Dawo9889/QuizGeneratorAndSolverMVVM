using PropertyChanged;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using PierwszePodejscieDoQuizu.Database;
using PierwszePodejscieDoQuizu.Model;
using System.Windows;
using PierwszePodejscieDoQuizu.Commands;
using PierwszePodejscieDoQuizu.Database.Entities;

namespace PierwszePodejscieDoQuizu.ViewModel
{
    public class EditWindowViewModel : INotifyPropertyChanged
    {
        private QuizRepository _quizRepository;
        private ObservableCollection<Database.Entities.QuestionAndAnswers> _questionAndAnswersList;
        private List<Database.Entities.Answer> SelectedAnswers { get; set; } = new List<Database.Entities.Answer>();
        private ObservableCollection<Database.Entities.Quiz> _quizzes;
        private Database.Entities.Quiz _selectedQuiz;
        private Database.Entities.QuestionAndAnswers _currentQuestionAndAnswers;
        private int _currentQuestionIndex;
        private int Score;
        private int _numberOfQuestions;
        private int _currentQuestionForFrontIndex;
        public string questionText { get; set; }
        public string answer1Text { get; set; }
        public string answer2Text { get; set; }
        public string answer3Text { get; set; }
        public string answer4Text { get; set; }
        public int QuestionNumber { get; set; }
        public Action OnQuizCompletedOrQuizNotSelected { get; set; }
        public Action BeforeQuizCompleted { get; set; }
        public Action ResetQuiz { get; set; }
        public Action TextBoxesVisibilityCollapsed { get; set; }
        public Action TextBoxesVisibility { get; set; }
        public Action ShowInformationAboutEmptyTextBoxes { get; set; }
        public Action UnLockButton { get; set; }
        public EditWindowViewModel()
        {
            _quizRepository = new QuizRepository();
            LoadQuizzes();
            CurrentQuestionForFrontIndex = 1;
        }
        public bool AnyNullContentInTextBoxes(string questionContent, List<string> answerContents)
        {
            if (string.IsNullOrEmpty(questionContent))
            {
                return true; 
            }

            foreach (var content in answerContents)
            {
                if (string.IsNullOrEmpty(content))
                {
                    return true;
                }
            }

            return false;
        }

        public ObservableCollection<Database.Entities.QuestionAndAnswers> QuestionAndAnswersList
        {
            get => _questionAndAnswersList;
            set
            {
                _questionAndAnswersList = value;
                OnPropertyChanged();
            }
        }

        public Database.Entities.QuestionAndAnswers CurrentQuestionAndAnswers
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
                    _selectQuizCommand = new RelayCommandForEdit<object>(SelectQuiz);
                    TextBoxesVisibilityCollapsed?.Invoke();
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
               TextBoxesVisibility?.Invoke();
               LoadQuestions();
               if (_questionAndAnswersList.Count == 1) { BeforeQuizCompleted?.Invoke(); }
            }
            else
            {
                quizNotSelected();
                OnQuizCompletedOrQuizNotSelected?.Invoke();
            }
        }
        public Database.Entities.Quiz SelectedQuiz
        {
            get { return _selectedQuiz; }
            set
            {
                _selectedQuiz = value;
            }
        }
        public ObservableCollection<Database.Entities.Quiz> Quizzes
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
                Quizzes = new ObservableCollection<Database.Entities.Quiz>(quizzes);
            }
        }
        public ObservableCollection<Database.Entities.QuestionAndAnswers> LoadQuestions(Database.Entities.Quiz quiz)
        {
            QuestionAndAnswersList = new ObservableCollection<Database.Entities.QuestionAndAnswers>();
            foreach (var question in quiz.Questions)
            {
                QuestionAndAnswersList.Add(new Database.Entities.QuestionAndAnswers
                {
                    Question = question,
                    Answers = new ObservableCollection<Database.Entities.Answer>(question.Answers)
                });
            }
            return QuestionAndAnswersList;
        }
        private void LoadQuestions()
        {
            if (SelectedQuiz != null)
            {
                QuestionAndAnswersList = new ObservableCollection<Database.Entities.QuestionAndAnswers>();

                foreach (var question in SelectedQuiz.Questions)
                {
                    QuestionAndAnswersList.Add(new Database.Entities.QuestionAndAnswers
                    {
                        Question = question,
                        Answers = new ObservableCollection<Database.Entities.Answer>(question.Answers)
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
                    _selectAnswerCommand = new RelayCommandForEdit<int>(SelectAnswer);
                }
                return _selectAnswerCommand;
            }
        }
        private void SelectAnswer(int answerId)
        {
            Database.Entities.Answer selectedAnswer = CurrentQuestionAndAnswers.Answers.FirstOrDefault(a => a.AnswerId == answerId);

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
            }
        }
        private ICommand _nextQuestionCommand;
        public ICommand NextQuestionCommand
        {
            get
            {
                if (_nextQuestionCommand == null)
                {
                    _nextQuestionCommand = new RelayCommandForEdit<object>(NextQuestion);
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
                    _exitQuizCommand = new RelayCommandForEdit<object>(ExitQuiz);
                }
                return _exitQuizCommand;
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

        public void NextQuestion(object parameter)
        {
            if (CurrentQuestionIndex < _questionAndAnswersList.Count - 1)
            {
                CurrentQuestionIndex++;
                CurrentQuestionForFrontIndex++;
                CurrentQuestionAndAnswers = _questionAndAnswersList[CurrentQuestionIndex];
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
        private void ExitQuiz(object parameter)
        {
            if (CurrentQuestionIndex < _questionAndAnswersList.Count - 1)
            {
                var Result = MessageBox.Show("Czy jestes pewny, że chcesz zakończyć edycje quizu?", "Jesteś pewny?", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (Result == MessageBoxResult.Yes)
                {
                    OnQuizCompletedOrQuizNotSelected?.Invoke();
                    ResetQuiz?.Invoke();
                    TextBoxesVisibilityCollapsed?.Invoke();
                }
            }
            else
            {
                OnQuizCompletedOrQuizNotSelected?.Invoke();
                ResetQuiz?.Invoke();
                TextBoxesVisibilityCollapsed?.Invoke();
            }
            UpdateQuestionsAndAnswersInDatabase();

        }
        public void UpdateQuestionsAndAnswersInDatabase()
        {
            if (QuestionAndAnswersList != null && QuestionAndAnswersList.Any())
            {
                _quizRepository.UpdateQuestionsAndAnswers(QuestionAndAnswersList);
            }
        }
        private void quizNotSelected()
        {
            string message ="Zaznacz Quiz który chcesz zedytować";
;
            MessageBox.Show(message, "Błąd", MessageBoxButton.OK, MessageBoxImage.Exclamation);
        }
        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
