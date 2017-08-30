using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;


namespace MednaNetAPI.Controllers
{

    

    public class APIController : ApiController
    {
        //http://www.vinaysahni.com/best-practices-for-a-pragmatic-restful-api#restful



        

        [Route("api/v1/installs")]
        [HttpPost]
        public IHttpActionResult CreateInstall()
        {
            string guid = Guid.NewGuid().ToString();

            MednaNetAPIClient.Models.Installs install = null;

            using (Models.MedLaunchChatEntities db = new Models.MedLaunchChatEntities())
            {
                Models.install newInstall = new Models.install();
                newInstall.banned = false;
                newInstall.code = guid;
                newInstall.last_checkin = DateTime.Now;
                newInstall.registered_on = DateTime.Now;
                newInstall.username = "";

                db.installs.Add(newInstall);
                db.SaveChanges();

                install = (from q in db.installs
                           where q.code == guid
                           select new MednaNetAPIClient.Models.Installs()
                           {
                               id = q.id,
                               banned = q.banned,
                               code = q.code,
                               lastCheckin = q.last_checkin,
                               registeredOn = q.registered_on,
                               tempBan = q.temp_ban,
                               tempBanEnd = q.temp_ban_end,
                               username = q.username
                           }).FirstOrDefault();
            }

            return Ok(install);
        }

        [Route("api/v1/installs")]
        [HttpPut]
        public IHttpActionResult UpdateInstall(MednaNetAPIClient.Models.Installs install)
        {
            IEnumerable<string> headerValues = Request.Headers.GetValues("Authorization");
            string installKey = headerValues.FirstOrDefault();
            Models.Installs.checkinInstall(installKey);

            using (Models.MedLaunchChatEntities db = new Models.MedLaunchChatEntities())
            {
                var dbInstall = (from q in db.installs
                               where q.code == installKey
                               select q).FirstOrDefault();

                dbInstall.username = install.username;

                db.SaveChanges();
            }

            return Ok();
        }

        [Route("api/v1/installs")]
        [HttpGet]
        public IHttpActionResult GetInstall()
        {
            IEnumerable<string> headerValues = Request.Headers.GetValues("Authorization");
            string installKey = headerValues.FirstOrDefault();

            MednaNetAPIClient.Models.Installs install = null;

            using (Models.MedLaunchChatEntities db = new Models.MedLaunchChatEntities())
            {
                install = (from q in db.installs
                           where q.code == installKey
                           select new MednaNetAPIClient.Models.Installs()
                           {
                               id = q.id,
                               banned = q.banned,
                               code = q.code,
                               lastCheckin = q.last_checkin,
                               registeredOn = q.registered_on,
                               tempBan = q.temp_ban,
                               tempBanEnd = q.temp_ban_end,
                               username = q.username
                           }).FirstOrDefault();
            }

            return Ok(install);
        }


        [Route("api/v1/users")]
        [HttpGet]
        public IHttpActionResult GetCheckedInUsers()
        {
            IEnumerable<string> headerValues = Request.Headers.GetValues("Authorization");
            string installKey = headerValues.FirstOrDefault();

            List<MednaNetAPIClient.Models.Users> users = null;

            
            using (Models.MedLaunchChatEntities db = new Models.MedLaunchChatEntities())
            {
                users = (from q in db.installs
                            where System.Data.Entity.DbFunctions.AddMinutes(q.last_checkin, 10) > System.Data.Entity.DbFunctions.AddMinutes(DateTime.Now, -10) && q.code != "botInstallKey"
                            select new MednaNetAPIClient.Models.Users()
                            {
                                id = q.id,
                                username = q.username
                            }).ToList();
            }
          

            return Ok(users);
        }


        [Route("api/v1/installs/checkin")]
        [HttpGet]
        public IHttpActionResult CheckinInstall()
        {
            

            IEnumerable<string> headerValues = Request.Headers.GetValues("Authorization");
            string installKey = headerValues.FirstOrDefault();
            Models.Installs.checkinInstall(installKey);

            using (Models.MedLaunchChatEntities db = new Models.MedLaunchChatEntities())
            {
                var install = (from q in db.installs
                               where q.code == installKey
                               select q).FirstOrDefault();

                install.last_checkin = DateTime.Now;

                db.SaveChanges();
            }

            return Ok();

        }

