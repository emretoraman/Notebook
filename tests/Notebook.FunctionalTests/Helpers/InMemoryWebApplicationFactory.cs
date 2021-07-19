using Notebook.Infrastructure.Data;
using Notebook.WebApi;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using System;
using System.Linq;

namespace Notebook.FunctionalTests.WebApi.Helpers
{
    public class InMemoryWebApplicationFactory : WebApplicationFactory<Startup>
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
            builder
                .UseSetting("https_port", "443")
                .ConfigureServices(services =>
                {
                    ServiceDescriptor descriptor = services.SingleOrDefault(d => d.ServiceType == typeof(DbContextOptions<EfCoreDbContext>));

                    if (descriptor != null)
                    {
                        services.Remove(descriptor);
                    }

                    string inMemoryCollectionName = Guid.NewGuid().ToString();

                    services.AddDbContext<EfCoreDbContext>(options => options.UseInMemoryDatabase(inMemoryCollectionName));
                });
        }
    }
}
