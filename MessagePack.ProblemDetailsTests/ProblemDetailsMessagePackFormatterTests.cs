using MessagePack.ProblemDetails;
using MessagePack.Resolvers;
using Xunit;

namespace MessagePack.ProblemDetailsTests
{
    public class ProblemDetailsMessagePackFormatterTests
    {
        public ProblemDetailsMessagePackFormatterTests()
        {
            var resolver = CompositeResolver.Create(StandardResolver.Instance, ProblemDetailsFormatResolver.Instance);
            _options = MessagePackSerializerOptions.Standard
                .WithResolver(resolver);
        }


        [Fact]
        public void ShouldHandleNull()
        {
            var serialized = MessagePackSerializer.Serialize((Microsoft.AspNetCore.Mvc.ProblemDetails) null, _options);
            var deserialized = MessagePackSerializer.Deserialize<Microsoft.AspNetCore.Mvc.ProblemDetails>(serialized, _options);

            Assert.Null(deserialized);
        }
    
     
        [Fact]
        public void ShouldHandleEmptyProblemDetails()
        {
            var details = new Microsoft.AspNetCore.Mvc.ProblemDetails();

            var serialized = MessagePackSerializer.Serialize(details, _options);
            var deserialized = MessagePackSerializer.Deserialize<Microsoft.AspNetCore.Mvc.ProblemDetails>(serialized, _options);

            Assert.Null(deserialized.Detail);
            Assert.Null(deserialized.Instance);
            Assert.Null(deserialized.Status);
            Assert.Null(deserialized.Title);
            Assert.Null(deserialized.Type);
            Assert.Empty(deserialized.Extensions);
        }
    
     
        [Fact]
        public void ShouldHandleProblemDetails()
        {
            var details = new Microsoft.AspNetCore.Mvc.ProblemDetails
            {
                Type = "https://tools.ietf.org/html/rfc7231#section-6.6.1",
                Title = "Internal Server Error",
                Status = 500,
                Detail = "Failed to serialize CSharpFunctionalExtensions.Result`2[[System.Decimal, System.Private.CoreLib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=7cec85d7bea7798e],[Microsoft.AspNetCore.Mvc.ProblemDetails, Microsoft.AspNetCore.Mvc.Core, Version=3.1.4.0, Culture=neutral, PublicKeyToken=adb9793829ddae60]] value."
            };

            details.Extensions.Add("spanId", "0000000000000000");
            details.Extensions.Add("requestId", "0HLVTHCNOGD28");

            var serialized = MessagePackSerializer.Serialize(details, _options);
            var deserialized = MessagePackSerializer.Deserialize<Microsoft.AspNetCore.Mvc.ProblemDetails>(serialized, _options);

            Assert.Equal(details.Detail, deserialized.Detail);
            Assert.Null(deserialized.Instance);
            Assert.Equal(details.Status, deserialized.Status);
            Assert.Equal(details.Title, deserialized.Title);
            Assert.Equal(details.Type, deserialized.Type);
            Assert.Equal(details.Extensions, deserialized.Extensions);
        }
    
        
        private readonly MessagePackSerializerOptions _options;
    }
}
