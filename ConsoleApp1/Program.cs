using IdentityServer4.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;

namespace ConsoleApp1
{
  class Program
  {
    private static readonly string _getTokenUrl = @"https://api.weixin.qq.com/cgi-bin/token?grant_type=client_credential&appid={0}&secret={1}";
    //private static readonly string _getQRCode = @"https://api.weixin.qq.com/wxa/getwxacode?access_token={0}";

    private static readonly string _getQRCode = @"https://api.weixin.qq.com/wxa/getwxacodeunlimit?access_token={0}";
    private static readonly object _obj = new object();
    private static int Num = 1;
    private static string name = string.Empty;
    static void Main(string[] args)
    {
      #region
      //var data = new List<ResumeCheckDate>
      //{
      //  new ResumeCheckDate { StartTime = new DateTime(2012, 8, 15), EndTime = new DateTime(2015, 6, 14) },
      //  new ResumeCheckDate { StartTime = new DateTime(2015, 8, 15), EndTime = new DateTime(2017, 6, 14) },
      //  new ResumeCheckDate { StartTime = new DateTime(2017, 4, 15), EndTime = new DateTime(2019, 6, 14) }
      //};
      //var expIterator = new ResumeExpIterator<ResumeCheckDate>(data.OrderBy(p => p.StartTime).ToArray());
      //int num = 0;
      //while(expIterator.HasNext())
      //{
      //  var currentDate = expIterator.GetValue();
      //  var nextDate = expIterator.Next();
      //  //if ((nextDate.StartTime - currentDate.EndTime).TotalDays > 30 * 1)
      //  if(nextDate.StartTime < currentDate.EndTime)
      //  {
      //    //Console.WriteLine($"在{nextDate.EndTime.ToString("yyyy-MM")}月至{currentDate.StartTime.ToString("yyyy-MM")}月 存在工作空档期");
      //    Console.WriteLine($"在{nextDate.StartTime.ToString("yyyy-MM")}月至{currentDate.EndTime.ToString("yyyy-MM")}月 存在工作重叠");
      //  }
      //  //num++;
      //};
      //Console.WriteLine("完成");
      //Console.ReadKey();
      #endregion

      Thread thread1 = new Thread(p=> {
        lock (_obj)
        {
          Thread.Sleep(1000);
          Num = 5;
        }  
      });
      Thread thread2 = new Thread(p => {
        lock (_obj)
        {
          Num = 6;
        }        
      });
      thread1.Start();
      thread2.Start();
      Thread.Sleep(1500);
      Console.Write(Num);
      Console.ReadKey();
    }
    private static readonly AutoResetEvent jsEvent = new AutoResetEvent(false);
    private static readonly AutoResetEvent osEvent = new AutoResetEvent(false);
    public static void PrinJs()
    {
      //jsEvent.WaitOne();
      for (int i = 1; i <= 100; i++)
      {
        if (i % 2 != 1) continue;
        Console.WriteLine($"{Thread.CurrentThread.Name}：{i}");
        osEvent.Set();
        jsEvent.WaitOne();
      }
    }
    public static void PrinOs()
    {
      osEvent.WaitOne();
      for (int i = 1; i <= 100; i++)
      {
        if (i % 2 != 0) continue;
        Console.WriteLine($"{Thread.CurrentThread.Name}：{i}");
        jsEvent.Set();
        osEvent.WaitOne();
      }
    }
    /// <summary>
    /// 获取token
    /// </summary>
    public static TokenResult GetAccessToken()
    {
      var url = string.Format(_getTokenUrl, "", "");
      var response = HttpHelper.CreateGetHttpResponse(url, null, null, null);
      Stream myResponseStream = response.GetResponseStream();
      StreamReader myStreamReader = new StreamReader(myResponseStream ?? throw new InvalidOperationException(), Encoding.GetEncoding("UTF-8"));     
      return JsonConvert.DeserializeObject<TokenResult>(myStreamReader.ReadToEnd());
    }
    public string SaveImage(MemoryStream stream, string name)
    {
      if (stream?.Length <= 0)
        return string.Empty; ;
      var path = $@"D:\Image\{name}";
      using (FileStream fileStream = System.IO.File.Create(path))
      {
        var fileBytes = stream.ToArray();
        fileStream.Write(fileBytes, 0, fileBytes.Length);
        fileStream.Close();
      }
      return path;
    }
    /// <summary>
    /// 获取二维码
    /// </summary>
    /// <param name="picParam"></param>
    /// <returns></returns>
    //public static byte[] GetPicBuffer(PicParam picParam)
    //{
    //  var tokenResult = CacheUtitlty.Get<TokenResult>("tokenResult");
    //  if (tokenResult == null || (tokenResult != null && tokenResult.access_token == null))
    //  {
    //    tokenResult = GetAccessToken(picParam);
    //    CacheUtitlty.Insert("tokenResult", tokenResult, 60 * 60);
    //  }

    //  byte[] buffer = GetCode(picParam, tokenResult);
    //  return buffer;
    //}

    /// <summary>
    /// 获取二维码字节流
    /// </summary>
    /// <param name="picParam"></param>
    /// <param name="tokenResult"></param>
    /// <returns></returns>
    //public static byte[] GetCode(PicParam picParam, TokenResult tokenResult)
    //{
    //  var postUrl = string.Format(_getQRCode, tokenResult.access_token);
    //  int width = picParam.PicWith ?? 280;//参数范围280--1280px
    //  var query = new { path = picParam.Page + picParam.Scene, width = width };
    //  using (HttpClient client = new HttpClient())
    //  {
    //    HttpContent content = new StringContent(JsonConvert.SerializeObject(query));
    //    content.Headers.ContentType = new MediaTypeHeaderValue("application/json");
    //    try
    //    {
    //      var httpResponse = client.PostAsync(postUrl, content).Result;
    //      if (httpResponse.IsSuccessStatusCode)
    //      {
    //        var result = httpResponse.Content.ReadAsByteArrayAsync().Result;
    //        return result;
    //      }
    //    }
    //    catch (Exception)
    //    {
    //      return null;
    //    }
    //  }
    //  return null;
    //}

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
  public class ResumeExpIterator<T> where T : class, new()
  {
    private T[] _data;
    private int _Length;
    public ResumeExpIterator(T[] data)
    {
      _data = data;
      _Length = data.Length;
    }
    private int _dataIndex = 0;
    public T GetValue()
    {
      if (_dataIndex > _Length)
        return null;
      //_dataIndex = index;
      return _data[_dataIndex];
    }
    public bool HasNext()
    {
      if (_dataIndex + 1 < _Length)
        return true;
      return false;
    }
    public T Next()
    {
      _dataIndex++;
      return _data[_dataIndex];
    }
  }
  public class ResumeCheckDate
  {
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }
  }
  public class TokenResult
  {
    /// <summary>
    /// 
    /// </summary>
    public string access_token { get; set; }

    /// <summary>
    /// 
    /// </summary>
    public int expires_in { get; set; }
  }

}
