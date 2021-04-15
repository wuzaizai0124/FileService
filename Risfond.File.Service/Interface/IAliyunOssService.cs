using Risfond.File.Model;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Risfond.File.Service.Interface
{
  public interface IAliyunOssService
  {
    string UploadImgReturnKey(string name,string path, MemoryStream fileStream);
    string UploadImgReturnUrl(string name, string path, MemoryStream fileStream);
  }
}
