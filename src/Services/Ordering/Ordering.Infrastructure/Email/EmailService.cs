using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Ordering.Application.Contracts.Infrastructure;
using Ordering.Application.Models;
using SendGrid;
using SendGrid.Helpers.Mail;
using System;
using System.Threading.Tasks;

namespace Ordering.Infrastructure.Email
{
    public class EmailService : IEmailService
    {
        public EmailSettings _emailSettings;
        public ILogger<EmailService> _logger;
        public EmailService(IOptions<EmailSettings> emailSettings,ILogger<EmailService> logger)
        {
            _emailSettings = emailSettings.Value;
            _logger = logger;
        }
        public async Task<bool> SendEmail(Application.Models.Email email)
        {
            var client = new SendGridClient(_emailSettings.APIKey);

            var subject = email.Subject;
            var body = email.Body;
            var to=new EmailAddress(email.To);

            var from=new EmailAddress { Email=_emailSettings.FromAddress,Name=_emailSettings.FromName};

            var sendGridMessage = MailHelper.CreateSingleEmail(from, to, subject, body, body);
            var response=await client.SendEmailAsync(sendGridMessage);

            if (response.IsSuccessStatusCode)
            {
                return true;
            }
            return false;
        }
    }
}
