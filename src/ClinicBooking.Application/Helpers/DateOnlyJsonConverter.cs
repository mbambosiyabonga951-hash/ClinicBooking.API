using System;

using System.Globalization;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace ClinicBooking.Application.Helpers
{
    public class DateOnlyJsonConverter : JsonConverter<DateOnly>
    {
        private const string Format = "yyyy-MM-dd";
        private static readonly CultureInfo Invariant = CultureInfo.InvariantCulture;

        public override DateOnly Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            var s = reader.GetString();
            if (string.IsNullOrWhiteSpace(s))
                throw new JsonException("Expected non-empty date string.");

            // Try exact date-only first
            if (DateOnly.TryParseExact(s, Format, Invariant, DateTimeStyles.None, out var date))
                return date;

            // Try generic DateOnly parse
            if (DateOnly.TryParse(s, Invariant, DateTimeStyles.None, out date))
                return date;

            // Fallback: parse as DateTimeOffset or DateTime and extract date part
            if (DateTimeOffset.TryParse(s, Invariant, DateTimeStyles.AllowWhiteSpaces | DateTimeStyles.AssumeUniversal, out var dto))
                return DateOnly.FromDateTime(dto.DateTime);

            if (DateTime.TryParse(s, Invariant, DateTimeStyles.AllowWhiteSpaces, out var dt))
                return DateOnly.FromDateTime(dt);

            throw new JsonException($"Cannot parse '{s}' as DateOnly.");
        }

        public override void Write(Utf8JsonWriter writer, DateOnly value, JsonSerializerOptions options)
            => writer.WriteStringValue(value.ToString(Format, Invariant));
    }
}
