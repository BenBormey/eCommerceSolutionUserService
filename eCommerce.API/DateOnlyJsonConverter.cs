using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace eCommerce.API
{
    public class DateOnlyJsonConverter : JsonConverter<DateOnly>
    {
        private static readonly string[] Formats = { "yyyy-MM-dd" };
        public override DateOnly Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            var s = reader.GetString();
            if (s is null) throw new JsonException("Date is null");
            if (DateOnly.TryParseExact(s, Formats, CultureInfo.InvariantCulture, DateTimeStyles.None, out var d))
                return d;
            throw new JsonException("Invalid date format. Use 'yyyy-MM-dd'.");
        }
        public override void Write(Utf8JsonWriter writer, DateOnly value, JsonSerializerOptions options) =>
            writer.WriteStringValue(value.ToString("yyyy-MM-dd", CultureInfo.InvariantCulture));
    }
}
