using System;
using Microsoft.Maui.Controls;

namespace GetYakkingV2
{
    public partial class ScoreboardPage : ContentPage
    {
        public ScoreboardPage()
        {
            InitializeComponent();
        }

        private void OnAddPlayerClicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new AddPlayerPage());
        }

        private void OnRemovePlayerClicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new RemovePlayerPage());
        }

        private void OnViewClicked(object sender, EventArgs e)
        {
            Navigation.PushAsync(new ViewPlayersPage());
        }
    }
}
