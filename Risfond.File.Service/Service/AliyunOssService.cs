using Aliyun.OSS;
using Risfond.File.Model.ConfigureModel;
using Risfond.File.Service.Interface;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Risfond.File.Service.Service
{
  public class AliyunOssService : IAliyunOssService
  {
    public AliyunOssConfig _ossConfig;
    //public AliyunOssService(AliyunOssConfig config)
    //{
    //  this._ossConfig = config;
    //}
    /// <summary>
    /// 上传文件获取访问Key
    /// </summary>
    /// <param name="name"></param>
    /// <param name="path"></param>
    /// <param name="fileStream"></param>
    /// <returns></returns>
    public string UploadImgReturnKey(string name,string path, MemoryStream fileStream)
    {
      var ossClient = new OssClient(_ossConfig.EndPoint, _ossConfig.AccessKey, _ossConfig.AccessSecret);
      var result=ossClient.PutObject(_ossConfig.Bucket,name,fileStream);
      return result.RequestId;
    }
    /// <summary>
    /// 上传文件获取文件URL
    /// </summary>
    /// <param name="name"></param>
    /// <param name="path"></param>
    /// <param name="fileStream"></param>
    /// <returns></returns>
    public string UploadImgReturnUrl(string name, string path, MemoryStream fileStream)
    {
      var requestKey = UploadImgReturnKey(name, path, fileStream);
      return GetViewUrl(requestKey);
    }
    /// <summary>
    /// 获取预览路径
    /// </summary>
    /// <param name="key"></param>
    /// <returns></returns>
    private string GetViewUrl(string key)
    {      
      return $"{_ossConfig.UrlForView}{key}";
    }

  }
}
