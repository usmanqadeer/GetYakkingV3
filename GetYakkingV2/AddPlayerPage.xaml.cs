using System;
using System.Collections.ObjectModel;
using Microsoft.Maui.Controls;

namespace GetYakkingV2;

public partial class AddPlayerPage : ContentPage
{
    public AddPlayerPage()
    {
        InitializeComponent();
        playersList.ItemsSource = PlayerDataService.Instance.Players;
    }

    private void OnPlayerNameEntryCompleted(object sender, EventArgs e)
    {
        var playerName = playerNameEntry.Text;
        if (!string.IsNullOrWhiteSpace(playerName))
        {
            PlayerDataService.Instance.Players.Add(new Player(playerName));
            playerNameEntry.Text = string.Empty;
        }
    }
}
