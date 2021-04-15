using Google.Protobuf;
using Grpc.Core;
using Grpc.Net.Client;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualBasic;
using Risfond.File.ConsulUnitity;
using Risfond.File.ServiceApi.Protos;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace TestService
{
  class Program
  {
    static async Task Main(string[] args)
    {
      #region
      //var service = new ServiceCollection();

      //var configuration = new ConfigurationBuilder().SetBasePath(Environment.CurrentDirectory)
      //                                  .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
      //                                  .Build();
      //service.AddOptions();
      //service.DiscoveryService(configuration);
      //IServiceProvider serviceProvider = service.BuildServiceProvider();
      //var disconverService = serviceProvider.GetService<IServiceDiscovery>();
      //var discoveryResult = await disconverService.DiscoveryService("FileService");
      //if (discoveryResult?.Count <= 0)
      //{
      //  Console.WriteLine("未发现服务");
      //  return;
      //}
      //Console.WriteLine($"Consul地址：{configuration.GetSection("ServiceDiscoveryConfig").Get<ServiceDiscoveryConfig>()?.ConsulAddress}");
      //Console.WriteLine($"服务地址：{discoveryResult.FirstOrDefault()?.Url}");
      //var channel = GrpcChannel.ForAddress(discoveryResult.FirstOrDefault()?.Url);
      ////不使用证书
      ////var httpHandler = new HttpClientHandler { ServerCertificateCustomValidationCallback=HttpClientHandler.DangerousAcceptAnyServerCertificateValidator};
      ////var httpClient = new HttpClient(httpHandler);
      //var cacert = File.ReadAllText("1_api.jiaoyc.cn_bundle.crt");
      //var ssl = new SslCredentials(cacert);
      //var channOptions = new List<ChannelOption>
      //      {
      //          new ChannelOption(ChannelOptions.SslTargetNameOverride,"api.jiaoyc.cn")
      //      };
      //var grpcChannelOptions = new GrpcChannelOptions();
      //var httpClient = new HttpClient();
      #endregion
      //var httpHandler = new HttpClientHandler();
      //var channel = GrpcChannel.ForAddress("https://api.jiaoyc.cn:443");
      var channel = GrpcChannel.ForAddress("https://106.12.205.124:8500");
      var client = new ImageManager.ImageManagerClient(channel);
      using (var result = client.UploadImage())
      {
        Console.WriteLine($"开始时间：{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}");
        var path = @"D:\软件\metronic_v4.5.4.zip";
        FileStream files = new FileStream(path, FileMode.Open);
        byte[] imgByte = new byte[files.Length];
        files.Read(imgByte, 0, imgByte.Length);
        files.Close();
        int btSize = imgByte.Length;
        int buffSize = 1024*100; //1M
        int lastBiteSize = btSize % buffSize;
        int currentTimes = 0;
        int loopTimes = btSize / buffSize;
        UploadImgRequest requestData=null;
        while (currentTimes <= loopTimes)
        {
          ByteString sbytes=null;
          if (currentTimes == loopTimes)
          {
            sbytes = ByteString.CopyFrom(imgByte, currentTimes * buffSize, lastBiteSize);
          }
          else
          {
            sbytes = ByteString.CopyFrom(imgByte, currentTimes * buffSize, buffSize);
          }         
          requestData = new UploadImgRequest { Name = "metronic_v4.5.4.zip", FileStream = sbytes, Path = "22", Index = currentTimes + 1, ReturnType = ImgReturnType.Url };        
          await result.RequestStream.WriteAsync(requestData);
          currentTimes++;
        }
        await result.RequestStream.CompleteAsync();
        var resoponse=await result;
        Console.WriteLine($"结束时间：{DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss")}");
        Console.WriteLine(resoponse.ImageUrl);
      }
      Console.ReadKey();
    }
  }
}
