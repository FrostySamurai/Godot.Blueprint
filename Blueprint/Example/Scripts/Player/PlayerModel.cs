using System;
using Newtonsoft.Json;
using Samurai.Application.Saving;

namespace Samurai.Example.Player;

[Serializable]
public class PlayerModel : ISavable
{
    public string WeaponId;

    [JsonIgnore]
    public PlayerController Player;
}