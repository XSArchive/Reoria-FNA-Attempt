using Reoria.Framework.Serialization.Interfaces;
using System.Text.Json;
using System.Text.Json.Serialization;
using Json = System.Text.Json.JsonSerializer;

namespace Reoria.Framework.Serialization
{
    public class JsonSerializer : Serializer<string>, IJsonSerializer
    {
        private readonly JsonSerializerOptions jsonSettings;

        public JsonSerializer()
        {
            this.jsonSettings = new JsonSerializerOptions()
            {
                AllowTrailingCommas = true,
                DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
                NumberHandling = JsonNumberHandling.AllowNamedFloatingPointLiterals | JsonNumberHandling.AllowReadingFromString,
                WriteIndented = true,
                UnmappedMemberHandling = JsonUnmappedMemberHandling.Skip,
            };
        }

        public JsonSerializer(JsonSerializerOptions jsonSettings)
        {
            this.jsonSettings = jsonSettings;
        }

        public override InputType Deserialize<InputType>(string value)
        {
            if(string.IsNullOrWhiteSpace(value)) { return Activator.CreateInstance<InputType>(); }

            return Json.Deserialize<InputType>(value, jsonSettings) ?? Activator.CreateInstance<InputType>();
        }

        public override string Serialize<InputType>(InputType value)
        {
            if (value is null) { return string.Empty; }

            return Json.Serialize(value, jsonSettings);
        }
    }
}
