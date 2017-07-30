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
        [HttpGet]
        public IHttpActionResult GetInstalls()
        {
            IEnumerable<string> headerValues = Request.Headers.GetValues("Authorization");
            string installKey = headerValues.FirstOrDefault();

            List<MednaNetAPIClient.Data.Installs> installs = null;

            if (installKey == "supersecret")
            {
                using (Models.MedLaunchChatEntities db = new Models.MedLaunchChatEntities())
                {
                    installs = (from q in db.installs
                                select new MednaNetAPIClient.Data.Installs()
                                {
                                    banned = q.banned,
                                    code = q.code,
                                    lastCheckin = q.last_checkin,
                                    registeredOn = q.registered_on,
                                    tempBan = q.temp_ban,
                                    tempBanEnd = q.temp_ban_end,
                                    username = q.username
                                }).ToList();
                }
            }

            return Ok(installs);
        }

        [Route("api/v1/installs")]
        [HttpPost]
        public IHttpActionResult CreateInstall()
        {
            string guid = Guid.NewGuid().ToString();

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
            }

            return Ok(guid);
        }

        //Should only be used as an admin function. Needs to be locked so only called by a specific admin install key
        [Route("api/v1/installs/{installKey}")]
        [HttpGet]
        public IHttpActionResult GetInstall(string installKey)
        {
            MednaNetAPIClient.Data.Installs install = null;

            using (Models.MedLaunchChatEntities db = new Models.MedLaunchChatEntities())
            {
                install = (from q in db.installs
                           where q.code == installKey
                           select new MednaNetAPIClient.Data.Installs()
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
        [HttpGet]
        public IHttpActionResult GetInstall()
        {
            IEnumerable<string> headerValues = Request.Headers.GetValues("Authorization");
            string installKey = headerValues.FirstOrDefault();

            MednaNetAPIClient.Data.Installs install = null;

            using (Models.MedLaunchChatEntities db = new Models.MedLaunchChatEntities())
            {
                install = (from q in db.installs
                           where q.code == installKey
                           select new MednaNetAPIClient.Data.Installs()
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

        [Route("api/v1/installs/checkin")]
        [HttpGet]
        public IHttpActionResult CheckinInstall()
        {
            IEnumerable<string> headerValues = Request.Headers.GetValues("Authorization");
            string installKey = headerValues.FirstOrDefault();

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

            List<MednaNetAPIClient.Data.Groups> groups = null;

            using (Models.MedLaunchChatEntities db = new Models.MedLaunchChatEntities())
            {
                var install = (from q in db.installs
                               where q.code == installKey
                               select q).FirstOrDefault();

                if (install != null)
                {
                    groups = (from q in db.group_members
                              where q.install_id == install.id
                              select new MednaNetAPIClient.Data.Groups()
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
        public IHttpActionResult CreateGroup(MednaNetAPIClient.Data.Groups group)
        {
            var APIReturn = new Models.APIReturn();

            IEnumerable<string> headerValues = Request.Headers.GetValues("Authorization");
            string installKey = headerValues.FirstOrDefault();


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

            MednaNetAPIClient.Data.Groups group = null;

            using (Models.MedLaunchChatEntities db = new Models.MedLaunchChatEntities())
            {
                var install = (from q in db.installs
                               where q.code == installKey
                               select q).FirstOrDefault();

                if (install != null)
                {
                    group = (from q in db.groups
                             where q.id == id
                             select new MednaNetAPIClient.Data.Groups()
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
        public IHttpActionResult CreateMessage(int id, MednaNetAPIClient.Data.Messages message)
        {
            IEnumerable<string> headerValues = Request.Headers.GetValues("Authorization");
            string installKey = headerValues.FirstOrDefault();

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

            List<MednaNetAPIClient.Data.Messages> messages = null;

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
                        select new MednaNetAPIClient.Data.Messages()
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

            DateTime fromDate = DateTime.ParseExact(from, "yyyyMMddTHHmmss", System.Globalization.CultureInfo.InvariantCulture).ToLocalTime();

            List<MednaNetAPIClient.Data.Messages> messages = null;

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
                         select new MednaNetAPIClient.Data.Messages()
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

            List<MednaNetAPIClient.Data.Channels> channels = null;

            using (Models.MedLaunchChatEntities db = new Models.MedLaunchChatEntities())
            {
                channels =  (from q in db.discord_channels
                               select new MednaNetAPIClient.Data.Channels()
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

            MednaNetAPIClient.Data.Channels channel = null;

            using (Models.MedLaunchChatEntities db = new Models.MedLaunchChatEntities())
            {
                channel = (from q in db.discord_channels
                           where q.id == id
                            select new MednaNetAPIClient.Data.Channels()
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

            List<MednaNetAPIClient.Data.Messages> messages = null;

            using (Models.MedLaunchChatEntities db = new Models.MedLaunchChatEntities())
            {
                messages = (from q in db.discord_messages
                            where q.channel == id
                            select new MednaNetAPIClient.Data.Messages()
                            {
                                channel = q.channel,
                                code = q.code,
                                message = q.message,
                                name = q.name,
                                postedOn = q.posted_on
                            }).ToList();
            }

            return Ok(messages);
        }

        [Route("api/v1/discord/channels/{id}/messages")]
        [HttpPost]
        public IHttpActionResult CreateDiscordMessage(int id, MednaNetAPIClient.Data.Messages message)
        {
            IEnumerable<string> headerValues = Request.Headers.GetValues("Authorization");
            string installKey = headerValues.FirstOrDefault();

            string username = "";

            

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

        [Route("api/v1/discord/channels/{id}/messages/from/{from}")]
        [HttpGet]
        public IHttpActionResult GetChannelMessagesFrom(int id, string from)
        {
            IEnumerable<string> headerValues = Request.Headers.GetValues("Authorization");
            string installKey = headerValues.FirstOrDefault();

            DateTime fromDate = DateTime.ParseExact(from, "yyyyMMddTHHmmss", System.Globalization.CultureInfo.InvariantCulture).ToLocalTime();

            List<MednaNetAPIClient.Data.Messages> messages = null;

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
                         where m.posted_on > fromDate
                         select new MednaNetAPIClient.Data.Messages()
                         {
                             channel = g.id,
                             code = m.code,
                             message = m.message,
                             name = m.name,
                             postedOn = m.posted_on
                         }).ToList();
                }
            }

            return Ok(messages);
        }
    }
}
