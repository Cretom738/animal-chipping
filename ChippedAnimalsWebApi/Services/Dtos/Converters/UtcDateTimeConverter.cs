using System.Text.Json;
using System.Text.Json.Serialization;

namespace Services.Dtos.Converters
{
    public class UtcDateTimeConverter : JsonConverter<DateTime>
    {
        public override DateTime Read(
            ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            DateTime deserializedDateTime = JsonSerializer.Deserialize<DateTime>(ref reader);
            if (deserializedDateTime.Kind != DateTimeKind.Utc)
            {
                return deserializedDateTime.ToUniversalTime();
            }
            return deserializedDateTime;
        }

        public override void Write(
            Utf8JsonWriter writer, DateTime value, JsonSerializerOptions options)
        {
            if (value.Kind != DateTimeKind.Utc)
            {
                value = value.ToUniversalTime();
            }
            writer.WriteStringValue(value);
        }
    }
}
