namespace Samurai.Application.Pooling;

public interface IPoolRetrievable
{
    public void OnRetrieveFromPool();
}

public interface IPoolReturnable
{
    public void OnReturnToPool();
}