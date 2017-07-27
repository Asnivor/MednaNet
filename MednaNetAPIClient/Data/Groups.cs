using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MednaNetAPIClient.Data
{
    public class Groups
    {
        public int id { get; set; }
        public int groupOwner { get; set; }
        public string groupName { get; set; }
        public string groupDescription { get; set; }
    }
}
