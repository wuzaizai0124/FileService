using Consul;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Risfond.File.ConsulUnitity
{
  public class ServiceDiscovery : IServiceDiscovery
  {
    //private readonly IConfiguration _configuration;
    private readonly ServiceDiscoveryConfig _configuration;
    public ServiceDiscovery(IOptions<ServiceDiscoveryConfig> configuration)
    {
      _configuration = configuration.Value;
    }
    /// <summary>
    /// 服务发现
    /// </summary>
    /// <param name="serviceName"></param>
    /// <returns></returns>
    public async Task<List<DiscoveryServiceData>> DiscoveryService(string serviceName)
    {
      if (string.IsNullOrWhiteSpace(serviceName))
        return null;
      var config = _configuration;// _configuration.GetSection("ServiceDiscoveryConfig").Get<ServiceDiscoveryConfig>();
      //建立连接
      var client = new ConsulClient(option =>
      {
        option.Address = new Uri(config.ConsulAddress);
      });
      var services = await client.Catalog.Service(serviceName);
      var result = services.Response.Select(p => new DiscoveryServiceData
      {
        ServiceName = serviceName,
        Url = $"{p.ServiceAddress}:{p.ServicePort}"
      }).ToList();
      return result;
    }
  }
}
