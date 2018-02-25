using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

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
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int RecordID { get; set; }
        public string ClientID { get; set; }
        [Column(TypeName="Date") ]
        public DateTime Date { get; set; }
        public TimeSpan Time { get; set; }
        public string Type { get; set; }
        public uint ResponseTime { get; set; }
        public string Memo { get; set; }

        private string _msg;
    
        /// <summary>
        /// constructor
        /// </summary>
        public MonitorItem()
        {
            Date = DateTime.Now.Date;
            Time = DateTime.Now.TimeOfDay;
            _msg = "";
        }

        public override string ToString()
        {
            return String.Format("ClientID={0}, Date={1:yyyy-MM-dd}, Time={2:hh\\:mm\\:ss}, Type={3}, ResponseTime={4}, Memo={5}",
                ClientID, Date, Time, Type, ResponseTime, Memo);
        }
        public string ToCSV()
        {
            return String.Format("{0},{1:yyyy-MM-dd},{2:hh\\:mm\\:ss},{3},{4},\"{5}\"",
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
            else if (ResponseTime < 1 || ResponseTime > 600000)  // ten minute limit
            {
                _msg += "Response time out of range   ";
                valid = false;
            }
            if (valid)
                _msg = "VALID";
            else
                _msg = "ERROR - " + _msg;

            return valid;
        }

        /// <summary>
        /// call this after IsValid if false to get error message
        /// </summary>
        /// <returns>message</returns>
        public string GetMsg()
        {
            return _msg;
        }

    }
}
