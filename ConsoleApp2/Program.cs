using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ConsoleApp2
{
  class Program
  {
    private static readonly object _obj = new object();
    private static int Num = 1;
    private static string name = string.Empty;
    static void Main(string[] args)
    {     
      Thread thread1 = new Thread(p => {
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
  }
}
