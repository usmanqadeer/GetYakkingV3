using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Maui.Controls;

namespace GetYakkingV2
{
    public partial class ClassicPage : ContentPage
    {
        private bool areRulesVisible = false;
        private int flipCounter = 0; // Counter for card flips
        private List<Question> questions, unrankedQuestions, rankedQuestions;
        private Question currentQuestion;
        private int questionDisplayCount = 0;
        private System.Timers.Timer rankTimer;

        public ClassicPage()
        {
            InitializeComponent();
            LoadQuestions();
            unrankedQuestions = new List<Question>(questions);
            rankedQuestions = new List<Question>();
            SetupTimer();
            // DisplayQuestion(); // Comment out this initial call
        }

        private void LoadQuestions()
        {
            var assembly = IntrospectionExtensions.GetTypeInfo(typeof(ClassicPage)).Assembly;
            Stream stream = assembly.GetManifestResourceStream("GetYakkingV2.Questions.ClassicQuestions.json");
            if (stream == null)
            {
                throw new FileNotFoundException("Embedded resource not found.");
            }
            using (var reader = new StreamReader(stream))
            {
                var jsonString = reader.ReadToEnd();
                questions = JsonSerializer.Deserialize<List<Question>>(jsonString);
            }
        }

        private void SetupTimer()
        {
            rankTimer = new System.Timers.Timer(20000);
            rankTimer.Elapsed += OnTimedEvent;
            rankTimer.AutoReset = true;
            rankTimer.Enabled = true;
        }

        private void OnTimedEvent(Object source, System.Timers.ElapsedEventArgs e)
        {
            Device.BeginInvokeOnMainThread(() =>
            {
                if (currentQuestion != null)
                {
                    currentQuestion.Rank += 1;
                    IncrementCategoryRank(currentQuestion.Category);
                }
            });
        }

        private void IncrementCategoryRank(string category)
        {
            var categoryQuestions = questions.Where(q => q.Category == category).ToList();
            foreach (var question in categoryQuestions)
            {
                if (question != currentQuestion)
                {
                    question.Rank += 1;
                }
            }
        }

        private void DisplayQuestion()
        {
            questionDisplayCount++;
            if (questionDisplayCount % 5 == 0 && rankedQuestions.Any())
            {
                currentQuestion = SelectRandomQuestion(rankedQuestions);
            }
            else
            {
                currentQuestion = SelectRandomQuestion(unrankedQuestions);
                if (currentQuestion != null)
                {
                    unrankedQuestions.Remove(currentQuestion);
                    rankedQuestions.Add(currentQuestion);
                }
            }

            if (currentQuestion != null)
            {
                questionLabel.Text = currentQuestion.QuestionText;
                questionLabel.IsVisible = true;
            }
        }

        private Question SelectRandomQuestion(List<Question> questionsList)
        {
            if (questionsList.Count > 0)
            {
                var random = new Random();
                int index = random.Next(questionsList.Count);
                return questionsList[index];
            }
            return null;
        }

        private async Task AnimateButton(Button button)
        {
            await button.ScaleTo(1.5, 100, Easing.Linear);
            await button.ScaleTo(1.0, 100, Easing.Linear);
        }

        private async void OnRulesClicked(object sender, EventArgs e)
        {
            await AnimateButton((Button)sender);
            areRulesVisible = !areRulesVisible;
            card.IsVisible = !areRulesVisible;
            rulesLabel.IsVisible = areRulesVisible;
            rulesButton.Text = areRulesVisible ? "Hide" : "Rules";
        }

        private async void OnScoreClicked(object sender, EventArgs e)
        {
            await AnimateButton((Button)sender);
            await Navigation.PushAsync(new ScoreboardPage());
        }

        private async void OnCardTapped(object sender, EventArgs e)
        {
            await FlipCard(frontView, backView, true);
        }

        private async void OnBackCardTapped(object sender, EventArgs e)
        {
            await FlipCard(backView, frontView, false);
        }

        private async Task FlipCard(View fromView, View toView, bool displayQuestionOnFlip)
        {
            // Temporarily remove the shadow for the flip animation
            card.Shadow = null;
            await fromView.RotateYTo(90, 250);
            fromView.IsVisible = false;
            toView.IsVisible = true;
            toView.RotationY = -90;
            await toView.RotateYTo(0, 250);

            // Display the question if needed
            if (toView == backView && displayQuestionOnFlip)
            {
                flipCounter++;
                DisplayQuestion(); // Display question only after the flip
                questionLabel.IsVisible = true; // Show the question label after flip
            }
            else
            {
                questionLabel.IsVisible = false; // Hide the question label after flip
            }

            // Reapply the shadow to the card
            ApplyShadowToCard();
        }

        private void ApplyShadowToCard()
        {
            card.Shadow = new Shadow
            {
                Brush = Brush.Black,
                Offset = new Point(6, 12),
                Opacity = 0.6F, // Use 'F' suffix to denote float
                Radius = 8F    // Use 'F' suffix to denote float
            };
        }

        public class Question
        {
            public string Id { get; set; }
            public string Category { get; set; }
            public string QuestionText { get; set; }
            public int Rank { get; set; }
        }
    }
}
