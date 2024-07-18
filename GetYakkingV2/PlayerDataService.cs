using System.Collections.ObjectModel;
namespace GetYakkingV2;

public class PlayerDataService
{
    private static readonly Lazy<PlayerDataService> _instance = new Lazy<PlayerDataService>(() => new PlayerDataService());
    public static PlayerDataService Instance => _instance.Value;

    public ObservableCollection<Player> Players { get; private set; }

    private PlayerDataService()
    {
        Players = new ObservableCollection<Player>();
    }
}

