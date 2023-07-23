namespace Reoria.Framework.Serialization.Interfaces
{
    public interface ISerializer<InputType, OutputType> where InputType : class where OutputType : class
    {
        OutputType Serialize(InputType value);
        InputType Deserialize(OutputType value);
    }
}