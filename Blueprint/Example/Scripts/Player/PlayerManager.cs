using Godot;
using Samurai.Application;
using Samurai.Example.Entities;

namespace Samurai.Example.Player;

public partial class PlayerManager : Node2D
{
    [Export]
    private EntityDefinition _playerDefinition;
    
    public override void _EnterTree()
    {
        base._EnterTree();

        var parent = Session.Get<SessionReferences>().PlayerParent;
        EntitySystem.Spawn(_playerDefinition, parent, GetViewportRect().Size / 2f);
    }
}