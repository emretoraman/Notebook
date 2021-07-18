using Notebook.Core.Aggregates.NoteAggregate.Events;
using Notebook.Core.Interfaces;
using Notebook.Core.Services;
using Notebook.Infrastructure.Data;
using Notebook.Infrastructure.Services;
using Notebook.SharedKernel.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System.Net.Mail;

namespace Notebook.DependencyInjection
{
    public static class DependencyInjection
	{
		public static void AddCore(this IServiceCollection services)
		{
			services.AddMediatR(typeof(NoteUpdatedEvent).Assembly);
			services.AddScoped<INoteSearchService, NoteSearchService>();
		}

		public static void AddInfrastructure(this IServiceCollection services, string connectionString, SmtpSettings smtpSettings)
		{
			services.AddDbContext<EfCoreDbContext>(options =>
				options.UseSqlServer(
					connectionString,
					b => b.MigrationsAssembly(typeof(EfCoreDbContext).Assembly.FullName)
				)
			);

			services.AddScoped(typeof(IRepository<>), typeof(EfCoreRepository<>));

			services.AddTransient(services => new SmtpClient 
			{
				DeliveryMethod = smtpSettings.DeliveryMethod,
				PickupDirectoryLocation = smtpSettings.PickupDirectory
			});

			services.AddTransient<IEmailSender, EmailSender>();
		}
	}
}
