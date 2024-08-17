using Godot;
using Samurai.Application;
using Samurai.Application.SessionHandling;

namespace Samurai.Example;

[GlobalClass]
public partial class ExampleSessionHandler : NodeSessionHandler
{
    [Export]
    public string Data;
    
    public override void OnSessionStart()
    {
        Log.Debug($"Session start! Adding some models to session.", "Example");
        Session.Add(new ExampleModel(Data));
    }

    public override void OnSessionEnd()
    {
        Log.Debug($"Session End!", "Example");
    }
}