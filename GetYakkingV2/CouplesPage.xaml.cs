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
    public partial class CouplesPage : ContentPage
    {
        private bool areRulesVisible = false;
        private int flipCounter = 0; // Counter for card flips
        private List<Question> questions, unrankedQuestions, rankedQuestions;
        private Question currentQuestion;

        public CouplesPage()
        {
            InitializeComponent();
            LoadQuestions();
            unrankedQuestions = new List<Question>(questions);
            rankedQuestions = new List<Question>();
        }

        private void LoadQuestions()
        {
            var assembly = IntrospectionExtensions.GetTypeInfo(typeof(CouplesPage)).Assembly;
            Stream stream = assembly.GetManifestResourceStream("GetYakkingV2.Questions.Couples.json");
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

        private void DisplayQuestion()
        {
            currentQuestion = SelectRandomQuestion(unrankedQuestions);
            if (currentQuestion != null)
            {
                unrankedQuestions.Remove(currentQuestion);
                rankedQuestions.Add(currentQuestion);
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
            card.Shadow = null;
            await fromView.RotateYTo(90, 250);
            fromView.IsVisible = false;
            toView.IsVisible = true;
            toView.RotationY = -90;
            await toView.RotateYTo(0, 250);

            if (toView == backView && displayQuestionOnFlip)
            {
                flipCounter++;
                DisplayQuestion();
                questionLabel.IsVisible = true;
            }
            else
            {
                questionLabel.IsVisible = false;
            }

            ApplyShadowToCard();
        }

        private void ApplyShadowToCard()
        {
            card.Shadow = new Shadow
            {
                Brush = Brush.Black,
                Offset = new Point(10f, 10f),
                Opacity = 0.6f,
                Radius = 10
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
