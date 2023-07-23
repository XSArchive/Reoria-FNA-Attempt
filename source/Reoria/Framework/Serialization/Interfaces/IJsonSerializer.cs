namespace Reoria.Framework.Serialization.Interfaces
{
    public interface IJsonSerializer : ISerializer<string>
    {
        // This interface doesn't define anything. It is purely used for requesting a JsonSerializer from DependencyInjection at this time.
    }
}