        [Route("api/v1/groups")]
        [HttpGet]
        public IHttpActionResult GetGroups()
        {
            IEnumerable<string> headerValues = Request.Headers.GetValues("Authorization");
            string installKey = headerValues.FirstOrDefault();
            Models.Installs.checkinInstall(installKey);

            List<MednaNetAPIClient.Models.Groups> groups = null;

            using (Models.MedLaunchChatEntities db = new Models.MedLaunchChatEntities())
            {
                var install = (from q in db.installs
                               where q.code == installKey
                               select q).FirstOrDefault();

                if (install != null)
                {
                    groups = (from q in db.group_members
                              where q.install_id == install.id
                              select new MednaNetAPIClient.Models.Groups()
                              {
                                  groupDescription = q.@group.group_description,
                                  groupName = q.@group.group_name,
                                  groupOwner = q.@group.group_owner,
                                  id = q.id
                              }).ToList();
                }
            }

            return Ok(groups.ToList());
        }

        [Route("api/v1/groups")]
        [HttpPost]
        public IHttpActionResult CreateGroup(MednaNetAPIClient.Models.Groups group)
        {
            var APIReturn = new Models.APIReturn();

            IEnumerable<string> headerValues = Request.Headers.GetValues("Authorization");
            string installKey = headerValues.FirstOrDefault();
            Models.Installs.checkinInstall(installKey);


            using (Models.MedLaunchChatEntities db = new Models.MedLaunchChatEntities())
            {
                var install = (from q in db.installs
                               where q.code == installKey
                               select q).FirstOrDefault();

                if (install != null)
                {
                    Models.group newGroup = new Models.group();
                    newGroup.group_description = group.groupDescription;
                    newGroup.group_name = group.groupName;
                    newGroup.group_owner = install.id;

                    if (newGroup.group_name.Length > 20)
                    {
                        newGroup.group_name = newGroup.group_name.Substring(0, 19);
                    }

                    db.groups.Add(newGroup);

                    try
                    {
                        db.SaveChanges();
                        APIReturn.returnMessage = "Group created.";
                    }
                    catch (Exception e)
                    {
                        APIReturn.returnMessage = "Could not create group.";
                        return new System.Web.Http.Results.ExceptionResult(e, this);
                    }

                    Models.group_members groupMember = new Models.group_members();
                    groupMember.awaiting_invite_confrim = false;
                    groupMember.group_id = newGroup.id;
                    groupMember.install_id = install.id;
                    db.group_members.Add(groupMember);


                    try
                    {
                        db.SaveChanges();
                        APIReturn.returnMessage += "| Install added to group";
                    }
                    catch (Exception e)
                    {
                        APIReturn.returnMessage = "Could not add install to group.";
                        return new System.Web.Http.Results.ExceptionResult(e, this);
                    }
                }
                else
                {
                    APIReturn.returnMessage = "Install not found";
                }
            }

            return Ok();
        }

        [Route("api/v1/groups/{id}")]
        [HttpPost]
        public IHttpActionResult GetGroupById(int id)
        {
            IEnumerable<string> headerValues = Request.Headers.GetValues("Authorization");
            string installKey = headerValues.FirstOrDefault();
            Models.Installs.checkinInstall(installKey);

            MednaNetAPIClient.Models.Groups group = null;

            using (Models.MedLaunchChatEntities db = new Models.MedLaunchChatEntities())
            {
                var install = (from q in db.installs
                               where q.code == installKey
                               select q).FirstOrDefault();

                if (install != null)
                {
                    group = (from q in db.groups
                             where q.id == id
                             select new MednaNetAPIClient.Models.Groups()
                             {
                                 groupDescription = q.group_description,
                                 groupName = q.group_name,
                                 groupOwner = q.group_owner,
                                 id = q.id
                             }).FirstOrDefault();
                }

                return Ok(group);

            }
        }

