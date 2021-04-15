using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AppIoc
{
  public class Startup
  {
    public Startup(IConfiguration configuration)
    {
      Configuration = configuration;
    }

    public IConfiguration Configuration { get; }

    // This method gets called by the runtime. Use this method to add services to the container.
    public void ConfigureServices(IServiceCollection services)
    {

      services.AddControllers();
      services.AddScoped<IPerson,Student>();
      services.AddScoped<IPerson,Teacher>();
      //services.AddScoped(option =>
      //{
      //  Func<string, IPerson> accesor = key =>
      //  {
      //    if (key.Equals("Student"))
      //    {
      //      return option.GetService<Student>();
      //    }
      //    else if (key.Equals("Teacher"))
      //    {
      //      return option.GetService<Teacher>();
      //    }
      //    else
      //    {
      //      throw new ArgumentException($"Not Support key : {key}");
      //    }
      //  };
      //  return accesor;
      //});
    }

    // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
    public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
    {
      if (env.IsDevelopment())
      {
        app.UseDeveloperExceptionPage();
      }

      app.UseRouting();

      app.UseAuthorization();

      app.UseEndpoints(endpoints =>
      {
        endpoints.MapControllers();
      });
    }
  }
}
