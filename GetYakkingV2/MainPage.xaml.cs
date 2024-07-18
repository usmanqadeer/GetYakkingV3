using System;
using System.Threading.Tasks;
using Microsoft.Maui.Controls;

namespace GetYakkingV2
{
    public partial class MainPage : ContentPage
    {
        public MainPage()
        {
            InitializeComponent();
        }

        private async Task AnimateButton(Button button)
        {
            // Scale up slightly
            await button.ScaleTo(1.1, 100, Easing.Linear);
            // Scale back to original size
            await button.ScaleTo(1.0, 100, Easing.Linear);
        }

        private async void OnClassicClicked(object sender, EventArgs e)
        {
            await AnimateButton(sender as Button);
            await Navigation.PushAsync(new ClassicPage());
        }

        private async void OnCouplesClicked(object sender, EventArgs e)
        {
            await AnimateButton(sender as Button);
            await Navigation.PushAsync(new CouplesPage());
        }

        private async void OnRiskyClicked(object sender, EventArgs e)
        {
            await AnimateButton(sender as Button);
            await Navigation.PushAsync(new RiskyPage());
        }
    }
}
