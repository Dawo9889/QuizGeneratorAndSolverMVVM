using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Threading;
using QuizSolution.ViewModel;

namespace QuizSolution
{
    public partial class MainWindow : Window
    {
        private DispatcherTimer _dispatcherTimer;
        private DateTime _startTime;
        private TimeSpan _elapsedTime;
        private MainViewModel _viewModel;
        public MainWindow()
        {
            InitializeComponent();
            _viewModel = new MainViewModel();
            DataContext = _viewModel;

            ((MainViewModel)DataContext) .OnQuizCompletedOrQuizNotSelected += OnQuizCompletedOrQuizNotSelected;
            ((MainViewModel)DataContext).BeforeQuizCompleted += BeforeQuizCompleted;
            ((MainViewModel)DataContext).ResetQuiz += ResetQuiz;
            ((MainViewModel)DataContext).AnswerButtonVisibilityCollapsed += AnswerButtonVisibilityCollapsed;
            ((MainViewModel)DataContext).AnswerButtonVisibility += AnswerButtonVisibility;

            _dispatcherTimer = new DispatcherTimer();
            _dispatcherTimer.Interval = TimeSpan.FromMilliseconds(100);
            _dispatcherTimer.Tick += DispatcherTimer_Tick;
            TimerTextBlock.Visibility = Visibility.Collapsed;
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button clickedButton)
            {
                foreach (var child in stackPanel.Children)
                {
                    if (child is Button otherButton)
                    {
                        otherButton.Background = Brushes.LightGray;
                    }
                }
                clickedButton.Background = Brushes.LightBlue;
            }
        }


        private void DispatcherTimer_Tick(object sender, EventArgs e)
        {
            _elapsedTime = DateTime.Now - _startTime;
            TimerTextBlock.Text = _elapsedTime.ToString(@"mm\:ss\:ff");
        }

        private void SelectQuizButton_Click(object sender, RoutedEventArgs e)
        {
            SelectQuizButton.IsEnabled = false;
            _startTime = DateTime.Now;
            _dispatcherTimer.Start();
        }
        private void OnQuizCompletedOrQuizNotSelected()
        {
            SelectQuizButton.Dispatcher.Invoke(() =>
            {
                SelectQuizButton.IsEnabled = true;
                _dispatcherTimer.Stop();
                _viewModel.ElapsedTime = _elapsedTime;
                TimerTextBlock.Text = "00:00:00";
                TimerTextBlock.Visibility = Visibility.Collapsed;
            });
        }

        private void BeforeQuizCompleted()
        {
            NextQuestionButton.Dispatcher.Invoke(() => NextQuestionButton.Content = "Zakończ quiz");
            EndQuizButton.Visibility = Visibility.Collapsed;
        }

        private void ResetQuiz()
        {
            NextQuestionButton.Dispatcher.Invoke(() => NextQuestionButton.Content = "Następne pytanie");
        }

        private void AnswerButtonVisibilityCollapsed()
        {
            answerButton1.Visibility = Visibility.Collapsed;
            answerButton2.Visibility = Visibility.Collapsed;
            answerButton3.Visibility = Visibility.Collapsed;
            answerButton4.Visibility = Visibility.Collapsed;
            QuestionTextBlock.Visibility = Visibility.Collapsed;
            QuizListBox.UnselectAll();
            NextQuestionButton.Visibility = Visibility.Collapsed;
            EndQuizButton.Visibility = Visibility.Collapsed;
        }

        private void AnswerButtonVisibility()
        {
            answerButton1.Visibility = Visibility.Visible;
            answerButton2.Visibility = Visibility.Visible;
            answerButton3.Visibility = Visibility.Visible;
            answerButton4.Visibility = Visibility.Visible;
            QuestionTextBlock.Visibility = Visibility.Visible;
            NextQuestionButton.Visibility = Visibility.Visible;
            EndQuizButton.Visibility = Visibility.Visible;
            TimerTextBlock.Visibility = Visibility.Visible;
        }
    }
}
