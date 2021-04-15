using System;
using System.Collections.Generic;
using System.Text;

namespace Risfond.File.ConsulUnitity
{
  public interface IServiceRegistry
  {
    /// <summary>
    /// 注册服务
    /// </summary>
    /// <param name="config"></param>
    void RegistryService(ServiceRegistryConfig config);
    /// <summary>
    /// 撤销服务
    /// </summary>
    /// <param name="config"></param>
    void DeregisterService(ServiceRegistryConfig config);    
  }
}
