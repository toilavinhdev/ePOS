using ePOS.Application.Common.Contracts;
using ePOS.Shared.ValueObjects;
using Microsoft.Extensions.Logging;
using MimeKit;

namespace ePOS.Infrastructure.Services;

public class MailService : IMailService
{
    private readonly MailConfig _mailConfig;
    private readonly ILogger<MailService> _logger;

    public MailService(AppSettings appSettings, ILogger<MailService> logger)
    {
        _logger = logger;
        _mailConfig = appSettings.MailConfig;
    }
    
    public async Task<bool> SendMailAsync(string toMail, string toName, string subject, string body)
    {
        var mimeMessage = new MimeMessage();
        mimeMessage.Sender = new MailboxAddress(_mailConfig.DisplayName, _mailConfig.Mail);
        mimeMessage.From.Add(new MailboxAddress(_mailConfig.DisplayName, _mailConfig.Mail));
        mimeMessage.To.Add(MailboxAddress.Parse(toMail));
        mimeMessage.Subject = subject;
        mimeMessage.Body = new TextPart("html") { Text = body };
        
        using var smtp = new MailKit.Net.Smtp.SmtpClient();
        
        try {
            _logger.LogInformation("Start send mail to {Mail}", toMail);
            await smtp.ConnectAsync(_mailConfig.Host, _mailConfig.Port, false);
            await smtp.AuthenticateAsync (_mailConfig.Mail, _mailConfig.Password);
            await smtp.SendAsync(mimeMessage);
            await smtp.DisconnectAsync(true);
            _logger.LogInformation("Send mail to {Mail} successfully", toMail);
            return true;
        }
        catch (Exception ex) {
            _logger.LogError("Send mail failed: {Error}", ex.Message);
            return false;
        }
    }
}