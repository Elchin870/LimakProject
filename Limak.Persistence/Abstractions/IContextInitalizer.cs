namespace Limak.Persistence.Abstractions;

public interface IContextInitalizer
{
    Task InitDatabaseAsync();
}
