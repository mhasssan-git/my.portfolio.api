using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;
using Amazon.Lambda.Core;
using Amazon.SimpleEmail;
using Amazon.SimpleEmail.Model;
using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;

// Assembly attribute to enable the Lambda function's JSON input to be converted into a .NET class.
[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.SystemTextJson.DefaultLambdaJsonSerializer))]

namespace my.portfolio.lambda;


public class Function
{

    /// <summary>
    /// A simple function that takes a string and does a ToUpper
    /// </summary>
    /// <param name="input"></param>
    /// <param name="context"></param>
    /// <returns></returns>
    public async Task<string> FunctionHandler(string input, ILambdaContext context)
    {
        var mailRequest = JsonSerializer.Deserialize<MailRequest>(input);
        var email = new MimeMessage { Sender = MailboxAddress.Parse(mailRequest?.From) };
        email.From.Add(MailboxAddress.Parse(mailRequest?.From));
        email.To.Add(MailboxAddress.Parse(mailRequest?.To));
        email.Subject=mailRequest?.Subject;
        email.Body= new EmailBodyBuilder()
            .BuildEmailBody(mailRequest?.Body)
            .Build();

        using var smtp = new SmtpClient();
        
        await smtp.ConnectAsync(MailSettings.Host, MailSettings.Port,
            SecureSocketOptions.StartTls);
        await smtp.AuthenticateAsync(MailSettings.UserName, MailSettings.Password);
        return  await smtp.SendAsync(email);
    }
}

public static class MailSettings
{
    public static string EmailFrom => "xef786@gmail.com";
    public static string UserName => "xef786@gmail.com";
    public static string Password => "gxfqfmqeyenenodv";
    public static string Host => "smtp.gmail.com";
    public static int Port => 587;
}

public class EmailBodyBuilder
{
    private BodyBuilder builder;

    public EmailBodyBuilder()
    {
        builder = new BodyBuilder();
    }

    public EmailBodyBuilder BuildEmailBody(string body)
    {
        builder.HtmlBody = body;
        return this;
    }

    public MimeEntity Build()
    {
        return builder.ToMessageBody();
    }
}

public class MailRequest
{
    public MailRequest(string from, string to, string subject, string body)
    {
        From = from;
        To = to;
        Subject = subject;
        Body = body;
    }

    public string From { get; set; }
    public string To { get; set; }
    public string Subject { get; set; }
    public string Body { get; set; }
}