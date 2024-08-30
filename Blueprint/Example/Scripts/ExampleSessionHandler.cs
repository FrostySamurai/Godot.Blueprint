using Godot;
using Samurai.Application;
using Samurai.Application.SessionHandling;
using Samurai.Example.Entities.Enemies;
using Samurai.Example.Entities.Player;
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
            WeaponId = DefaultWeapon.Id
        };

        Session.Add(playerModel);
        Session.Add(new SpawnManager(GetViewport()));
    }

    public override void OnSessionEnd()
    {
        Log.Debug($"Session End!", "ExampleSessionHandler");
    }
}