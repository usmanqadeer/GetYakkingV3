using System.Collections.ObjectModel;
using System.Linq;
using Microsoft.Maui.Controls;

namespace GetYakkingV2
{
    public partial class ViewPlayersPage : ContentPage
    {
        public ViewPlayersPage()
        {
            InitializeComponent();
            SortAndBindPlayers();
            SubscribeToScoreChanges();
        }

        private void SortAndBindPlayers()
        {
            var sortedPlayers = PlayerDataService.Instance.Players
                                .OrderByDescending(player => player.Score)
                                .ToList();

            playersList.ItemsSource = new ObservableCollection<Player>(sortedPlayers);
        }

        private void SubscribeToScoreChanges()
        {
            foreach (var player in PlayerDataService.Instance.Players)
            {
                player.OnScoreChanged += Player_OnScoreChanged;
            }
        }

        private void Player_OnScoreChanged()
        {
            SortAndBindPlayers(); // Re-sort and update the list
        }

        private async void OnPlayerTapped(object sender, ItemTappedEventArgs e)
        {
            if (e.Item is Player player)
            {
                bool confirm = await DisplayAlert("Confirm", $"Are you sure you want to give a point to {player.Name}?", "Yes", "No");
                if (confirm)
                {
                    player.IncrementScore();
                }
            }
        }

        protected override void OnDisappearing()
        {
            base.OnDisappearing();
            foreach (var player in PlayerDataService.Instance.Players)
            {
                player.OnScoreChanged -= Player_OnScoreChanged;
            }
        }
    }
}
