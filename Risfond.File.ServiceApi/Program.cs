using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

namespace Risfond.File.ServiceApi
{
  public class Program
  {
    public static void Main(string[] args)
    {
      CreateHostBuilder(args).Build().Run();
    }

    // Additional configuration is required to successfully run gRPC on macOS.
    // For instructions on how to configure Kestrel and gRPC clients on macOS, visit https://go.microsoft.com/fwlink/?linkid=2099682
    public static IHostBuilder CreateHostBuilder(string[] args) =>
        Host.CreateDefaultBuilder(args)
            .ConfigureWebHostDefaults(webBuilder =>
            {
              var configuration = new ConfigurationBuilder().SetBasePath(Environment.CurrentDirectory)
                                          .AddJsonFile("host.json")
                                          .Build();
              var url = configuration["urls"];
              webBuilder.UseStartup<Startup>()
               .UseKestrel(
               // option =>
               //{
               //  option.Listen(System.Net.IPAddress.Any, 443, listenOptions =>
               //  {
               //    listenOptions.UseHttps(AppContext.BaseDirectory + @"\api.jiaoyc.cn.pfx", "1091674840");
               //    //listenOptions.UseHttps(AppContext.BaseDirectory + @"\localhost.pfx", "109167");
               //  });
               //}
               )
              .UseUrls(url)
              .UseIIS()
              .UseIISIntegration();                           
            });
  }
}
