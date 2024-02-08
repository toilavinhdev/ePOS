namespace ePOS.Application.Common.Contracts;

public interface IMailService
{
    Task<bool> SendMailAsync(string toMail, string toName, string subject, string body);
}