using System.Text.Json;
using System.Text.Json.Serialization;

namespace backend.Models
{
    public class EnumConverter<TEnum> : JsonConverter<TEnum> where TEnum : Enum
    {
        public override TEnum Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            var stringValue = reader.GetString();
            return (TEnum)Enum.Parse(typeof(TEnum), stringValue, ignoreCase: true);
        }

        public override void Write(Utf8JsonWriter writer, TEnum value, JsonSerializerOptions options)
        {
            writer.WriteStringValue(value.ToString());
        }
    }


}
