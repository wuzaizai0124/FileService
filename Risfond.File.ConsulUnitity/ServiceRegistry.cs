using Consul;
using System;
using System.Collections.Generic;
using System.Text;

namespace Risfond.File.ConsulUnitity
{
  public class ServiceRegistry : IServiceRegistry
  {
    public void DeregisterService(ServiceRegistryConfig config)
    {

      // 1、创建consul客户端连接
      var consulClient = new ConsulClient(configuration =>
      {
        //1.1 建立客户端和服务端连接
        configuration.Address = new Uri(config.RegistryAddress);
      });

      // 2、注销服务
      consulClient.Agent.ServiceDeregister(config.Id);

      // 3、关闭连接
      consulClient.Dispose();
    }
    /// <summary>
    /// 注册服务
    /// </summary>
    /// <param name="config"></param>
    public void RegistryService(ServiceRegistryConfig config)
    {
      var client = new ConsulClient(option =>
      {
        option.Address = new Uri(config.RegistryAddress);
      });          
      var registration = new AgentServiceRegistration
      {
        ID = config.Id,
        Name = config.Name,
        Address = config.Address,
        Port = config.Port,
        Tags = config.Tags,
        Check = new AgentServiceCheck
        {
          // 3.1、consul健康检查超时间
          Timeout = TimeSpan.FromSeconds(10),
          // 3.2、服务停止5秒后注销服务
          DeregisterCriticalServiceAfter = TimeSpan.FromSeconds(5),
          // 3.3、consul健康检查地址
          HTTP = config.HealthCheckAddress,
          // 3.4 consul健康检查间隔时间
          Interval = TimeSpan.FromSeconds(10),
        }
      };
      client.Agent.ServiceRegister(registration).Wait();
      client.Dispose();
    }
  }
}
