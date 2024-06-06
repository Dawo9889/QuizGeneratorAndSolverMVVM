using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Threading;
using PierwszePodejscieDoQuizu.ViewModel;

namespace PierwszePodejscieDoQuizu.View
{
    public partial class EditWindow : Window
    {

        private EditWindowViewModel _viewModel;
        public EditWindow()
        {
            InitializeComponent();
            _viewModel = new EditWindowViewModel();
            DataContext = _viewModel;
            if (_viewModel != null)
            {
                this.Closing += EditWindow_Closing;
            }

            ((EditWindowViewModel)DataContext).OnQuizCompletedOrQuizNotSelected += OnQuizCompletedOrQuizNotSelected;
            ((EditWindowViewModel)DataContext).BeforeQuizCompleted += BeforeQuizCompleted;
            ((EditWindowViewModel)DataContext).ResetQuiz += ResetQuiz;
            ((EditWindowViewModel)DataContext).TextBoxesVisibilityCollapsed += TextBoxesVisibilityCollapsed;
            ((EditWindowViewModel)DataContext).TextBoxesVisibility += TextBoxesVisibility;
            ((EditWindowViewModel)DataContext).ShowInformationAboutEmptyTextBoxes += ShowInformationAboutEmptyTextBoxes;

            QuestionNrTextBlock.Visibility = Visibility.Collapsed;
        }
        private void ShowInformationAboutEmptyTextBoxes()
        {
            MessageBox.Show("Pola nie mogą być puste!", "Błąd", MessageBoxButton.OK, MessageBoxImage.Exclamation);
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
        private void NextQuestion_Click(object sender, RoutedEventArgs e)
        {
            string? questionContent = QuestionTextBlock.Text;
            string? answer1Content = answerTextBox1.Text;
            string? answer2Content = answerTextBox2.Text;
            string? answer3Content = answerTextBox3.Text;
            string? answer4Content = answerTextBox4.Text;

            List<string> listOfAnswers = new List<string>() {answer1Content, answer2Content, answer3Content, answer4Content };

            if (_viewModel.AnyNullContentInTextBoxes(questionContent, listOfAnswers))
            {
                MessageBox.Show("Pola tekstowe nie mogą być puste!", "Błąd", MessageBoxButton.OK, MessageBoxImage.Error);
            }
            else
            {
                _viewModel.NextQuestion(1);
            }
        }
        private void SelectQuizButton_Click(object sender, RoutedEventArgs e)
        {
            SelectQuizButton.IsEnabled = false;
        }
        private void OnQuizCompletedOrQuizNotSelected()
        {
            SelectQuizButton.Dispatcher.Invoke(() =>
            {
                SelectQuizButton.IsEnabled = true;
                QuestionNrTextBlock.Visibility = Visibility.Collapsed;
                NextQuestionButton.Background = Brushes.Gray;
            });
        }

        private void BeforeQuizCompleted()
        {
            NextQuestionButton.Dispatcher.Invoke(() => NextQuestionButton.Content = "Zakończ edycje quizu");
            NextQuestionButton.Background = Brushes.Red;
            EndQuizButton.Visibility = Visibility.Collapsed;
        }

        private void ResetQuiz()
        {
            NextQuestionButton.Dispatcher.Invoke(() => NextQuestionButton.Content = "Następne pytanie");
        }

        private void TextBoxesVisibilityCollapsed()
        {
            answerTextBox1.Visibility = Visibility.Collapsed;
            answerTextBox2.Visibility = Visibility.Collapsed;
            answerTextBox3.Visibility = Visibility.Collapsed;
            answerTextBox4.Visibility = Visibility.Collapsed;

            answerCheckBox1.Visibility = Visibility.Collapsed;
            answerCheckBox2.Visibility = Visibility.Collapsed;
            answerCheckBox3.Visibility = Visibility.Collapsed;
            answerCheckBox4.Visibility = Visibility.Collapsed;

            QuestionTextBlock.Visibility = Visibility.Collapsed;
            QuizListBox.UnselectAll();
            NextQuestionButton.Visibility = Visibility.Collapsed;
            EndQuizButton.Visibility = Visibility.Collapsed;
        }

        private void TextBoxesVisibility()
        {
            answerTextBox1.Visibility = Visibility.Visible;
            answerTextBox2.Visibility = Visibility.Visible;
            answerTextBox3.Visibility = Visibility.Visible;
            answerTextBox4.Visibility = Visibility.Visible;

            answerCheckBox1.Visibility = Visibility.Visible;
            answerCheckBox2.Visibility = Visibility.Visible;
            answerCheckBox3.Visibility = Visibility.Visible;
            answerCheckBox4.Visibility = Visibility.Visible;

            QuestionTextBlock.Visibility = Visibility.Visible;
            NextQuestionButton.Visibility = Visibility.Visible;
            EndQuizButton.Visibility = Visibility.Visible;;
            QuestionNrTextBlock.Visibility = Visibility.Visible;
        }
        private void EditWindow_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            var result = MessageBox.Show("Czy chcesz zapisać zmiany przed zamknięciem?", "Zamknięcie okna", MessageBoxButton.YesNoCancel, MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
            {
                _viewModel?.UpdateQuestionsAndAnswersInDatabase();
            }
            else if (result == MessageBoxResult.Cancel)
            {
                e.Cancel = true;
            }
        }
        private void EndQuizButton_Click(object sender, RoutedEventArgs e)
        {

        }

        private void NavigateToMainWindow_Click(object sender, RoutedEventArgs e)
        {
            MainWindow mainWindow = new MainWindow();
            mainWindow.Show();
            this.Close();
        }
    }
}
