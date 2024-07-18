using System;
using System.Collections.ObjectModel;
using Microsoft.Maui.Controls;

namespace GetYakkingV2
{
    public partial class RemovePlayerPage : ContentPage
    {
        public RemovePlayerPage()
        {
            InitializeComponent();
            playersList.ItemsSource = PlayerDataService.Instance.Players;
        }

        private async void OnPlayerTapped(object sender, ItemTappedEventArgs e)
        {
            if (e.Item is Player player)
            {
                var response = await DisplayAlert("Confirm", $"Are you sure you want to remove {player.Name}?", "Yes", "No");
                if (response)
                {
                    PlayerDataService.Instance.Players.Remove(player);
                }
            }
        }
    }
}
