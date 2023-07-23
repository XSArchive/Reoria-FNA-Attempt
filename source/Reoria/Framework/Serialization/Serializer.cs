using Reoria.Framework.Serialization.Interfaces;

namespace Reoria.Framework.Serialization
{
    public abstract class Serializer<InputType, OutputType> : ISerializer<InputType, OutputType> where InputType : class where OutputType : class
    {
        public abstract InputType Deserialize(OutputType value);
        public abstract OutputType Serialize(InputType value);
    }
}
