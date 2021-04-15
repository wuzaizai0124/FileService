using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Risfond.File.ConsulUnitity;
using Risfond.File.Model.ConfigureModel;
using Risfond.File.Polly;
using Risfond.File.Service.Interface;
using Risfond.File.Service.Service;
using Risfond.File.ServiceApi.Services;
using System.Net;
using System.Net.Http;
namespace Risfond.File.ServiceApi
{
  public class Startup
  {
    public Startup(IWebHostEnvironment env)
    {
      var build = new ConfigurationBuilder()
         .SetBasePath(env.ContentRootPath)
         .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
         .AddJsonFile($"appsettings.{env.EnvironmentName}.json", optional: true)
         .AddEnvironmentVariables();
      Configuration = build.Build();
    }
    // This method gets called by the runtime. Use this method to add services to the container.
    // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
    private IConfiguration Configuration;
    public void ConfigureServices(IServiceCollection services)
    {
      // 1、自定义异常处理(用缓存处理)
      var fallbackResponse = new HttpResponseMessage
      {
        Content = new StringContent("系统正繁忙，请稍后重试"),// 内容，自定义内容
        StatusCode = HttpStatusCode.GatewayTimeout // 504
      };
      var pollyConfig = Configuration.GetSection("PollyConfig").Get<PollyConfig>();
      services.AddPolly("mrico", options =>
      {
        options.TimeoutTime = pollyConfig.TimeoutTime; // 1、超时时间
        options.RetryCount = pollyConfig.RetryCount;// 2、重试次数
        options.CircuitBreakerOpenFallCount = pollyConfig.CircuitBreakerOpenFallCount;// 3、熔断器开启(多少次失败开启)
        options.CircuitBreakerDownTime = pollyConfig.CircuitBreakerDownTime;// 4、熔断器开启时间
        options.httpResponseMessage = fallbackResponse;// 5、降级处理
      });
      services.AddGrpc();
      services.AddControllers();
      services.AddScoped<IAliyunOssService, AliyunOssService>(option => new AliyunOssService
      {
        _ossConfig = new AliyunOssConfig()
      });
      services.AddConsulService(Configuration);
    }

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
      if (env.IsDevelopment())
      {
        app.UseDeveloperExceptionPage();
      }
      //app.Map("/HealthCheck", HealthMap);
      app.UseRouting();
      app.UseStaticFiles();
      //app.UseConsulRegister();
      //app.UseMvc();
      app.UseEndpoints(endpoints =>
      {
        endpoints.MapControllerRoute("default","{controller=Home}/{action=Index}/{id?}");
        endpoints.MapGrpcService<GreeterService>();
        endpoints.MapGrpcService<ImageService>();
        endpoints.MapGrpcService<HealthCheckService>();       
      });
    }   
  }
}
