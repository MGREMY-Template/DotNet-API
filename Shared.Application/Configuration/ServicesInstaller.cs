namespace Shared.Application.Configuration;

using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Shared.Core.Attributes;
using Shared.Core.Configuration;

[ConfigOrder(1)]
public class ServicesInstaller : IServiceInstaller
{
    public void Configure(IServiceCollection services, IConfiguration configuration) => services.AddMediatR(conf => conf.RegisterServicesFromAssembly(typeof(Shared.Application.Marker).Assembly));
}
