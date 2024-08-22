using Newtonsoft.Json;
using Samurai.Application.Saving;
using Samurai.Example.Player.Defs;

namespace Samurai.Example.Player;

public class PlayerModel : ISavable
{
    [JsonIgnore]
    public string Id => "player_model";

    public WeaponDefinition Weapon;
}