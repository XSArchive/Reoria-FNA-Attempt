using Reoria.Framework.Serialization.Interfaces;

namespace Reoria.Framework.Serialization
{
    public abstract class Serializer<OutputType> : ISerializer<OutputType> where OutputType : class
    {
        public abstract InputType Deserialize<InputType>(OutputType value) where InputType : class;
        public abstract OutputType Serialize<InputType>(InputType value) where InputType : class;
    }
}
