using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using NETCore.MailKit.Core;
using Project.Core.EF;
using Project.Data.Enteties;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace Project.Controllers
{
    [Route("api/[controller]/[action]")]
    public class LetterReportController: Controller
    {
        private readonly MyContext _context;
        private readonly IEmailService _EmailService;
        private readonly IConfiguration _config;

        public LetterReportController(MyContext context, IEmailService es, IConfiguration conf)
        {
            _context = context;
            _EmailService = es;
            _config = conf;
        }

        [HttpGet("{id}/{year}/{month}/{status}")]
        public async Task<ActionResult<string>> Notify(int id, int year, int month, int status)
        {
            try
            {
                var user = await _context.Users.AsNoTracking().FirstAsync(x => x.Id == id);
                if (user == null)
                    return NotFound();
                if (user.Last_Accept_Year == year && user.Last_Accept_Month == month)
                    return "You have already accepted this month!";
                var notification = new Notification
                {
                    UserID = id,
                    Is_Seen = false,
                    Is_Approved = Convert.ToBoolean(status),
                    Year = year,
                    Month = month
                };
                await _context.Notifications.AddAsync(notification);
                if (Convert.ToBoolean(status))
                {
                    user.Last_Accept_Month = DateTime.Today.Month;
                    user.Last_Accept_Year = DateTime.Today.Year;
                    _context.Users.Update(user);
                    string boss = _config["Boss"];
                   /* await _EmailService.SendAsync(boss, "Report", Templates.Templates.ReportLetter
                        (user.Surname, user.Name, user.ManagerEmail, user.Id, year, month), true);*/
                }
                await _context.SaveChangesAsync();
                return "All Good! Thakns!";
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex);
            }
        }

        [HttpGet("{id}/{year}/{month}")]
        [Produces(typeof(object))]
        public async Task<ActionResult<object>> Table(int id, int year, int month)
        {
            try
            {
                var user = _context.Users.AsNoTracking().FirstOrDefault(x => x.Id == id);
                if (user == null)
                    return BadRequest();
                var items = await _context.Timesheets.AsNoTracking()
                    .Where(y => y.UserID == id && y.Year == year && y.Month == month).OrderBy(z => z.Day).ToListAsync();
                string result;
                if (user.Is_Contractor)
                    result = Templates.Templates.ContractorTable(month, year, items);
                else
                    result = Templates.Templates.WorkerTable(month, year, items);
                return new ContentResult
                {
                    ContentType = "text/html",
                    StatusCode = (int)HttpStatusCode.OK,
                    Content = result
                };
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex);
            }
        }
        
    }
}
