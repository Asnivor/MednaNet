using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MednaNetAPIClient.Models
{
    public class Messages
    {
        public int id { get; set; }
        public string code { get; set; }
        public string message { get; set; }
        public DateTime postedOn { get; set; }
        public string name { get; set; }
        public int channel { get; set; }
    }
}
