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
        // sample url to record data using get request
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



        // GET with no version or parameters returns last 100 records
        [HttpGet]
        public IActionResult GetTest()
        {
            IQueryable<MonitorItem> items = _context.MonitorItems;
            items = items.OrderByDescending(item => item.Date).ThenByDescending(item => item.Time);
            items = items.Take(100); 
            return new ObjectResult(items.ToList());
        }


        /// <summary>
        /// HttpGet returns records matchng parameters
        /// </summary>
        /// <param name="version">V#</param>
        /// <param name="ClientID">ClientID</param>
        /// <param name="Type">Type</param>
        /// <param name="Memo">records wich contain memo</param>
        /// <param name="Days">days of records</param>
        /// <param name="Records">number of records</param>
        /// <returns></returns>
        // http://localhost:60695/api/mon/V1?clientid=Client1&Type=ALM&Memo=test&days=10&records=100
        [HttpGet("{version}")]
        public IActionResult GetMonData(string version,
            [FromQuery] string ClientID,
            [FromQuery] string Type,
            [FromQuery] string Memo,
            [FromQuery] string Days,
            [FromQuery] string Records)
        {
            int records = 5000;   // default number of records to fetch

            IQueryable<MonitorItem> items = _context.MonitorItems;

            if (Request.QueryString.HasValue)
            {
                // set limit on number of records to return
                if (int.TryParse(Records, out int irecords))
                    if (irecords > 0) records = irecords;

                // how many days back to look
                if (int.TryParse(Days, out int idays))
                {
                    DateTime fromdate = DateTime.Now.AddDays(-idays);
                    items = from item in _context.MonitorItems
                            where (item.Date >= fromdate)
                            select item;
                }

                // specific ClientID
                if (!string.IsNullOrWhiteSpace(ClientID))
                    items = items.Intersect
                             (from item in _context.MonitorItems
                             where item.ClientID.Equals(ClientID)
                             select item);

                // Specific Type
                if (!string.IsNullOrWhiteSpace(Type))
                    items = items.Intersect
                            (from item in _context.MonitorItems
                            where item.Type.Equals(Type)
                            select item);

                // Memo contains
                if (!string.IsNullOrWhiteSpace(Memo))
                    items = items.Intersect
                            (from item in _context.MonitorItems
                             where item.Memo.Contains(Memo)
                             select item);
            }

            // sort the records in descending order
            items = items.OrderByDescending(item => item.Date).ThenByDescending(item => item.Time);
            items = items.Take(records);  // how many records to take

            // this is where the query actually executes
            return new ObjectResult(items.ToList());
               
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
