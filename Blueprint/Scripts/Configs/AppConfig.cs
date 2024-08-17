using Godot;

namespace Samurai.Application.Configs
{
    [GlobalClass]
    public partial class AppConfig : Config
    {
        [ExportGroup("General")]
        [Export]
        public PackedScene MainMenuScene;
        [Export]
        public PackedScene SessionScene;
        
        [ExportGroup("Saving")]
        [Export]
        public string SavesFolder;
        [Export]
        public bool EnableAutosaves;
    }
}