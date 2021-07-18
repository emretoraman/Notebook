using Notebook.SharedKernel.Interfaces;
using System;
using System.IO;
using System.Net.Mail;
using System.Threading;
using System.Threading.Tasks;

namespace Notebook.Infrastructure.Services
{
    public class EmailSender : IEmailSender
    {
        private readonly SmtpClient _smtpClient;

        public EmailSender(SmtpClient smtpClient)
        {
            _smtpClient = smtpClient;

            if (smtpClient.DeliveryMethod == SmtpDeliveryMethod.SpecifiedPickupDirectory)
            {
                Directory.CreateDirectory(smtpClient.PickupDirectoryLocation);
            }
        }

        public void Dispose()
        {
            _smtpClient.Dispose();
            GC.SuppressFinalize(this);
        }

        public async Task SendEmailAsync(string from, string to, string subject, string body, CancellationToken cancellationToken = default)
        {
            MailMessage message = new(from, to, subject, body);

            await _smtpClient.SendMailAsync(message, cancellationToken);
        }
    }
}
