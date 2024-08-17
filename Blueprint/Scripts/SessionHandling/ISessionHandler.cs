namespace Samurai.Application.SessionHandling
{
    public interface ISessionHandler
    {
        int Priority => 0;
        void OnSessionStart() {}
        void OnSessionEnd() {}
    }
}