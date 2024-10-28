using Newtonsoft.Json;
using Samurai.Application.Saving;

namespace Samurai.Example.Entities.Player;

public class PlayerModel : ISavable
{
    public string WeaponId;

    [JsonIgnore]
    public PlayerController Player;
}