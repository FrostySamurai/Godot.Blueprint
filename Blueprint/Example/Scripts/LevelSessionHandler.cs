using Samurai.Application;
using Samurai.Application.SessionHandling;

namespace Samurai.Example;

public class LevelSessionHandler : ISessionHandler
{
    public bool IsOneShot => true;

    private readonly string _level;
    
    public LevelSessionHandler(string level)
    {
        _level = level;
    }

    public void OnSessionStart()
    {
        Session.Add(new LevelModel(_level));
    }
}