        [Route("api/v1/groups/{id}/messages")]
        [HttpPost]
        public IHttpActionResult CreateMessage(int id, MednaNetAPIClient.Models.Messages message)
        {
            IEnumerable<string> headerValues = Request.Headers.GetValues("Authorization");
            string installKey = headerValues.FirstOrDefault();
            Models.Installs.checkinInstall(installKey);

            var APIReturn = new Models.APIReturn();
            APIReturn.returnMessage = "something";
            using (Models.MedLaunchChatEntities db = new Models.MedLaunchChatEntities())
            {
                var install = (from q in db.installs
                               where q.code == installKey
                               select q).FirstOrDefault();

                if (install != null)
                {
                    if (install.banned)
                    {
                        //Return user is perm banned
                        APIReturn.returnMessage = "You have been banned from chat.";
                    }
                    else
                    {
                        if (install.temp_ban)
                        {
                            //Return user is temp banned, pass back temp_ban_end for when ban will end
                            APIReturn.returnMessage = "You have been temporarily banned from chat. This ban will end on " + install.temp_ban_end.ToString();
                        }
                        else
                        {
                            var newRecord = new Models.message();

                            newRecord.code = message.code;
                            newRecord.message1 = message.message;
                            newRecord.name = install.username;
                            newRecord.posted_on = DateTime.Now;
                            newRecord.channel = id;

                            db.messages.Add(newRecord);
                            db.SaveChanges();

                        }
                    }
                }
                else
                {
                    //Return can't find install
                    APIReturn.returnMessage = "Unable to find install.";
                }
            }

            return Ok(APIReturn);
        }

        [Route("api/v1/groups/{id}/messages")]
        [HttpGet]
        public IHttpActionResult GetMessages(int id)
        {
            IEnumerable<string> headerValues = Request.Headers.GetValues("Authorization");
            string installKey = headerValues.FirstOrDefault();
            Models.Installs.checkinInstall(installKey);

            List<MednaNetAPIClient.Models.Messages> messages = null;

            using (Models.MedLaunchChatEntities db = new Models.MedLaunchChatEntities())
            {
                var install = (from q in db.installs
                               where q.code == installKey
                               select q).FirstOrDefault();

                if (install != null)
                {
                    //var m = from q in db.groups
                    //     where q.id == id && q.group_members.Select(b => b.install_id).Contains(install.id)
                    //    select q.messages;

                    messages =
                       (from g in db.groups
                        from m in g.messages
                        from gm in g.group_members
                        where gm.install_id == install.id && g.id == id
                        select new MednaNetAPIClient.Models.Messages()
                        {
                            channel = g.id,
                            code = m.code,
                            message = m.message1,
                            name = m.name,
                            postedOn = m.posted_on
                        }).ToList();
                }
            }

            return Ok(messages.ToList());
        }

        [Route("api/v1/groups/{id}/messages/from/{from}")]
        [HttpGet]
        public IHttpActionResult GetMessagesFrom(int id, string from)
        {
            IEnumerable<string> headerValues = Request.Headers.GetValues("Authorization");
            string installKey = headerValues.FirstOrDefault();
            Models.Installs.checkinInstall(installKey);

            DateTime fromDate = DateTime.ParseExact(from, "yyyyMMddTHHmmss", System.Globalization.CultureInfo.InvariantCulture).ToLocalTime();

            List<MednaNetAPIClient.Models.Messages> messages = null;

            using (Models.MedLaunchChatEntities db = new Models.MedLaunchChatEntities())
            {
                var install = (from q in db.installs
                               where q.code == installKey
                               select q).FirstOrDefault();

                if (install != null)
                {

                    messages =
                        (from g in db.groups
                         from m in g.messages
                         from gm in g.group_members
                         where m.posted_on > fromDate && gm.install_id == install.id
                         select new MednaNetAPIClient.Models.Messages()
                         {
                             channel = g.id,
                             code = m.code,
                             message = m.message1,
                             name = m.name,
                             postedOn = m.posted_on
                         }).ToList();



                }
            }

            return Ok(messages);
        }


        [Route("api/v1/discord/channels")]
        [HttpGet]
        public IHttpActionResult GetDiscordChannels()
        {
            IEnumerable<string> headerValues = Request.Headers.GetValues("Authorization");
            string installKey = headerValues.FirstOrDefault();
            Models.Installs.checkinInstall(installKey);

            List<MednaNetAPIClient.Models.Channels> channels = null;

            using (Models.MedLaunchChatEntities db = new Models.MedLaunchChatEntities())
            {
                channels =  (from q in db.discord_channels
                               select new MednaNetAPIClient.Models.Channels()
                               {
                                   channelName = q.channel_name,
                                   id = q.id
                               }).ToList();
            }

            return Ok(channels);
        }

