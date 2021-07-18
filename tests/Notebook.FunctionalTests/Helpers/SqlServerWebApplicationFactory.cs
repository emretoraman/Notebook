using Notebook.Infrastructure.Data;
using Notebook.WebApi;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System.IO;
using System.Net.Mail;

namespace Notebook.FunctionalTests.WebApi.Helpers
{
    public class SqlServerWebApplicationFactory : WebApplicationFactory<Startup>
    {
        protected override IHost CreateHost(IHostBuilder builder)
        {
            IHost host = builder.Build();

            using (IServiceScope scope = host.Services.CreateScope())
            {
                EfCoreDbContext db = scope.ServiceProvider.GetRequiredService<EfCoreDbContext>();

                db.Database.EnsureCreated();
            }

            host.Start();

            return host;
        }

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureServices(services =>
            {
                services.AddTransient(services => new SmtpClient
                {
                    DeliveryMethod = SmtpDeliveryMethod.SpecifiedPickupDirectory,
                    PickupDirectoryLocation = Path.Combine(Path.GetTempPath(), GetType().FullName)
                });
            });
        }
    }
}
