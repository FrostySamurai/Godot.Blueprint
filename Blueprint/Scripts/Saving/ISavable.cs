namespace Samurai.Application.Saving
{
    public interface ISavable
    {
        public void OnSave() {}
        public void OnLoad() {}
    }
}