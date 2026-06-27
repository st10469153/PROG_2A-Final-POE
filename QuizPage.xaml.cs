using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;

namespace ChatBotApp
{
    public partial class QuizPage : Window
    {
        public class Question
        {
            public string QuestionText { get; set; }
            public string[] Options { get; set; }
            public int CorrectAnswer { get; set; }
        }

        private int questionNr = 1;
        private int score = 0;

        private Dictionary<int, Question> questions = new Dictionary<int, Question>();

        public QuizPage()
        {
            InitializeComponent();

            questions.Add(1, new Question
            {
                QuestionText = "Question 1:Online safety should be treated as an ongoing habit ",
                Options = new string[] { "True", "False" },
                CorrectAnswer = 1
            });

            questions.Add(2, new Question
            {
                QuestionText = "Question 2: What is one of the most effective defenses against online threats?",
                Options = new string[] { "Click on any link that you see", "Slow down and think before you respond", "Call the Police", "Send an email to the developers" },
                CorrectAnswer = 2
            });

            questions.Add(3, new Question
            {
                QuestionText = "Question 3: Limiting the amount of personal information you share online\n will improve your cybersecurity skills",
                Options = new string[] { "True", "False" },
                CorrectAnswer = 1

            });

            questions.Add(4, new Question
            {
                QuestionText = "Question 4: What is the best way to protect your online accounts?",
                Options = new string[] { "Use strong and unique passwords for each account", "Share your passwords with friends", "Use the same password for all accounts", "Write your passwords on a sticky note" },
                CorrectAnswer = 1
            });

            questions.Add(5, new Question
            {
                QuestionText = "Question 5: What is the purpose of two-factor authentication (2FA)?",
                Options = new string[] { "To make logging in faster", "To provide an extra layer of security", "To allow anyone to access your account", "To eliminate the need for passwords" },
                CorrectAnswer = 2
            });
            LoadQuestion();

        }

        private void LoadQuestion()
        {
            Question currentQuestion = questions[questionNr];

            displayQuestion.Text = currentQuestion.QuestionText;
            feedbackText.Text = "...";

            Option1.Content = currentQuestion.Options[0];
            Option2.Content = currentQuestion.Options[1];

            Option3.Visibility = Visibility.Hidden;
            Option4.Visibility = Visibility.Hidden;

            if (currentQuestion.Options.Length == 4)
            {
                Option3.Content = currentQuestion.Options[2];
                Option4.Content = currentQuestion.Options[3];

                Option3.Visibility = Visibility.Visible;
                Option4.Visibility = Visibility.Visible;
            }

            Option1.IsChecked = false;
            Option2.IsChecked = false;
            Option3.IsChecked = false;
            Option4.IsChecked = false;
        }

        private void submitBtn_Click(object sender, RoutedEventArgs e)
        {
            int selectedAnswer = 0;

            if (Option1.IsChecked == true) selectedAnswer = 1;
            if (Option2.IsChecked == true) selectedAnswer = 2;
            if (Option3.IsChecked == true) selectedAnswer = 3;
            if (Option4.IsChecked == true) selectedAnswer = 4;

            if (selectedAnswer == questions[questionNr].CorrectAnswer)
            {
                score++;
                feedbackText.Text = "Correct!";
            }

            questionNr++;

            if (questionNr > questions.Count)
            {
                MessageBox.Show($"Quiz Finished!\nYour Score: {score}/{questions.Count}");

                Close();
                return;
            }

            LoadQuestion();
        }
    }
}