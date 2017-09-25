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
        //https://apigee.com/about/blog/technology/restful-api-design-what-about-errors
        //https://stackoverflow.com/questions/10732644/best-practice-to-return-errors-in-asp-net-web-api




        [Route("api/v1/installs")]
        [HttpPost]
        public IHttpActionResult CreateInstall()
        {
            string guid = Guid.NewGuid().ToString();

            MednaNetAPIClient.Models.Installs install = null;

            try
            {
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
            }
            catch(Exception ex)
            {
                HttpError err = new HttpError("Error registering new install. ");
                return ResponseMessage(Request.CreateResponse(HttpStatusCode.InternalServerError, err));
            }
            

            return Ok(install);
        }

        [Route("api/v1/installs")]
        [HttpPut]
        public IHttpActionResult UpdateInstall(MednaNetAPIClient.Models.Installs install)
        {
            IEnumerable<string> headerValues = Request.Headers.GetValues("Authorization");
            string installKey = headerValues.FirstOrDefault();

            if (installKey == "")
            {
                return ResponseMessage(Models.Errors.InstallKeyBlank(Request));
            }

            try
            {
                Models.Installs.checkinInstall(installKey);
            }
            catch (Exception ex)
            {
                return ResponseMessage(Models.Errors.InstallCheckinFailed(Request));
            }

            try
            {
                using (Models.MedLaunchChatEntities db = new Models.MedLaunchChatEntities())
                {
                    var dbInstall = (from q in db.installs
                                     where q.code == installKey
                                     select q).FirstOrDefault();

                    dbInstall.username = install.username;

                    db.SaveChanges();
                }
            }
            catch(Exception ex)
            {
                HttpError err = new HttpError("Username update failed. Check you are passing an existing installKey");
                return ResponseMessage(Request.CreateResponse(HttpStatusCode.InternalServerError, err));
            }
            

            return Ok();
        }

        [Route("api/v1/installs")]
        [HttpGet]
        public IHttpActionResult GetInstall()
        {
            IEnumerable<string> headerValues = Request.Headers.GetValues("Authorization");
            string installKey = headerValues.FirstOrDefault();

            if (installKey == "")
            {
                return ResponseMessage(Models.Errors.InstallKeyBlank(Request));
            }

            try
            {
                Models.Installs.checkinInstall(installKey);
            }
            catch (Exception ex)
            {
                return ResponseMessage(Models.Errors.InstallCheckinFailed(Request));
            }

            MednaNetAPIClient.Models.Installs install = null;

            try
            {
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
            }
            catch (Exception ex)
            {
                HttpError err = new HttpError("Error retrieving install. ");
                return ResponseMessage(Request.CreateResponse(HttpStatusCode.InternalServerError, err));
            }

            return Ok(install);
        }


        [Route("api/v1/users")]
        [HttpGet]
        public IHttpActionResult GetCheckedInUsers()
        {
            IEnumerable<string> headerValues = Request.Headers.GetValues("Authorization");
            string installKey = headerValues.FirstOrDefault();

            if (installKey == "")
            {
                return ResponseMessage(Models.Errors.InstallKeyBlank(Request));
            }

            try
            {
                Models.Installs.checkinInstall(installKey);
            }
            catch (Exception ex)
            {
                return ResponseMessage(Models.Errors.InstallCheckinFailed(Request));
            }

            List<MednaNetAPIClient.Models.Users> users = null;

            try
            {
                using (Models.MedLaunchChatEntities db = new Models.MedLaunchChatEntities())
                {
                    users = (from q in db.installs
                             where System.Data.Entity.DbFunctions.AddMinutes(q.last_checkin, 10) > System.Data.Entity.DbFunctions.AddMinutes(DateTime.Now, -10) && q.code != "botInstallKey"
                             select new MednaNetAPIClient.Models.Users()
                             {
                                 id = q.id,
                                 username = q.username,
                                 isOnline = true
                             }).ToList();
                }
            }
            catch (Exception ex)
            {
                HttpError err = new HttpError("Error retrieving checked in users. ");
                return ResponseMessage(Request.CreateResponse(HttpStatusCode.InternalServerError, err));
            }

            return Ok(users);
        }


        [Route("api/v1/installs/checkin")]
        [HttpGet]
        public IHttpActionResult CheckinInstall()
        {
            IEnumerable<string> headerValues = Request.Headers.GetValues("Authorization");
            string installKey = headerValues.FirstOrDefault();

            if (installKey == "")
            {
                return ResponseMessage(Models.Errors.InstallKeyBlank(Request));
            }

            try
            {
                Models.Installs.checkinInstall(installKey);
            }
            catch (Exception ex)
            {
                return ResponseMessage(Models.Errors.InstallCheckinFailed(Request));
            }

            try
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
            catch (Exception ex)
            {
                return ResponseMessage(Models.Errors.InstallCheckinFailed(Request));
            }
           
            return Ok();

        }

        [Route("api/v1/groups")]
        [HttpGet]
        public IHttpActionResult GetGroups()
        {
            IEnumerable<string> headerValues = Request.Headers.GetValues("Authorization");
            string installKey = headerValues.FirstOrDefault();

            if (installKey == "")
            {
                return ResponseMessage(Models.Errors.InstallKeyBlank(Request));
            }

            try
            {
                Models.Installs.checkinInstall(installKey);
            }
            catch (Exception ex)
            {
                return ResponseMessage(Models.Errors.InstallCheckinFailed(Request));
            }

            List<MednaNetAPIClient.Models.Groups> groups = null;

            try
            {
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
            }
            catch (Exception ex)
            {
                HttpError err = new HttpError("Error retrieving groups. ");
                return ResponseMessage(Request.CreateResponse(HttpStatusCode.InternalServerError, err));
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

            if (installKey == "")
            {
                return ResponseMessage(Models.Errors.InstallKeyBlank(Request));
            }

            try
            {
                Models.Installs.checkinInstall(installKey);
            }
            catch (Exception ex)
            {
                return ResponseMessage(Models.Errors.InstallCheckinFailed(Request));
            }

            try
            {
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
            }
            catch (Exception ex)
            {
                HttpError err = new HttpError("Error creating group. ");
                return ResponseMessage(Request.CreateResponse(HttpStatusCode.InternalServerError, err));
            }

            return Ok();
        }

        [Route("api/v1/groups/{id}")]
        [HttpPost]
        public IHttpActionResult GetGroupById(int id)
        {
            IEnumerable<string> headerValues = Request.Headers.GetValues("Authorization");
            string installKey = headerValues.FirstOrDefault();

            if (installKey == "")
            {
                return ResponseMessage(Models.Errors.InstallKeyBlank(Request));
            }

            try
            {
                Models.Installs.checkinInstall(installKey);
            }
            catch (Exception ex)
            {
                return ResponseMessage(Models.Errors.InstallCheckinFailed(Request));
            }

            MednaNetAPIClient.Models.Groups group = null;

            try
            {
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
            catch (Exception ex)
            {
                HttpError err = new HttpError("Error retrieving group. ");
                return ResponseMessage(Request.CreateResponse(HttpStatusCode.InternalServerError, err));
            }
        }

        [Route("api/v1/groups/{id}/messages")]
        [HttpPost]
        public IHttpActionResult CreateMessage(int id, MednaNetAPIClient.Models.Messages message)
        {
            IEnumerable<string> headerValues = Request.Headers.GetValues("Authorization");
            string installKey = headerValues.FirstOrDefault();


            if (installKey == "")
            {
                return ResponseMessage(Models.Errors.InstallKeyBlank(Request));
            }

            try
            {
                Models.Installs.checkinInstall(installKey);
            }
            catch (Exception ex)
            {
                return ResponseMessage(Models.Errors.InstallCheckinFailed(Request));
            }

            try
            {
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
                        
                    }
                }
            }
            catch (Exception ex)
            {
                HttpError err = new HttpError("Error creating message for group. ");
                return ResponseMessage(Request.CreateResponse(HttpStatusCode.InternalServerError, err));
            }

            return Ok();
        }

        /*[Route("api/v1/groups/{id}/messages")]
        [HttpGet]
        public IHttpActionResult GetMessages(int id)
        {
            IEnumerable<string> headerValues = Request.Headers.GetValues("Authorization");
            string installKey = headerValues.FirstOrDefault();

            if (installKey == "")
            {
                return ResponseMessage(Models.Errors.InstallKeyBlank(Request));
            }

            try
            {
                Models.Installs.checkinInstall(installKey);
            }
            catch (Exception ex)
            {
                return ResponseMessage(Models.Errors.InstallCheckinFailed(Request));
            }

            List<MednaNetAPIClient.Models.Messages> messages = null;

            try
            {
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
            }
            catch (Exception ex)
            {
                HttpError err = new HttpError("Error retrieving messages for group. ");
                return ResponseMessage(Request.CreateResponse(HttpStatusCode.InternalServerError, err));
            }

            return Ok(messages.ToList());
        }*/

        /*[Route("api/v1/groups/{id}/messages/from/{from}")]
        [HttpGet]
        public IHttpActionResult GetMessagesFrom(int id, string from)
        {
            IEnumerable<string> headerValues = Request.Headers.GetValues("Authorization");
            string installKey = headerValues.FirstOrDefault();
            DateTime fromDate;

            if (installKey == "")
            {
                return ResponseMessage(Models.Errors.InstallKeyBlank(Request));
            }

            try
            {
                Models.Installs.checkinInstall(installKey);
            }
            catch (Exception ex)
            {
                return ResponseMessage(Models.Errors.InstallCheckinFailed(Request));
            }


            try
            {
                fromDate = DateTime.ParseExact(from, "yyyyMMddTHHmmss", System.Globalization.CultureInfo.InvariantCulture).ToLocalTime();
            }
            catch (Exception ex)
            {
                HttpError err = new HttpError("Unable to convert from value to a datetime value. ");
                return ResponseMessage(Request.CreateResponse(HttpStatusCode.BadRequest, err));
            }

            List<MednaNetAPIClient.Models.Messages> messages = null;

            try
            {
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
            }
            catch (Exception ex)
            {
                HttpError err = new HttpError("Error retrieving messages for group. ");
                return ResponseMessage(Request.CreateResponse(HttpStatusCode.InternalServerError, err));
            }

            return Ok(messages);
        }*/


        [Route("api/v1/discord/channels")]
        [HttpGet]
        public IHttpActionResult GetDiscordChannels()
        {
            IEnumerable<string> headerValues = Request.Headers.GetValues("Authorization");
            string installKey = headerValues.FirstOrDefault();
            Models.Installs.checkinInstall(installKey);

            if (installKey == "")
            {
                return ResponseMessage(Models.Errors.InstallKeyBlank(Request));
            }

            try
            {
                Models.Installs.checkinInstall(installKey);
            }
            catch (Exception ex)
            {
                return ResponseMessage(Models.Errors.InstallCheckinFailed(Request));
            }

            List<MednaNetAPIClient.Models.Channels> channels = null;

            try
            {
                using (Models.MedLaunchChatEntities db = new Models.MedLaunchChatEntities())
                {
                    channels = (from q in db.discord_channels
                                select new MednaNetAPIClient.Models.Channels()
                                {
                                    channelName = q.channel_name,
                                    id = q.id,
                                    discordId = q.channel_discord_id
                                }).ToList();
                }
            }
            catch (Exception ex)
            {
                HttpError err = new HttpError("Error retrieving discord channels. ");
                return ResponseMessage(Request.CreateResponse(HttpStatusCode.InternalServerError, err));
            }

            return Ok(channels);
        }

        [Route("api/v1/discord/channels/{id}")]
        [HttpGet]
        public IHttpActionResult GetDiscordChannel(int id)
        {
            IEnumerable<string> headerValues = Request.Headers.GetValues("Authorization");
            string installKey = headerValues.FirstOrDefault();

            if (installKey == "")
            {
                return ResponseMessage(Models.Errors.InstallKeyBlank(Request));
            }

            try
            {
                Models.Installs.checkinInstall(installKey);
            }
            catch (Exception ex)
            {
                return ResponseMessage(Models.Errors.InstallCheckinFailed(Request));
            }

            MednaNetAPIClient.Models.Channels channel = null;


            try
            {
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
            }
            catch (Exception ex)
            {
                HttpError err = new HttpError("Error retrieving discord channel. ");
                return ResponseMessage(Request.CreateResponse(HttpStatusCode.InternalServerError, err));
            }

            return Ok(channel);
        }

        [Route("api/v1/discord/channels/{id}/messages")]
        [HttpGet]
        public IHttpActionResult GetChannelMessages(int id)
        {
            IEnumerable<string> headerValues = Request.Headers.GetValues("Authorization");
            string installKey = headerValues.FirstOrDefault();

            if (installKey == "")
            {
                return ResponseMessage(Models.Errors.InstallKeyBlank(Request));
            }

            try
            {
                Models.Installs.checkinInstall(installKey);
            }
            catch (Exception ex)
            {
                return ResponseMessage(Models.Errors.InstallCheckinFailed(Request));
            }

            List<MednaNetAPIClient.Models.Messages> messages = null;

            try
            {
                using (Models.MedLaunchChatEntities db = new Models.MedLaunchChatEntities())
                {
                    //(tempVariable != null) ? (int?)tempVariable.Length : null;

                    messages = (from q in db.discord_messages
                                join du in db.discord_users on q.discord_user_id equals du.user_discord_id
                                join i in db.installs on q.code equals i.code
                                where q.channel == id && !q.clients_ignore
                                select new MednaNetAPIClient.Models.Messages()
                                {
                                    channel = q.channel,
                                    code = q.code,
                                    message = q.message,
                                    user = new MednaNetAPIClient.Models.Users()
                                    {
                                        discordId = du.user_discord_id,
                                        id = i.id,
                                        username = (du.username == null) ? i.username : du.username,
                                        isOnline = ((du.username == null && (System.Data.Entity.DbFunctions.AddMinutes(i.last_checkin, 10) > System.Data.Entity.DbFunctions.AddMinutes(DateTime.Now, -10)) || du.is_online == true)) ? true : false
                                        
                                    },
                                    postedOn = q.posted_on,
                                    id = q.id
                                }).ToList();
                }
            }
            catch (Exception ex)
            {
                HttpError err = new HttpError("Error retrieving messages for discord channel. ");
                return ResponseMessage(Request.CreateResponse(HttpStatusCode.InternalServerError, err));
            }

            return Ok(messages);
        }

        [Route("api/v1/discord/channels/{id}/messages")]
        [HttpPost]
        public IHttpActionResult CreateDiscordMessage(int id, MednaNetAPIClient.Models.Messages message)
        {
            IEnumerable<string> headerValues = Request.Headers.GetValues("Authorization");
            string installKey = headerValues.FirstOrDefault();

            if (installKey == "")
            {
                return ResponseMessage(Models.Errors.InstallKeyBlank(Request));
            }

            try
            {
                Models.Installs.checkinInstall(installKey);
            }
            catch (Exception ex)
            {
                return ResponseMessage(Models.Errors.InstallCheckinFailed(Request));
            }

            string username = "";
            bool clientIgnore = false;

            Models.discord_messages newRecord = null;
            MednaNetAPIClient.Models.Messages insertedMessage = null;
            try
            {
                newRecord = new Models.discord_messages();
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
                                    username = message.user.username;

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
                                newRecord.discord_user_id = message.user.discordId ?? "";
                                db.discord_messages.Add(newRecord);
                                db.SaveChanges();



                                //(tempVariable != null) ? (int?)tempVariable.Length : null;
                                
                               /* insertedMessage = (from q in db.discord_messages
                                                join du in db.discord_users on q.discord_user_id equals du.user_discord_id
                                                join i in db.installs on q.code equals i.code
                                                where q.id == newRecord.id
                                                select new MednaNetAPIClient.Models.Messages()
                                                {
                                                    channel = q.channel,
                                                    code = q.code,
                                                    message = q.message,
                                                    user = new MednaNetAPIClient.Models.Users()
                                                    {
                                                        discordId = du.user_discord_id,
                                                        id = i.id,
                                                        username = (du.username == null) ? i.username : du.username,
                                                        isOnline = ((du.username == null && (System.Data.Entity.DbFunctions.AddMinutes(i.last_checkin, 10) > System.Data.Entity.DbFunctions.AddMinutes(DateTime.Now, -10)) || du.is_online == true)) ? true : false

                                                    },
                                                    postedOn = q.posted_on,
                                                    id = q.id
                                                }).FirstOrDefault();*/

                                insertedMessage =
                                (from g in db.discord_channels
                                 from m in g.discord_messages
                                 join du in db.discord_users on m.discord_user_id equals du.user_discord_id into du2
                                 from discordUsers in du2.DefaultIfEmpty()
                                 join i in db.installs on m.code equals i.code into i2
                                 from medLaunchInstall in i2.DefaultIfEmpty()
                                 where m.id == newRecord.id
                                 select new MednaNetAPIClient.Models.Messages()
                                 {
                                     channel = g.id,
                                     code = m.code,
                                     message = m.message,
                                     user = new MednaNetAPIClient.Models.Users()
                                     {
                                         discordId = discordUsers.user_discord_id,
                                         id = medLaunchInstall.id,
                                         username = (discordUsers.username == null) ? medLaunchInstall.username : discordUsers.username,
                                         isOnline = ((discordUsers.username == null && (System.Data.Entity.DbFunctions.AddMinutes(medLaunchInstall.last_checkin, 10) > System.Data.Entity.DbFunctions.AddMinutes(DateTime.Now, -10)) || discordUsers.is_online == true)) ? true : false

                                     },
                                     postedOn = m.posted_on,
                                     id = m.id
                                 }).FirstOrDefault();

                            }
                        }
                    }
                    else
                    {

                    }
                }
            }
            catch (Exception ex)
            {
                HttpError err = new HttpError("Error creating message for discord channel. ");
                return ResponseMessage(Request.CreateResponse(HttpStatusCode.InternalServerError, err));
            }

            

            return Ok(insertedMessage);
        }


        [Route("api/v1/discord/channels/{id}/messages/last")]
        [HttpGet]
        public IHttpActionResult GetChannelLastMessages(int id)
        {
            IEnumerable<string> headerValues = Request.Headers.GetValues("Authorization");
            string installKey = headerValues.FirstOrDefault();

            if (installKey == "")
            {
                return ResponseMessage(Models.Errors.InstallKeyBlank(Request));
            }

            try
            {
                Models.Installs.checkinInstall(installKey);
            }
            catch (Exception ex)
            {
                return ResponseMessage(Models.Errors.InstallCheckinFailed(Request));
            }


            MednaNetAPIClient.Models.Messages message = null;

            try
            {
                using (Models.MedLaunchChatEntities db = new Models.MedLaunchChatEntities())
                {
                    var install = (from q in db.installs
                                   where q.code == installKey
                                   select q).FirstOrDefault();

                    if (install != null)
                    {

                         /*message =
                             (from g in db.discord_channels
                              from m in g.discord_messages
                              join du in db.discord_users on m.discord_user_id equals du.user_discord_id
                              join i in db.installs on m.code equals i.code
                              where !m.clients_ignore && m.channel == id
                              orderby m.id descending
                              select new MednaNetAPIClient.Models.Messages()
                              {
                                  channel = g.id,
                                  code = m.code,
                                  message = m.message,
                                  user = new MednaNetAPIClient.Models.Users()
                                  {
                                      discordId = du.user_discord_id,
                                      id = i.id,
                                      username = (du.username == null) ? i.username : du.username,
                                      isOnline = ((du.username == null && (System.Data.Entity.DbFunctions.AddMinutes(i.last_checkin, 10) > System.Data.Entity.DbFunctions.AddMinutes(DateTime.Now, -10)) || du.is_online == true)) ? true : false

                                  },
                                  postedOn = m.posted_on,
                                  id = m.id
                              }).FirstOrDefault();*/


                        message =
                            (from g in db.discord_channels
                             from m in g.discord_messages
                             join du in db.discord_users on m.discord_user_id equals du.user_discord_id into du2
                             from discordUsers in du2.DefaultIfEmpty()
                             join i in db.installs on m.code equals i.code into i2
                             from medLaunchInstall in i2.DefaultIfEmpty()
                             where !m.clients_ignore && m.channel == id
                             orderby m.id descending
                             select new MednaNetAPIClient.Models.Messages()
                             {
                                 channel = g.id,
                                 code = m.code,
                                 message = m.message,
                                 user = new MednaNetAPIClient.Models.Users()
                                 {
                                     discordId = discordUsers.user_discord_id,
                                     id = medLaunchInstall.id,
                                     username = (discordUsers.username == null) ? medLaunchInstall.username : discordUsers.username,
                                     isOnline = ((discordUsers.username == null && (System.Data.Entity.DbFunctions.AddMinutes(medLaunchInstall.last_checkin, 10) > System.Data.Entity.DbFunctions.AddMinutes(DateTime.Now, -10)) || discordUsers.is_online == true)) ? true : false

                                 },
                                 postedOn = m.posted_on,
                                 id = m.id
                             }).FirstOrDefault();
                    }
                }
            }
            catch (Exception ex)
            {
                HttpError err = new HttpError("Error retrieving last message for discord channel. ");
                return ResponseMessage(Request.CreateResponse(HttpStatusCode.InternalServerError, err));
            }

            return Ok(message);
        }

        [Route("api/v1/discord/channels/{id}/messages/from/{from}")]
        [HttpGet]
        public IHttpActionResult GetChannelMessagesFrom(int id, string from)
        {
            IEnumerable<string> headerValues = Request.Headers.GetValues("Authorization");
            string installKey = headerValues.FirstOrDefault();

            if (installKey == "")
            {
                return ResponseMessage(Models.Errors.InstallKeyBlank(Request));
            }

            try
            {
                Models.Installs.checkinInstall(installKey);
            }
            catch (Exception ex)
            {
                return ResponseMessage(Models.Errors.InstallCheckinFailed(Request));
            }

            DateTime fromDate;

            try
            {
                fromDate = DateTime.ParseExact(from, "yyyyMMddTHHmmss", System.Globalization.CultureInfo.InvariantCulture).ToLocalTime();
            }
            catch (Exception ex)
            {
                HttpError err = new HttpError("Unable to convert from value to a datetime value. ");
                return ResponseMessage(Request.CreateResponse(HttpStatusCode.BadRequest, err));
            }

            

            List<MednaNetAPIClient.Models.Messages> messages = null;

            try
            {
                using (Models.MedLaunchChatEntities db = new Models.MedLaunchChatEntities())
                {
                    var install = (from q in db.installs
                                   where q.code == installKey
                                   select q).FirstOrDefault();

                    if (install != null)
                    {

                        /*messages =
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
                             }).ToList();*/

                        /*var query = from person in people
                                    join pet in pets on person equals pet.Owner into gj
                                    from subpet in gj.DefaultIfEmpty()
                                    select new { person.FirstName, PetName = subpet?.Name ?? String.Empty };*/


                        messages =
                             (from g in db.discord_channels
                              from m in g.discord_messages
                              join du in db.discord_users on m.discord_user_id equals du.user_discord_id into du2
                              from discordUsers in du2.DefaultIfEmpty()
                              join i in db.installs on m.code equals i.code into i2
                              from medLaunchInstall in i2.DefaultIfEmpty()
                              where m.posted_on >= fromDate && !m.clients_ignore && m.channel == id
                              orderby m.posted_on
                              select new MednaNetAPIClient.Models.Messages()
                              {
                                  channel = g.id,
                                  code = m.code,
                                  message = m.message,
                                  user = new MednaNetAPIClient.Models.Users()
                                  {
                                      discordId = discordUsers.user_discord_id,
                                      id = medLaunchInstall.id,
                                      username = (discordUsers.username == null) ? medLaunchInstall.username : discordUsers.username,
                                      isOnline = ((discordUsers.username == null && (System.Data.Entity.DbFunctions.AddMinutes(medLaunchInstall.last_checkin, 10) > System.Data.Entity.DbFunctions.AddMinutes(DateTime.Now, -10)) || discordUsers.is_online == true)) ? true : false

                                  },
                                  postedOn = m.posted_on,
                                  id = m.id
                              }).ToList();



                    }
                }
            }
            catch (Exception ex)
            {
                HttpError err = new HttpError("Error retrieving messages for discord channel. ");
                return ResponseMessage(Request.CreateResponse(HttpStatusCode.InternalServerError, err));
            }

            return Ok(messages);
        }


        [Route("api/v1/discord/channels/{id}/messages/after/{messageId}")]
        [HttpGet]
        public IHttpActionResult GetChannelMessagesAfterMessageId(int id, int messageId)
        {
            IEnumerable<string> headerValues = Request.Headers.GetValues("Authorization");
            string installKey = headerValues.FirstOrDefault();

            if (installKey == "")
            {
                return ResponseMessage(Models.Errors.InstallKeyBlank(Request));
            }

            try
            {
                Models.Installs.checkinInstall(installKey);
            }
            catch (Exception ex)
            {
                return ResponseMessage(Models.Errors.InstallCheckinFailed(Request));
            }

            List<MednaNetAPIClient.Models.Messages> messages = null;

            try
            {
                using (Models.MedLaunchChatEntities db = new Models.MedLaunchChatEntities())
                {
                    var install = (from q in db.installs
                                   where q.code == installKey
                                   select q).FirstOrDefault();

                    if (install != null)
                    {

                        /* messages =
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
                              }).ToList();*/


                        /*  messages =
                               (from g in db.discord_channels
                                from m in g.discord_messages
                                join du in db.discord_users on m.discord_user_id equals du.user_discord_id
                                join i in db.installs on m.code equals i.code
                                where m.id > messageId && !m.clients_ignore && m.channel == id
                                orderby m.posted_on
                                select new MednaNetAPIClient.Models.Messages()
                                {
                                    channel = g.id,
                                    code = m.code,
                                    message = m.message,
                                    user = new MednaNetAPIClient.Models.Users()
                                    {
                                        discordId = du.user_discord_id,
                                        id = i.id,
                                        username = (du.username == null) ? i.username : du.username,
                                        isOnline = ((du.username == null && (System.Data.Entity.DbFunctions.AddMinutes(i.last_checkin, 10) > System.Data.Entity.DbFunctions.AddMinutes(DateTime.Now, -10)) || du.is_online == true)) ? true : false

                                    },
                                    postedOn = m.posted_on,
                                    id = m.id
                                }).ToList();*/


                        messages =
                           (from g in db.discord_channels
                            from m in g.discord_messages
                            join du in db.discord_users on m.discord_user_id equals du.user_discord_id into du2
                            from discordUsers in du2.DefaultIfEmpty()
                            join i in db.installs on m.code equals i.code into i2
                            from medLaunchInstall in i2.DefaultIfEmpty()
                            where m.id > messageId && !m.clients_ignore && m.channel == id
                            orderby m.posted_on
                            select new MednaNetAPIClient.Models.Messages()
                            {
                                channel = g.id,
                                code = m.code,
                                message = m.message,
                                user = new MednaNetAPIClient.Models.Users()
                                {
                                    discordId = discordUsers.user_discord_id,
                                    id = medLaunchInstall.id,
                                    username = (discordUsers.username == null) ? medLaunchInstall.username : discordUsers.username,
                                    isOnline = ((discordUsers.username == null && (System.Data.Entity.DbFunctions.AddMinutes(medLaunchInstall.last_checkin, 10) > System.Data.Entity.DbFunctions.AddMinutes(DateTime.Now, -10)) || discordUsers.is_online == true)) ? true : false

                                },
                                postedOn = m.posted_on,
                                id = m.id
                            }).ToList();

                    }
                }
            }
            catch (Exception ex)
            {
                HttpError err = new HttpError("Error retrieving messages for discord channel. ");
                return ResponseMessage(Request.CreateResponse(HttpStatusCode.InternalServerError, err));
            }

            return Ok(messages);
        }


        [Route("api/v1/discord/users")]
        [HttpPost]
        public IHttpActionResult AddDiscordUser(List<MednaNetAPIClient.Models.Users> users)
        {
            IEnumerable<string> headerValues = Request.Headers.GetValues("Authorization");
            string installKey = headerValues.FirstOrDefault();

            if (installKey == "")
            {
                return ResponseMessage(Models.Errors.InstallKeyBlank(Request));
            }

            try
            {
                Models.Installs.checkinInstall(installKey);
            }
            catch (Exception ex)
            {
                return ResponseMessage(Models.Errors.InstallCheckinFailed(Request));
            }

            try
            {
                if (installKey == "botInstallKey")
                {
                    using (Models.MedLaunchChatEntities db = new Models.MedLaunchChatEntities())
                    {
                        //Delete all users in table
                        db.Database.ExecuteSqlCommand("TRUNCATE TABLE [discord_users]");

                        foreach (var user in users)
                        {
                            var u = new Models.discord_users();

                            u.is_online = user.isOnline;
                            u.username = user.username;
                            u.user_discord_id = user.discordId;

                            db.discord_users.Add(u);
                            db.SaveChanges();
                        }

                        //Add in any discord users like webhook users etc.
                        db.Database.ExecuteSqlCommand("insert into discord_users select name as username, discord_user_id as user_discord_id, 0 as is_online from( select distinct name, discord_user_id from[discord_messages]  where discord_user_id not in (select user_discord_id from discord_users) and discord_user_id <> '') as t1");
                    }
                }
            }
            catch (Exception ex)
            {
                HttpError err = new HttpError("Error adding discord user.");
                return ResponseMessage(Request.CreateResponse(HttpStatusCode.InternalServerError, err));
            }

            return Ok();
        }

        [Route("api/v1/discord/users")]
        [HttpGet]
        public IHttpActionResult GetOnlineUsers()
        {
            IEnumerable<string> headerValues = Request.Headers.GetValues("Authorization");
            string installKey = headerValues.FirstOrDefault();

            if (installKey == "")
            {
                return ResponseMessage(Models.Errors.InstallKeyBlank(Request));
            }

            try
            {
                Models.Installs.checkinInstall(installKey);
            }
            catch (Exception ex)
            {
                return ResponseMessage(Models.Errors.InstallCheckinFailed(Request));
            }

            List<MednaNetAPIClient.Models.Users> users = null;

            try
            {
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
                                     username = q.username,
                                     isOnline = q.is_online
                                 }).ToList();
                    }
                }
            }
            catch (Exception ex)
            {
                HttpError err = new HttpError("Error retrieving discord users.");
                return ResponseMessage(Request.CreateResponse(HttpStatusCode.InternalServerError, err));
            }



            return Ok(users);
        }
    }
}
