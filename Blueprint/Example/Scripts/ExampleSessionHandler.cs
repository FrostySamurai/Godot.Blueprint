﻿using Godot;
using Samurai.Application;
using Samurai.Application.SessionHandling;
using Samurai.Example.Player;
using Samurai.Example.Player.Defs;

namespace Samurai.Example;

[GlobalClass]
public partial class ExampleSessionHandler : NodeSessionHandler
{
    [Export]
    public WeaponDefinition DefaultWeapon;
    
    public override void OnSessionStart()
    {
        Log.Debug($"Session start! Adding some models to session.", "ExampleSessionHandler");

        var playerModel = new PlayerModel
        {
            Weapon = DefaultWeapon
        };

        Session.Add(playerModel);
    }

    public override void OnSessionEnd()
    {
        Log.Debug($"Session End!", "ExampleSessionHandler");
    }
}