namespace Reoria.Framework.Serialization.Interfaces
{
    public interface ISerializer<OutputType> where OutputType : class
    {
        OutputType Serialize<InputType>(InputType value) where InputType : class;
        InputType Deserialize<InputType>(OutputType value) where InputType : class;
    }
}
