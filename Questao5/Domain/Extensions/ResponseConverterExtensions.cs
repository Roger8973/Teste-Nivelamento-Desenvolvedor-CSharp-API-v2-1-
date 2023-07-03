using Questao5.Application;
using Questao5.Application.Commands.Responses;
using Questao5.Application.Queries.Responses;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Questao5.Domain.Extensions
{
    public class ResponseConverterExtensions : JsonConverter<Response>
    {
        public override void Write(Utf8JsonWriter writer, Response value, JsonSerializerOptions options)
        {
            writer.WriteStartObject();

            writer.WritePropertyName("Success");
            JsonSerializer.Serialize(writer, value.Success, options);

            if (value.Success)
            {
                var dataProperty = value.GetType().GetProperty("Data");

                if (dataProperty != null)
                {
                    var dataValue = dataProperty.GetValue(value);

                    writer.WritePropertyName("Data");
                    JsonSerializer.Serialize(writer, dataValue, options);
                }
            }      
            else
            {
                writer.WritePropertyName("Errors");
                JsonSerializer.Serialize(writer, value.Error, options);
            }

            writer.WriteEndObject();
        }
        public override Response Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            throw new NotImplementedException();
        }

        public override bool CanConvert(Type typeToConvert)
        {
            return typeof(Response).IsAssignableFrom(typeToConvert);
        }
    }
}
