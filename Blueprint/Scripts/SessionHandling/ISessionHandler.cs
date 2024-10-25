namespace Samurai.Application.SessionHandling
{
    public interface ISessionHandler
    {
        int Priority => 0;
        bool IsOneShot => false;
        void OnSessionStart() {}
        void OnSessionEnd() {}
    }
}