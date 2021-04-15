using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Text;

namespace Risfond.File.ConsulUnitity
{
  public static class ConsulExtensions
  {
    /// <summary>
    /// 注册服务
    /// </summary>
    /// <param name="services"></param>
    /// <param name="configuration"></param>
    /// <returns></returns>
    public static IServiceCollection AddConsulService(this IServiceCollection services,IConfiguration configuration)
    {
      services.Configure<ServiceRegistryConfig>(configuration.GetSection("ConsulRegistry"));
      services.AddSingleton<IServiceRegistry, ServiceRegistry>();
      return services;
    }
    /// <summary>
    /// 发现服务
    /// </summary>
    /// <param name="service"></param>
    /// <param name="configuration"></param>
    /// <returns></returns>
    public static IServiceCollection DiscoveryService(this IServiceCollection service,IConfiguration configuration)
    {
      service.Configure<ServiceDiscoveryConfig>(configuration.GetSection("ServiceDiscoveryConfig"));
      service.AddSingleton<IServiceDiscovery, ServiceDiscovery>();    
      //service.AddSingleton<ServiceDiscoveryConfig>(configuration.GetSection("ServiceDiscoveryConfig").Value);
      return service;
    }
  }
}
