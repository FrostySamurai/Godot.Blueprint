using Godot;

namespace Samurai.Example.Weapons;

public partial class Weapon : Node2D
{
    internal const string LogTag = "Weapons";

    public bool IsActive { get; private set; }

    public void SetActive(bool state)
    {
        IsActive = state;
    }
}