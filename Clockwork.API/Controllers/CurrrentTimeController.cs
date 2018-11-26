using System;
using Microsoft.AspNetCore.Mvc;
using Clockwork.API.Models;
using System.Collections.Generic;

namespace Clockwork.API.Controllers
{
    [Route("api/[controller]")]
    public class CurrentTimeController : Controller
    {
        // GET api/currenttime
        [HttpGet]
        [Route("")]
        public IActionResult Get()
        {
            var utcTime = DateTime.UtcNow;
            var serverTime = DateTime.Now;
            var ip = this.HttpContext.Connection.RemoteIpAddress.ToString();

            return Ok(SaveTime(utcTime, serverTime, ip));
        }

        // GET api/currenttime/all
        [HttpGet]
        [Route("all")]
        public IActionResult GetAll()
        {
            var returnList = new List<CurrentTimeQuery>();

            using (var db = new ClockworkContext())
            {
                foreach (var CurrentTimeQuery in db.CurrentTimeQueries)
                {
                    returnList.Add(new CurrentTimeQuery
                    {
                        CurrentTimeQueryId = CurrentTimeQuery.CurrentTimeQueryId,
                        Time = CurrentTimeQuery.Time,
                        ClientIp = CurrentTimeQuery.ClientIp,
                        UTCTime = CurrentTimeQuery.UTCTime
                    });
                }
            }

            return Ok(returnList);
        }

        // GET api/currenttime/{timeZone}
        [HttpGet]
        [Route("{timeZone}")]
        public IActionResult GetByTimeZone(string timeZone)
        {
            TimeZoneInfo tz = TimeZoneInfo.FindSystemTimeZoneById(timeZone);
            DateTime dateTime = TimeZoneInfo.ConvertTimeFromUtc(DateTime.UtcNow, tz);

            var utcTime = DateTime.UtcNow;
            var serverTime = dateTime;
            var ip = this.HttpContext.Connection.RemoteIpAddress.ToString();
            

            return Ok(SaveTime(utcTime, serverTime, ip));
        }

        private CurrentTimeQuery SaveTime(DateTime utcTime, DateTime serverTime, string ip)
        {
            var returnVal = new CurrentTimeQuery
            {
                UTCTime = utcTime,
                ClientIp = ip,
                Time = serverTime
            };

            using (var db = new ClockworkContext())
            {
                db.CurrentTimeQueries.Add(returnVal);
                var count = db.SaveChanges();
                Console.WriteLine("{0} records saved to database", count);
                foreach (var CurrentTimeQuery in db.CurrentTimeQueries)
                {
                    Console.WriteLine(" - {0}", CurrentTimeQuery.UTCTime);
                }
            }
            return returnVal;
        }
    }
}