        [Route("api/v1/discord/channels/{id}")]
        [HttpGet]
        public IHttpActionResult GetDiscordChannel(int id)
        {
            IEnumerable<string> headerValues = Request.Headers.GetValues("Authorization");
            string installKey = headerValues.FirstOrDefault();
            Models.Installs.checkinInstall(installKey);

            MednaNetAPIClient.Models.Channels channel = null;

            using (Models.MedLaunchChatEntities db = new Models.MedLaunchChatEntities())
            {
                channel = (from q in db.discord_channels
                           where q.id == id
                            select new MednaNetAPIClient.Models.Channels()
                            {
                                channelName = q.channel_name,
                                id = q.id
                            }).FirstOrDefault();
            }

            return Ok(channel);
        }

        [Route("api/v1/discord/channels/{id}/messages")]
        [HttpGet]
        public IHttpActionResult GetChannelMessages(int id)
        {
            IEnumerable<string> headerValues = Request.Headers.GetValues("Authorization");
            string installKey = headerValues.FirstOrDefault();
            Models.Installs.checkinInstall(installKey);

            List<MednaNetAPIClient.Models.Messages> messages = null;

            using (Models.MedLaunchChatEntities db = new Models.MedLaunchChatEntities())
            {
                messages = (from q in db.discord_messages
                            where q.channel == id && !q.clients_ignore
                            select new MednaNetAPIClient.Models.Messages()
                            {
                                channel = q.channel,
                                code = q.code,
                                message = q.message,
                                name = q.name,
                                postedOn = q.posted_on,
                                id = q.id
                            }).ToList();
            }

            return Ok(messages);
        }

        [Route("api/v1/discord/channels/{id}/messages")]
        [HttpPost]
        public IHttpActionResult CreateDiscordMessage(int id, MednaNetAPIClient.Models.Messages message)
        {
            IEnumerable<string> headerValues = Request.Headers.GetValues("Authorization");
            string installKey = headerValues.FirstOrDefault();
            Models.Installs.checkinInstall(installKey);

            string username = "";
            bool clientIgnore = false;
            

            var newRecord = new Models.discord_messages();
            using (Models.MedLaunchChatEntities db = new Models.MedLaunchChatEntities())
            {
                var install = (from q in db.installs
                               where q.code == installKey
                               select q).FirstOrDefault();

                if (install != null)
                {
                    if (install.banned)
                    {
                       
                    }
                    else
                    {
                        if (install.temp_ban)
                        {
                           
                        }
                        else
                        {

                            if (installKey == "botInstallKey")
                            {
                                username = message.name;
                               
                            }
                            else
                            {
                                username = install.username;
                            }

                            newRecord = new Models.discord_messages();

                            newRecord.code = message.code;
                            newRecord.message = message.message;
                            newRecord.name = username;
                            newRecord.posted_on = DateTime.Now;
                            newRecord.channel = id;
                            newRecord.clients_ignore = clientIgnore;

                            db.discord_messages.Add(newRecord);
                            db.SaveChanges();

                        }
                    }
                }
                else
                {
                   
                }
            }

            return Ok(newRecord);
        }


        [Route("api/v1/discord/channels/{id}/messages/last")]
        [HttpGet]
        public IHttpActionResult GetChannelLastMessages(int id)
        {
            IEnumerable<string> headerValues = Request.Headers.GetValues("Authorization");
            string installKey = headerValues.FirstOrDefault();
            Models.Installs.checkinInstall(installKey);



            MednaNetAPIClient.Models.Messages message = null;

            using (Models.MedLaunchChatEntities db = new Models.MedLaunchChatEntities())
            {
                var install = (from q in db.installs
                               where q.code == installKey
                               select q).FirstOrDefault();

                if (install != null)
                {

                    message =
                        (from g in db.discord_channels
                         from m in g.discord_messages
                         where !m.clients_ignore && m.channel == id
                         orderby m.id descending
                         select new MednaNetAPIClient.Models.Messages()
                         {
                             channel = g.id,
                             code = m.code,
                             message = m.message,
                             name = m.name,
                             postedOn = m.posted_on,
                             id = m.id
                         }).FirstOrDefault();
                }
            }

            return Ok(message);
        }

