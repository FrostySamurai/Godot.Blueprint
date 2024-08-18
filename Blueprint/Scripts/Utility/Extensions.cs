using System.IO;
using Godot;

namespace Samurai.Application.Utility
{
    public static class Extensions
    {
        public static string Name(this PackedScene @this)
        {
            if (@this is null)
            {
                return string.Empty;
            }

            return Path.GetFileNameWithoutExtension(@this.GetPath());
        }
    }
}