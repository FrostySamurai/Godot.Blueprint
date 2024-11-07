using Godot;
using Samurai.Application;

namespace Samurai.Example;

public partial class SessionReferences : Node2D
{
    [Export]
    public Node2D GameRoot;
    [Export]
    public Node2D ProjectileParent;
    [Export]
    public Node2D EnemyParent;
    [Export]
    public Node2D PlayerParent;

    public override void _EnterTree()
    {
        base._EnterTree();
        
        Session.Add(this);
    }
}