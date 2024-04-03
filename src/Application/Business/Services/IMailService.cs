using Business.Models.Mail;

namespace Business.Services;

public interface IMailService
{
    bool SendMail(MailData mailData);
    Task<bool> SendMailAsync(MailData mailData);

    bool SendHTMLMail(MailHTMLData htmlMailData);
    Task<bool> SendHTMLMailAsync(MailHTMLData htmlMailData);
}
