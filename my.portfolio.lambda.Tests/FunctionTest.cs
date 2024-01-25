using Xunit;
using Amazon.Lambda.Core;
using Amazon.Lambda.TestUtilities;
using Amazon.SimpleEmail.Model;

namespace my.portfolio.lambda.Tests;

public class FunctionTest
{
    [Fact]
    public async  void TestToUpperFunction()
    {
        var input = "{\"From\": \"lalinmail@gmail.com\"," +
                    "\"To\": \"lalinmail@gmail.com\"," +
                    "\"Subject\": \"Test Data From Lambda\"," +
                    "\"Body\": \"Hi How Are You\"}";
        // Invoke the lambda function and confirm the string was upper cased.
        var function = new Function();
        var context = new TestLambdaContext();
        var output = await function.FunctionHandler(input, context);
        Assert.Contains("ok", output.ToLower());

    }
}
