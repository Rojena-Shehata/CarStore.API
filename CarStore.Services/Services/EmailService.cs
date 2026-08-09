using MimeKit;
using MailKit.Net.Smtp;
using CarStore.Shared.Settings;
using CarStore.ServicesAbstraction;
using Microsoft.Extensions.Options;
using Microsoft.Extensions.Logging;

namespace CarStore.Services.Services
{
    public class EmailService : IEmailService
    {
        private readonly SmtpSettings _smtpSettings;
        private readonly ILogger<EmailService> _logger;

        public EmailService(IOptions<SmtpSettings> options, ILogger<EmailService> logger)
        {
            _smtpSettings = options.Value;
            _logger = logger;
        }

        public async Task SendEmailAsync(string toEmail, string subject, string body, CancellationToken cancellationToken = default)
        {
            var mimeMessage = new MimeMessage();

            mimeMessage.From.Add(new MailboxAddress(_smtpSettings.SenderName, _smtpSettings.SenderEmail));
            mimeMessage.To.Add(MailboxAddress.Parse(toEmail));
            mimeMessage.Subject = subject;

            var bodyBuilder = new BodyBuilder
            {
                HtmlBody = body,
                TextBody = "Please use an HTML-compatible email client to view the content of this email."
            };
            mimeMessage.Body = bodyBuilder.ToMessageBody();

            using var client = new SmtpClient();

            try
            {
                var socketOption = _smtpSettings.Port == 465
                    ? MailKit.Security.SecureSocketOptions.SslOnConnect
                    : MailKit.Security.SecureSocketOptions.StartTls;

                await client.ConnectAsync(_smtpSettings.Host, _smtpSettings.Port, socketOption, cancellationToken);

                await client.AuthenticateAsync(_smtpSettings.UserName, _smtpSettings.Password, cancellationToken);
                await client.SendAsync(mimeMessage, cancellationToken);

                _logger.LogInformation("Email sent successfully to {ToEmail} with subject {Subject}", toEmail, subject);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to send email to {ToEmail} due to: {Message}", toEmail, ex.Message);
                throw;
            }
            finally
            {
                if (client.IsConnected)
                {
                    await client.DisconnectAsync(true, cancellationToken);
                }
            }
        }
    }
}