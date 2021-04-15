using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AppIoc
{
  public interface IPerson
  {
    string Hello();
  }
  public class Student : IPerson
  {
    public string Hello()
    {
      return "我是一个学生";
    }
  }
  public class Teacher : IPerson
  {
    public string Hello()
    {
      return "我是一个教师";
    }
  }
}
