using MailKit.Security;
using MimeKit;
using Cinema.Domain;
using Cinema.Domain.DomainModels;
using Cinema.Services.Interface;
using System;
using System.Collections.Generic;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace Cinema.Services.Implementation
{
    public class EmailService : IEmailService
    {
        private readonly EmailSettings _emailSettings;
        public EmailService(EmailSettings emailSettings)
        {
            _emailSettings = emailSettings;
        }

        public async Task SendEmailAsync(List<EmailMessage> emailMessages)
        {
            List<MimeMessage> messages = new List<MimeMessage>();
            foreach (var item in emailMessages)
            {
                messages.Add(MailMessage(item));
            }
            try
            {
                using var smtp = new MailKit.Net.Smtp.SmtpClient();
                smtp.ServerCertificateValidationCallback = (s, c, h, e) => true;
                var socketOpt = _emailSettings.EnableSSL ? SecureSocketOptions.StartTls : SecureSocketOptions.Auto;
                await smtp.ConnectAsync(_emailSettings.SMTPServer, _emailSettings.SMTPPort, socketOpt);
                if (!string.IsNullOrEmpty(_emailSettings.SMTPUsername))
                {
                    await smtp.AuthenticateAsync(_emailSettings.SMTPUsername, _emailSettings.SMTPPass);
                }
                foreach (var item in messages)
                {
                    await smtp.SendAsync(item);
                }
                await smtp.DisconnectAsync(true);
            }
            catch (SmtpException ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        public MimeMessage MailMessage(EmailMessage item)
        {
            var emailMsg = new MimeMessage
            {
                Sender = new MailboxAddress(_emailSettings.SenderName, _emailSettings.SMTPUsername),
                Subject = item.subject
            };
            emailMsg.From.Add(new MailboxAddress(_emailSettings.EmailDisplayName, _emailSettings.SMTPUsername));
            emailMsg.Body = new TextPart(MimeKit.Text.TextFormat.Plain) { Text = item.content };
            emailMsg.To.Add(MailboxAddress.Parse(item.mailTo));
            return emailMsg;
        }
    }
}
