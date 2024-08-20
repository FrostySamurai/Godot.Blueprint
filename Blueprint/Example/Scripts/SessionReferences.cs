using Godot;
using Samurai.Application;

namespace Samurai.Example;

public partial class SessionReferences : Node2D
{
    [Export]
    public Node2D ProjectileParent;

    public override void _EnterTree()
    {
        base._EnterTree();
        
        Session.Add(this);
    }
}