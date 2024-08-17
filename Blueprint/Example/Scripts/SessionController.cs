using Godot;
using Samurai.Application;

namespace Samurai.Example;

public partial class SessionController : Node
{
    [Export]
    private CanvasLayer _pauseView;

    public override void _Input(InputEvent @event)
    {
        base._Input(@event);

        if (@event.IsActionPressed("ui_cancel"))
        {
            App.TogglePause();
            _pauseView.Visible = App.IsPaused;
        }
    }
}