using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Threading;
using PierwszePodejscieDoQuizu.ViewModel;

namespace PierwszePodejscieDoQuizu.View
{
    public partial class EditWindow : Window
    {
        private DispatcherTimer _dispatcherTimer;
        private DateTime _startTime;
        private TimeSpan _elapsedTime;
        private EditWindowViewModel _viewModel;
        public EditWindow()
        {
            InitializeComponent();
            _viewModel = new EditWindowViewModel();
            DataContext = _viewModel;

            ((EditWindowViewModel)DataContext).OnQuizCompletedOrQuizNotSelected += OnQuizCompletedOrQuizNotSelected;
            ((EditWindowViewModel)DataContext).BeforeQuizCompleted += BeforeQuizCompleted;
            ((EditWindowViewModel)DataContext).ResetQuiz += ResetQuiz;
            ((EditWindowViewModel)DataContext).AnswerButtonVisibilityCollapsed += AnswerButtonVisibilityCollapsed;
            ((EditWindowViewModel)DataContext).AnswerButtonVisibility += AnswerButtonVisibility;

            _dispatcherTimer = new DispatcherTimer();
            _dispatcherTimer.Interval = TimeSpan.FromMilliseconds(100);
            _dispatcherTimer.Tick += DispatcherTimer_Tick;
            QuestionNrTextBlock.Visibility = Visibility.Collapsed;
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
                QuestionNrTextBlock.Visibility = Visibility.Collapsed;
                NextQuestionButton.Background = Brushes.Gray;
            });
        }

        private void BeforeQuizCompleted()
        {
            NextQuestionButton.Dispatcher.Invoke(() => NextQuestionButton.Content = "Zakończ quiz");
            NextQuestionButton.Background = Brushes.Red;
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
            EndQuizButton.Visibility = Visibility.Visible;;
            QuestionNrTextBlock.Visibility = Visibility.Visible;
        }

        private void EndQuizButton_Click(object sender, RoutedEventArgs e)
        {

        }
    }
}
