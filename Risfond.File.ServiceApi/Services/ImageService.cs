using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Grpc.Core;
using Microsoft.AspNetCore.Hosting;
using Risfond.File.Service.Interface;
using Risfond.File.Service.Service;
using Risfond.File.ServiceApi.Protos;
using static System.Net.Mime.MediaTypeNames;

namespace Risfond.File.ServiceApi.Services
{
  public class ImageService : ImageManager.ImageManagerBase
  {
    private IAliyunOssService _ossService;
    private IWebHostEnvironment _evn;
    public ImageService(IAliyunOssService service, IWebHostEnvironment evn)
    {
      this._ossService = service;
      this._evn = evn;
    }
    /// <summary>
    /// 上传图片
    /// </summary>
    /// <param name="requestStream">文件数据</param>
    /// <param name="context">请求信息</param>
    /// <returns></returns>
    //public override async Task<BaseResponse> UploadImage(IAsyncStreamReader<UploadImgRequest> requestStream, ServerCallContext context)
    //{
    //  try
    //  {
    //    if (requestStream == null)
    //      return await Task.FromResult(new BaseResponse { Success = false, Message = "提交数据文件为空", Status = 200 });
    //    var requestData = new List<UploadImgRequest>();
    //    while (await requestStream.MoveNext())
    //    {
    //      requestData.Add(requestStream.Current);
    //    }
    //    var fileName = requestData[0]?.Name;
    //    MemoryStream stream = new MemoryStream();
    //    requestData.OrderBy(x => x.Index).ToList().ForEach(x => x.FileStream.WriteTo(stream));
    //    var result = SaveImage(stream,fileName); //requestData[0].ReturnType == ImgReturnType.Key ? _ossService.UploadImgReturnKey(fileName, "", stream) : _ossService.UploadImgReturnUrl(fileName, "", stream);
    //    return await Task.FromResult(new BaseResponse { Success = true, Message = "上传成功", Status = 200, ImageUrl = result });
    //  }
    //  catch (Exception e)
    //  {
    //    return await Task.FromResult(new BaseResponse { Success = false, Message = $"上传失败:{e.Message}", Status = 500 });
    //  }
    //}
    /// <summary>
    /// 上传图片
    /// </summary>
    /// <param name="requestStream">文件数据</param>
    /// <param name="context">请求信息</param>
    /// <returns></returns>
    public override async Task<BaseResponse> UploadImage(IAsyncStreamReader<UploadImgRequest> requestStream, ServerCallContext context)
    {
      try
      {
        if (requestStream == null)
          return await Task.FromResult(new BaseResponse { Success = false, Message = "提交数据文件为空", Status = 200 });        
        string fileName = string.Empty;
        var path = $@"{_evn.WebRootPath}\Image\";
        FileStream fileStream = null;
        while (await requestStream.MoveNext())
        {
          if (string.IsNullOrWhiteSpace(fileName))
          {
            fileName = requestStream.Current.Name;
            path = $"{path}{fileName}";
          }
          if (fileStream == null)
            fileStream = System.IO.File.Create(path);
          MemoryStream stream = new MemoryStream();
          requestStream.Current.FileStream.WriteTo(stream);
          SaveImage(stream, fileStream);
        }
        if (fileStream != null)
        {
          fileStream.Close();
          fileStream.Dispose();
        }       
        return await Task.FromResult(new BaseResponse { Success = true, Message = "上传成功", Status = 200, ImageUrl = path });
      }
      catch (Exception e)
      {
        return await Task.FromResult(new BaseResponse { Success = false, Message = $"上传失败:{e.Message}", Status = 500 });
      }
    }
    public string SaveImage(MemoryStream stream, string name)
    {
      if (stream?.Length <= 0)
        return string.Empty; ;
      var path = $@"{_evn.WebRootPath}\Image\{name}";
      using (FileStream fileStream = System.IO.File.Create(path))
      {
        var fileBytes = stream.ToArray();
        fileStream.Write(fileBytes, 0, fileBytes.Length);
        fileStream.Close();
      }
      return path;
    }
    public void SaveImage(MemoryStream stream, FileStream fileStream)
    {

      //using (FileStream fileStream = System.IO.File.Create(path))
      //{
      var fileBytes = stream.ToArray();
      fileStream.Write(fileBytes, 0, fileBytes.Length);
      //fileStream.Close();
      //}
      // return path;
    }
  }
}
