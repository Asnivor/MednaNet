using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace MednaNetAPI.Models
{
    public static class Installs
    {
        public static void checkinInstall(string installKey)
        {
            using (Models.MedLaunchChatEntities db = new Models.MedLaunchChatEntities())
            {
                var install = (from q in db.installs
                               where q.code == installKey
                               select q).FirstOrDefault();

                install.last_checkin = DateTime.Now;

                db.SaveChanges();
            }
        }
    }
}