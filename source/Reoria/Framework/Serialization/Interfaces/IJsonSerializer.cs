namespace Reoria.Framework.Serialization.Interfaces
{
    public interface IJsonSerializer<InputType> : ISerializer<InputType, string> where InputType : class
    {
        // This interface doesn't define anything. It is purely used for requesting a JsonSerializer from DependencyInjection at this time.
    }
}
