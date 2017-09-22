using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MednaNetAPIClient.Models
{
    public class Users
    {
        public string username { get; set; }
        public int id { get; set; }
        public string discordId { get; set; }
        public bool isOnline { get; set; }

        public string userId
        {
            get
            {
                if(discordId == "0" || discordId == "")
                {
                    return id.ToString();
                }
                else
                {
                    return discordId;
                }
            }
        }
    }
}
