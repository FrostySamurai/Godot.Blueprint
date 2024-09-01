namespace Samurai.Application.Saving
{
    public interface ISavable
    {
        public string Id { get; }
        
        public void OnSave() {}
        public void OnLoad() {}
    }
}