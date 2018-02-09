using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using ohsmon.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860
// https://docs.microsoft.com/en-us/aspnet/core/tutorials/first-web-api
// https://www.sourceallies.com/2016/12/build-a-restful-service-with-dotnet-core/
// https://docs.microsoft.com/en-us/aspnet/core/mvc/controllers/routing

namespace ohsmon.Controllers
{
    [Route("api/[controller]")]
    public class MonController : Controller
    {
        [HttpGet]
        public IActionResult GetTest()
        {
            return new ObjectResult("TEST RETURN DATA");
        }

        // http://localhost:60695/api/mon/V1?id=Client1&Type=ALM&ResponseTime=400&Memo=test%20data
        [HttpGet("{version}")]
        public IActionResult GetMonData(string version, 
            [FromQuery] string ID, 
            [FromQuery] string Type, 
            [FromQuery] string ResponseTime, 
            [FromQuery] string Memo)
        {
            MonitorItem monitorItem = new Models.MonitorItem();
            monitorItem.ClientID = ID;
            monitorItem.Type = Type;
            monitorItem.ResponseTime = ResponseTime;
            monitorItem.Memo = Memo;

            if (version.ToUpper() == "V1")
            {
                if (monitorItem.IsValid())
                {
                    DataRecorder r = new DataRecorder(@"c:\temp\mondata.log");
                    r.AppendData(monitorItem.ToCSV());
                    Console.WriteLine(monitorItem.ToCSV());
                    return new ObjectResult(monitorItem.ToString());
                }
                else
                    return new ObjectResult(monitorItem.getMsg());
            }
            else
                return new ObjectResult("ERROR - Version not supported");
        }

        // POST api/<controller>
        [HttpPost]
        public void Create([FromBody]string value)
        {
            string testval = value;
            return;
        }

       

      
    }
}
