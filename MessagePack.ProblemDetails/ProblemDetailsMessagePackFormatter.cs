using System;
using System.Collections.Generic;
using MessagePack.Formatters;

namespace MessagePack.ProblemDetails
{
    public class ProblemDetailsMessagePackFormatter : IMessagePackFormatter<Microsoft.AspNetCore.Mvc.ProblemDetails>
    {
        public static readonly ProblemDetailsMessagePackFormatter Instance = new();


        public Microsoft.AspNetCore.Mvc.ProblemDetails Deserialize(ref MessagePackReader reader, MessagePackSerializerOptions options)
        {
            if (reader.IsNil)
                return default;

            var count = reader.ReadArrayHeader();
            if (count != HeaderCount)
                throw new InvalidOperationException(
                    $"A mismatch was occurred between serialized and deserialized header counts in {nameof(ProblemDetailsMessagePackFormatter)}");

            var detail = reader.ReadString();
            var instance = reader.ReadString();
            var title = reader.ReadString();
            var type = reader.ReadString();

            var nullableFormatter = options.Resolver.GetFormatterWithVerify<int?>();
            var status = nullableFormatter.Deserialize(ref reader, options);

            var dictionaryFormatter = options.Resolver.GetFormatterWithVerify<IDictionary<string, object>>();
            var extensions = dictionaryFormatter.Deserialize(ref reader, options);

            var result = new Microsoft.AspNetCore.Mvc.ProblemDetails
            {
                Detail = detail,
                Instance = instance,
                Status = status,
                Title = title,
                Type = type
            };

            foreach (var extension in extensions)
                result.Extensions.Add(extension);

            return result;
        }


        public void Serialize(ref MessagePackWriter writer, Microsoft.AspNetCore.Mvc.ProblemDetails value, MessagePackSerializerOptions options)
        {
            if (value is null)
            {
                writer.WriteNil();
                return;
            }


            writer.WriteArrayHeader(HeaderCount);
            writer.Write(value.Detail);
            writer.Write(value.Instance);
            writer.Write(value.Title);
            writer.Write(value.Type);

            var nullableFormatter = options.Resolver.GetFormatterWithVerify<int?>();
            nullableFormatter.Serialize(ref writer, value.Status, options);

            var dictionaryFormatter = options.Resolver.GetFormatterWithVerify<IDictionary<string, object>>();
            dictionaryFormatter.Serialize(ref writer, value.Extensions, options);
        }


        private const int HeaderCount = 6;
    }
}
