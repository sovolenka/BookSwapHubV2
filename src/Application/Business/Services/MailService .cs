using Business.Models.Mail;
using MailKit.Net.Smtp;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MimeKit;

namespace Business.Services;

public class MailService : IMailService
{
    private readonly MailSettings _mailSettings;

    public MailService(IOptions<MailSettings> mailSettingsOptions)
    {
        _mailSettings = mailSettingsOptions.Value;
    }

    public bool SendMail(MailData mailData)
    {
        try
        {
            using MimeMessage emailMessage = new();
            MailboxAddress emailFrom = new(_mailSettings.SenderName, _mailSettings.SenderEmail);
            emailMessage.From.Add(emailFrom);
            MailboxAddress emailTo = new(mailData.EmailToName, mailData.EmailToId);
            emailMessage.To.Add(emailTo);

            emailMessage.Subject = mailData.EmailSubject;

            BodyBuilder emailBodyBuilder = new()
            {
                TextBody = mailData.EmailBody
            };

            emailMessage.Body = emailBodyBuilder.ToMessageBody();
            //this is the SmtpClient from the Mailkit.Net.Smtp namespace, not the System.Net.Mail one
            using SmtpClient mailClient = new();
            mailClient.Connect(_mailSettings.Server, _mailSettings.Port, MailKit.Security.SecureSocketOptions.StartTls);
            mailClient.Authenticate(_mailSettings.UserName, _mailSettings.Password);
            mailClient.Send(emailMessage);
            mailClient.Disconnect(true);

            return true;
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
            return false;
        }
    }

    public async Task<bool> SendMailAsync(MailData mailData)
    {
        try
        {
            using MimeMessage emailMessage = new();
            MailboxAddress emailFrom = new(_mailSettings.SenderName, _mailSettings.SenderEmail);
            emailMessage.From.Add(emailFrom);
            MailboxAddress emailTo = new(mailData.EmailToName, mailData.EmailToId);
            emailMessage.To.Add(emailTo);

            emailMessage.Subject = mailData.EmailSubject;

            BodyBuilder emailBodyBuilder = new()
            {
                TextBody = mailData.EmailBody
            };

            emailMessage.Body = emailBodyBuilder.ToMessageBody();
            //this is the SmtpClient from the Mailkit.Net.Smtp namespace, not the System.Net.Mail one
            using SmtpClient mailClient = new();
            await mailClient.ConnectAsync(_mailSettings.Server, _mailSettings.Port, MailKit.Security.SecureSocketOptions.StartTls);
            await mailClient.AuthenticateAsync(_mailSettings.UserName, _mailSettings.Password);
            await mailClient.SendAsync(emailMessage);
            await mailClient.DisconnectAsync(true);

            return true;
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
            return false;
        }
    }

    public bool SendHTMLMail(MailHTMLData htmlMailData)
    {
        try
        {
            using MimeMessage emailMessage = new();
            MailboxAddress emailFrom = new(_mailSettings.SenderName, _mailSettings.SenderEmail);
            emailMessage.From.Add(emailFrom);

            MailboxAddress emailTo = new(htmlMailData.EmailToName, htmlMailData.EmailToId);
            emailMessage.To.Add(emailTo);

            emailMessage.Subject = htmlMailData.EmailSubject;

            BodyBuilder emailBodyBuilder = new()
            {
                HtmlBody = htmlMailData.EmailBodyHTML,
                TextBody = htmlMailData.EmailBody
            };

            emailMessage.Body = emailBodyBuilder.ToMessageBody();

            using SmtpClient mailClient = new();
            mailClient.Connect(_mailSettings.Server, _mailSettings.Port, MailKit.Security.SecureSocketOptions.StartTls);
            mailClient.Authenticate(_mailSettings.SenderEmail, _mailSettings.Password);
            mailClient.Send(emailMessage);
            mailClient.Disconnect(true);

            return true;
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
            return false;
        }
    }

    public async Task<bool> SendHTMLMailAsync(MailHTMLData htmlMailData)
    {
        try
        {
            using MimeMessage emailMessage = new();
            MailboxAddress emailFrom = new(_mailSettings.SenderName, _mailSettings.SenderEmail);
            emailMessage.From.Add(emailFrom);

            MailboxAddress emailTo = new(htmlMailData.EmailToName, htmlMailData.EmailToId);
            emailMessage.To.Add(emailTo);

            emailMessage.Subject = htmlMailData.EmailSubject;

            BodyBuilder emailBodyBuilder = new()
            {
                HtmlBody = htmlMailData.EmailBodyHTML,
                TextBody = htmlMailData.EmailBody
            };

            emailMessage.Body = emailBodyBuilder.ToMessageBody();

            using SmtpClient mailClient = new();
            await mailClient.ConnectAsync(_mailSettings.Server, _mailSettings.Port, MailKit.Security.SecureSocketOptions.StartTls);
            await mailClient.AuthenticateAsync(_mailSettings.SenderEmail, _mailSettings.Password);
            await mailClient.SendAsync(emailMessage);
            await mailClient.DisconnectAsync(true);

            return true;
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
            return false;
        }
    }
}
