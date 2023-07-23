using Reoria.Framework.Serialization.Interfaces;

namespace Reoria.Framework.Serialization.Extensions
{
    public static class SerializerFileExtensions
    {
        public static InputType DeserializeFromFile<InputType>(this ISerializer<string> serializer, string filePath) where InputType : class
        {
            if(string.IsNullOrWhiteSpace(filePath)) { throw new ArgumentNullException(nameof(filePath)); }

            using var fileStream = new StreamReader(filePath);
            var fileContents = fileStream.ReadToEnd();
            fileStream.Close();

            return serializer.Deserialize<InputType>(fileContents) ?? Activator.CreateInstance<InputType>();
        }

        public static void SerializeToFile<InputType>(this ISerializer<string> serializer, InputType value, string filePath) where InputType : class
        {
            if (value is null) { throw new ArgumentNullException(nameof(value)); }
            if (string.IsNullOrWhiteSpace(filePath)) { throw new ArgumentNullException(nameof(filePath)); }

            using var fileStream = new StreamWriter(filePath, false);
            fileStream.Write(serializer.Serialize(value));
            fileStream.Close();
        }
    }
}
