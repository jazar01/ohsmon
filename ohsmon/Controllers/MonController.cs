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
        private readonly MonitorContext _context;
        public MonController(MonitorContext context)
        {
            _context = context;

            if (_context.MonitorItems.Count()==0)
            {
                _context.MonitorItems.Add(new MonitorItem { ClientID = "INIT ENTRY" });
                _context.SaveChanges();
            }

        }


        // Note: monitor items can be submitted via GET or POST


    /// <summary>
    /// HTTP Post request to create a monitor item and record it
    ///     Data must be in body of request 
    ///     
    ///     example ( ClientID and ResponseTime are required)
    ///     
    ///    	{
	///     "ClientID":"Client1",
    ///     "Type":"ALM",
	///     "ResponseTime":210,
	///     "Memo":"Test memo data"
	///     }
    /// </summary>
    /// <param name="version"></param>
    /// <param name="monitorItem"></param>
    /// <returns></returns>
    [HttpPost("{version}")]
        public IActionResult Create(string version, [FromBody] MonitorItem monitorItem)
        {
            return RecordMonitorItem(version, monitorItem);   
        }


        // GET with no version is not allowed
        [HttpGet]
        public IActionResult GetTest()
        {
            return new ObjectResult("ERROR - API version must be specified");
        }

        /// <summary>
        /// HTTP GET request to add a monitor item to persistent storage
        /// Record Monitor Data
        /// </summary>
        /// <param name="version">V#</param>
        /// <param name="ClientID">ClientID</param>
        /// <param name="Type">Item type</param>
        /// <param name="ResponseTime">milliseconds</param>
        /// <param name="Memo"></param>
        /// <returns></returns>
        // http://localhost:60695/api/mon/V1/record?clientid=Client1&Type=ALM&ResponseTime=400&Memo=test%20data
        [HttpGet("{version}/record")]
        public IActionResult RecMonData(string version,
            [FromQuery] string ClientID,
            [FromQuery] string Type,
            [FromQuery] string ResponseTime,
            [FromQuery] string Memo)
        {
            if (uint.TryParse(ResponseTime, out uint rtime))
            {
                MonitorItem monitorItem = new Models.MonitorItem
                {
                    ClientID = ClientID,
                    Type = Type,
                    ResponseTime = rtime,
                    Memo = Memo
                };
                return RecordMonitorItem(version, monitorItem);
            }
            else
            {
                throw (new ArgumentException("Reponse time must be a positive number of milliseconds", "ResponseTime"));
            }   
        }

        /// <summary>
        /// HTTP GET request to retreive previously recorded data
        /// </summary>
        /// <param name="version">V#</param>
        /// <param name="ClientID">ClientID</param>
        /// <param name="Type">Item type</param>
        /// <param name="ResponseTime">milliseconds</param>
        /// <param name="Memo"></param>
        /// <returns></returns>
        // http://localhost:60695/api/mon/V1?clientid=Client1&Type=ALM&ResponseTime=400&Memo=test%20data
        [HttpGet("{version}")]
        public IActionResult GetMonData(string version,
            [FromQuery] string ClientID,
            [FromQuery] string Type,
            [FromQuery] string ResponseTime,
            [FromQuery] string Memo)
        {

            if (!Request.QueryString.HasValue)
            {
                // gets all records
                var item = _context.MonitorItems.ToList();
                return new ObjectResult(item);
            }
            else
            {
                // example of query on ClientID and Type
                using (_context)
                {
                    var item = (from cid in _context.MonitorItems
                                where cid.ClientID.Equals(ClientID)
                                select cid)
                               .Intersect
                               (from type in _context.MonitorItems
                                where type.Type.Equals(Type)
                                select type);

                    return new ObjectResult(item.ToList());
                }
            }
        }
        /// <summary>
        /// Checks for valid request, then records the new monitor item
        /// </summary>
        /// <param name="version"></param>
        /// <param name="monitorItem"></param>
        /// <returns></returns>
        private IActionResult RecordMonitorItem(string version, MonitorItem monitorItem)
        {
            if (version.ToUpper() == "V1")
            {
                if (monitorItem.IsValid())
                {
                    // * log to flat CSV file *
                    // FileDataStor fds = new FileDataStor(@"c:\temp\mondata.log");  //TODO hard coded filename for testing
                    // fds.AppendData(monitorItem.ToCSV());
                    // * log to database
                    _context.MonitorItems.Add(monitorItem);
                    _context.SaveChanges();

                    // * write to console 
                    Console.WriteLine(monitorItem.ToString());
                    return new ObjectResult(monitorItem.ToString());
                }
                else
                    return new ObjectResult(monitorItem.GetMsg());
            }
            else
                return new ObjectResult("ERROR - Version not supported");
        }
      
    }
}
