namespace Samurai.Example.Entities;

public interface IComponentData
{
    public EntityComponentDefinition ComponentDefinition { get; } // TODO: refactor this
    public void OnSave() {}
    public void OnLoad() {}
}