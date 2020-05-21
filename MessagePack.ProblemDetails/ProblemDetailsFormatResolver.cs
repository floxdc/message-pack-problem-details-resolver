using MessagePack.Formatters;

namespace MessagePack.ProblemDetails
{
    public class ProblemDetailsFormatResolver : IFormatterResolver
    {
        private ProblemDetailsFormatResolver()
        { }


        public IMessagePackFormatter<T> GetFormatter<T>() => FormatterCache<T>.Formatter;


        public static readonly IFormatterResolver Instance = new ProblemDetailsFormatResolver();
    }


    internal static class FormatterCache<T>
    {
        static FormatterCache()
        {
            Formatter = (IMessagePackFormatter<T>) ProblemDetailsMessagePackFormatter.Instance;
        }


        public static readonly IMessagePackFormatter<T> Formatter;
    }
}