        [Route("api/v1/discord/channels/{id}/messages/from/{from}")]
        [HttpGet]
        public IHttpActionResult GetChannelMessagesFrom(int id, string from)
        {
            IEnumerable<string> headerValues = Request.Headers.GetValues("Authorization");
            string installKey = headerValues.FirstOrDefault();
            Models.Installs.checkinInstall(installKey);

            DateTime fromDate = DateTime.ParseExact(from, "yyyyMMddTHHmmss", System.Globalization.CultureInfo.InvariantCulture).ToLocalTime();

            List<MednaNetAPIClient.Models.Messages> messages = null;

            using (Models.MedLaunchChatEntities db = new Models.MedLaunchChatEntities())
            {
                var install = (from q in db.installs
                               where q.code == installKey
                               select q).FirstOrDefault();

                if (install != null)
                {

                    messages =
                        (from g in db.discord_channels
                         from m in g.discord_messages
                         where m.posted_on >= fromDate && !m.clients_ignore && m.channel == id
                         orderby m.posted_on
                         select new MednaNetAPIClient.Models.Messages()
                         {
                             channel = g.id,
                             code = m.code,
                             message = m.message,
                             name = m.name,
                             postedOn = m.posted_on,
                             id = m.id
                         }).ToList();
                }
            }

            return Ok(messages);
        }


        [Route("api/v1/discord/channels/{id}/messages/after/{messageId}")]
        [HttpGet]
        public IHttpActionResult GetChannelMessagesAfterMessageId(int id, int messageId)
        {
            IEnumerable<string> headerValues = Request.Headers.GetValues("Authorization");
            string installKey = headerValues.FirstOrDefault();
            Models.Installs.checkinInstall(installKey);

            List<MednaNetAPIClient.Models.Messages> messages = null;

            using (Models.MedLaunchChatEntities db = new Models.MedLaunchChatEntities())
            {
                var install = (from q in db.installs
                               where q.code == installKey
                               select q).FirstOrDefault();

                if (install != null)
                {

                    messages =
                        (from g in db.discord_channels
                         from m in g.discord_messages
                         where m.id > messageId && !m.clients_ignore && m.channel == id
                         orderby m.posted_on
                         select new MednaNetAPIClient.Models.Messages()
                         {
                             channel = g.id,
                             code = m.code,
                             message = m.message,
                             name = m.name,
                             postedOn = m.posted_on,
                             id = m.id
                         }).ToList();
                }
            }

            return Ok(messages);
        }


        [Route("api/v1/discord/users")]
        [HttpPost]
        public IHttpActionResult AddDiscordUser(List<MednaNetAPIClient.Models.Users> users)
        {
            IEnumerable<string> headerValues = Request.Headers.GetValues("Authorization");
            string installKey = headerValues.FirstOrDefault();
            Models.Installs.checkinInstall(installKey);



            if (installKey == "botInstallKey")
            {
                using (Models.MedLaunchChatEntities db = new Models.MedLaunchChatEntities())
                {
                    //Delete all users in table
                    db.Database.ExecuteSqlCommand("TRUNCATE TABLE [discord_users]");

                    foreach(var user in users)
                    {
                        var u = new Models.discord_users();

                        u.is_online = user.isOnline;
                        u.username = user.username;
                        u.user_discord_id = user.discordId;

                        db.discord_users.Add(u);
                        db.SaveChanges();
                    }
                }
            }

            return Ok();
        }

        [Route("api/v1/discord/users")]
        [HttpGet]
        public IHttpActionResult GetOnlineUsers()
        {
            IEnumerable<string> headerValues = Request.Headers.GetValues("Authorization");
            string installKey = headerValues.FirstOrDefault();
            Models.Installs.checkinInstall(installKey);

            List<MednaNetAPIClient.Models.Users> users = null;

            
            using (Models.MedLaunchChatEntities db = new Models.MedLaunchChatEntities())
            {
                var install = (from q in db.installs
                                where q.code == installKey
                                select q).FirstOrDefault();

                if (install != null)
                {

                    users = (from q in db.discord_users
                                where q.is_online == true

                                select new MednaNetAPIClient.Models.Users()
                                {
                                    discordId = q.user_discord_id,
                                    id = q.id,
                                    username = q.username
                                }).ToList();
                }
            }


         

            return Ok(users);
        }
    }
}
