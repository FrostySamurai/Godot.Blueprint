using Godot;
using Samurai.Application;

namespace Samurai.Example;

public partial class SessionReferences : Node2D
{
    [Export]
    public Node2D ProjectileParent;
    [Export]
    public Node2D EnemyParent;

    public override void _EnterTree()
    {
        base._EnterTree();
        
        Session.Add(this);
    }
}