using Limak.Application.Abstractions.Services;
using Limak.Application.Dtos.EmailDtos;
using MailKit.Net.Smtp;
using Microsoft.Extensions.Configuration;
using MimeKit;

namespace Limak.Infrastructure.Implementations.Services;

public class EmailService : IEmailService
{
    private readonly IConfiguration _configuration;
    private readonly SmtpSettingsDto _settings;

    public EmailService(IConfiguration configuration)
    {
        _configuration = configuration;
        _settings = _configuration.GetSection("SmtpSettings").Get<SmtpSettingsDto>() ?? new();
    }

    public async Task SendEmailAsync(string email, string subject, string body)
    {
        var message = new MimeMessage();
        message.From.Add(new MailboxAddress(_settings.SenderName, _settings.SenderEmail));
        message.To.Add(new MailboxAddress(email, email));

        message.Subject = subject;
        message.Body = new TextPart("html")
        {
            Text = body
        };


        using var client = new SmtpClient();

        client.ServerCertificateValidationCallback = (x, y, z, i) => true;

        await client.ConnectAsync(_settings.Server, _settings.Port, true);


        await client.AuthenticateAsync(_settings.Username, _settings.Password);
        await client.SendAsync(message);
    }
}
