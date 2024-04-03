using Business.Models.Mail;

namespace Business.Services;

public interface IMailService
{
    bool SendMail(MailData mailData);
    Task<bool> SendMailAsync(MailData mailData);

    bool SendHTMLMail(HTMLMailData htmlMailData);
    Task<bool> SendHTMLMailAsync(HTMLMailData htmlMailData);
}
