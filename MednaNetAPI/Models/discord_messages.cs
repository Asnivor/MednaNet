//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace MednaNetAPI.Models
{
    using System;
    using System.Collections.Generic;
    
    public partial class discord_messages
    {
        public int id { get; set; }
        public System.DateTime posted_on { get; set; }
        public string code { get; set; }
        public string name { get; set; }
        public string message { get; set; }
        public int channel { get; set; }
        public bool clients_ignore { get; set; }
        public string discord_user_id { get; set; }
    
        public virtual discord_channels discord_channels { get; set; }
    }
}
