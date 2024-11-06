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


        public static void ReparentSafe(this Node node, Node newParent)
        {
            if (newParent is null)
            {
                return;
            }

            if (node.GetParent() is not null)
            {
                node.Reparent(newParent);
            }
            else
            {
                newParent.AddChild(node);
            }
        }
    }
}