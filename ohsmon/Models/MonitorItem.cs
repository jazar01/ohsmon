using System;
using System.ComponentModel.DataAnnotations;

using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Text;
using System.Globalization;

namespace ohsmon.Models
{
    /// <summary>
    ///     MonitorItem - data posted in reponse to a monitoring event.
    /// </summary>
    public class MonitorItem
    {
        [Required]
        public string ClientID { get; set; }
        public string Date { get; }
        public string Time { get;  }
        public string Type { get; set; }
        public string ResponseTime { get; set; }
        public string Memo { get; set; }

        private string _msg;
    
        /// <summary>
        /// constructor
        /// </summary>
        public MonitorItem()
        {
            Date = DateTime.Now.ToShortDateString();
            Time = DateTime.Now.ToString("T",CultureInfo.CreateSpecificCulture("es-ES"));
            _msg = "";
        }

        public override string ToString()
        {
            return String.Format("ClientID={0}, Date={1}, Time={2}, Type={3}, ResponseTime={4}, Memo={5}",
                ClientID, Date, Time, Type, ResponseTime, Memo);
        }
        public string ToCSV()
        {
            return String.Format("{0},{1},{2},{3},{4},\"{5}\"",
                ClientID, Date, Time, Type, ResponseTime, Memo);
        }

        /// <summary>
        /// test for valid data in item
        /// </summary>
        /// <returns>boolean</returns>
        public bool IsValid()
        {
            bool valid = true;
            _msg = "";
            if (string.IsNullOrWhiteSpace(ClientID))
            {
                _msg += "ClientID not valid   ";
                valid = false;
            }
            if (string.IsNullOrWhiteSpace(Type))
            {
                _msg += "Type not valid   ";
                valid = false;
            }

            if (!long.TryParse(ResponseTime, out long rtime))
                {
                _msg += "Response time not valid   ";
                valid = false;
            }
            else if (rtime < 1 || rtime > 600000)  // ten minute limit
            {
                _msg += "Response time out of range   ";
                valid = false;
            }
            if (valid)
                _msg = "VALID";

            return valid;
        }

        /// <summary>
        /// call this after IsValid if false to get error message
        /// </summary>
        /// <returns>message</returns>
        public string getMsg()
        {
            return _msg;
        }

    }
}
