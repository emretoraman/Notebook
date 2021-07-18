using Notebook.Infrastructure.Services;
using MsgReader.Mime;
using System;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Threading.Tasks;
using Xunit;

namespace Notebook.IntegrationTests.Infrastructure.Services
{
    public class EmailSenderTests
    {
        [Fact]
        public async Task SendEmailAsync_ByParameters_Sends()
        {
            MailMessage expected = new("from@test.com", "to@test.com", "test subject", "test body");

            string pickupDirectory = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());

            using (EmailSender sender = GetEmailSender(pickupDirectory))
            {
                await sender.SendEmailAsync(expected.From.Address, expected.To.Single().Address, expected.Subject, expected.Body);
            }

            string file = Directory.GetFiles(pickupDirectory).Single();

            Message actual = Message.Load(new FileInfo(file));
            
            string actualBody = actual.TextBody.BodyEncoding.GetString(actual.TextBody.Body);
            string newLine = "\r\n";
            if (actualBody.EndsWith(newLine))
            {
                actualBody = actualBody.Substring(0, actualBody.Length - newLine.Length);
            }

            Assert.Equal(expected.From.Address, actual.Headers.From.Address);
            Assert.Equal(expected.To.Single().Address, actual.Headers.To.Single().Address);
            Assert.Equal(expected.Subject, actual.Headers.Subject);
            Assert.Equal(expected.Body, actualBody);

            Directory.Delete(pickupDirectory, true);
        }

        private static EmailSender GetEmailSender(string pickupDirectory)
        {
            SmtpClient smtpClient = new()
            {
                DeliveryMethod = SmtpDeliveryMethod.SpecifiedPickupDirectory,
                PickupDirectoryLocation = pickupDirectory
            };
            return new EmailSender(smtpClient);
        }
    }
}
