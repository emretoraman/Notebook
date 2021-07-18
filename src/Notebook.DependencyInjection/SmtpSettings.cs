using System.Net.Mail;

namespace Notebook.DependencyInjection
{
    public class SmtpSettings
    {
        public SmtpDeliveryMethod DeliveryMethod { get; set; }
        public string PickupDirectory { get; set; }
    }
}
