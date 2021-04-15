using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Risfond.File.ConsulUnitity
{
  public interface IServiceDiscovery
  {
    Task<List<DiscoveryServiceData>> DiscoveryService(string serviceName);
  }
}
