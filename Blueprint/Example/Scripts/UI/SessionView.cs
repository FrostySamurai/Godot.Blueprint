using Godot;
using Samurai.Application;

namespace Samurai.Example.UI;

public partial class SessionView : CanvasLayer
{
    [Export]
    private Button _continueButton;
    [Export]
    private Button _mainMenuButton;

    public override void _EnterTree()
    {
        _continueButton.Pressed += () =>
        {
            Hide();
            App.SetPause(false);
        };
        _mainMenuButton.Pressed += App.EndSession;
    }
}