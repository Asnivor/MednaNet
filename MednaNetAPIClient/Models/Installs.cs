using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MednaNetAPIClient.Models
{
    public class Installs
    {
        public int id { get; set; }
        public DateTime registeredOn { get; set; }
        public string code { get; set; }
        public bool banned { get; set; }
        public bool tempBan {get;set;}
        public DateTime? tempBanEnd { get; set; }
        public DateTime lastCheckin { get; set; }
        public string username { get; set; }
    }
}
