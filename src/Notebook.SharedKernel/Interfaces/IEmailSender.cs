using System;
using System.Threading;
using System.Threading.Tasks;

namespace Notebook.SharedKernel.Interfaces
{
    public interface IEmailSender : IDisposable
	{
		Task SendEmailAsync(string from, string to, string subject, string body, CancellationToken cancellationToken = default);
	}
}
