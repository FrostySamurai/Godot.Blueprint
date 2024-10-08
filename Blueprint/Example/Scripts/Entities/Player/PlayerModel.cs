﻿using Newtonsoft.Json;
using Samurai.Application.Saving;

namespace Samurai.Example.Entities.Player;

public class PlayerModel : ISavable
{
    [JsonIgnore]
    public string Id => "player_model";

    public string WeaponId;

    [JsonIgnore]
    public PlayerController Player;
}