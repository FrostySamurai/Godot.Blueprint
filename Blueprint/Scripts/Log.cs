using Godot;
using RedHerring.Extensions.Collections;

namespace Samurai.Application
{
    public static class Log
    {
        public static void Debug(string message, string tag = null)
        {
            if (tag.IsNullOrEmpty())
            {
                GD.Print(message);
                return;
            }
            
            GD.Print($"[{tag}] {message}");
        }
        
        public static void Error(string message, string tag = null)
        {
            if (tag.IsNullOrEmpty())
            {
                GD.PrintErr(message);
                return;
            }
            
            GD.PrintErr($"[{tag}] {message}");
        }
    }
}