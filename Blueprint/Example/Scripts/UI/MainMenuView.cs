using Godot;
using Samurai.Application;

namespace Samurai.Example.UI;

public partial class MainMenuView : Node
{
    
    [ExportGroup("Interaction")]
    [Export]
    private Button _startButton;
    [Export]
    private Button _quitButton;

    public override void _EnterTree()
    {
        _startButton.Pressed += StartSession;
        _quitButton.Pressed += App.Quit;
    }

    private void StartSession()
    {
        Session.Register(new LevelSessionHandler("test"));
        App.StartSession("Session name");
    }
}