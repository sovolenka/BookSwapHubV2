using Business.Models.Mail;
using MailKit.Net.Smtp;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Options;
using MimeKit;

namespace Business.Services;

public class MailService : IEmailSender
{
    private readonly MailSettings _mailSettings;

    public MailService(IOptions<MailSettings> mailSettingsOptions)
    {
        _mailSettings = mailSettingsOptions.Value;
    }

    public async Task SendEmailAsync(string email, string subject, string message)
    {
        try
        {
            using MimeMessage emailMessage = new();
            MailboxAddress emailFrom = new(_mailSettings.SenderName, _mailSettings.SenderEmail);
            emailMessage.From.Add(emailFrom);
            MailboxAddress emailTo = new(email, email);
            emailMessage.To.Add(emailTo);

            emailMessage.Subject = subject;

            BodyBuilder emailBodyBuilder = new()
            {
                TextBody = message
            };

            emailMessage.Body = emailBodyBuilder.ToMessageBody();
            //this is the SmtpClient from the Mailkit.Net.Smtp namespace, not the System.Net.Mail one
            using SmtpClient mailClient = new();
            await mailClient.ConnectAsync(_mailSettings.Server, _mailSettings.Port, MailKit.Security.SecureSocketOptions.StartTls);
            await mailClient.AuthenticateAsync(_mailSettings.UserName, _mailSettings.Password);
            await mailClient.SendAsync(emailMessage);
            await mailClient.DisconnectAsync(true);
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex);
        }
    }
}
