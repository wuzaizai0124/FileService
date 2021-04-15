using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AppIoc.Controllers
{
  [Route("api/[controller]")]
  [ApiController]
  public class PersonController : ControllerBase
  {
    private IPerson _teacher;
    private IPerson _student;
    private IEnumerable<IPerson> _person;
    public PersonController(IEnumerable<IPerson> person)
    {
      _person = person;
    }
    [HttpGet]
    [Route("say")]
    public string SayHello()
    {
      var str = $"{_person.FirstOrDefault()?.Hello()},{_person.Skip(1).FirstOrDefault()?.Hello()}";
      return str;
    }
  }
}
