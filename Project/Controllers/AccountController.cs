using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using NETCore.MailKit.Core;
using Project.Core.EF;
using Project.Data.Enteties;
using Project.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Project.Controllers
{
    [Route("api/[controller]/[action]")]
    [Authorize]
    public class AccountController : Controller
    {
        private readonly MyContext _context;
        private readonly IEmailService _EmailService;

        public AccountController(MyContext context, IEmailService es)
        {
            _context = context;
            _EmailService = es;
        }

        [HttpPost]
        public async Task<ActionResult<bool>> AddOrUpdateTimesheetRange([FromBody] TimesheetRange range)
        {
            try
            {
                string id = HttpContext.User.FindFirst("id").Value;
                var user = await _context.Users.AsNoTracking().FirstAsync(x => x.Id == Int32.Parse(id));
                if (user == null)
                    return NotFound();
                foreach (Timesheet item in range.items)
                {
                    item.UserID = Int32.Parse(id);
                    if (item.Month != DateTime.Today.Month || item.Year != DateTime.Today.Year ||
                        (item.Year == user.Last_Accept_Year && item.Month == user.Last_Accept_Month))
                        return BadRequest();
                    var ts = await _context.Timesheets.AsNoTracking()
                        .FirstOrDefaultAsync(x => x.UserID == item.UserID && item.Year == x.Year && item.Month == x.Month &&
                        item.Day == x.Day);
                    if (ts == null)
                        await _context.Timesheets.AddAsync(item);
                    else
                        _context.Timesheets.Update(item);
                }
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex);
            }
        }


        [HttpPost]
        public async Task<ActionResult<Timesheet>> UpdateTimesheet([FromBody] Timesheet item)
        {
            try
            {
                if (item.Month != DateTime.Now.Month || item.Year != DateTime.Now.Year)
                    return BadRequest();
                string id = HttpContext.User.FindFirst("id").Value;
                var user = await _context.Users.AsNoTracking().FirstAsync(x => x.Id == Int32.Parse(id));
                item.UserID = Int32.Parse(id);
                if (item.Year == user.Last_Accept_Year && item.Month == user.Last_Accept_Month)
                    return BadRequest();
                var ts = await _context.Timesheets.AsNoTracking()
                    .FirstOrDefaultAsync(x => x.UserID == item.UserID && item.Year == x.Year && item.Month == x.Month &&
                    item.Day == x.Day);
                if (ts == null)
                {
                    var res = await _context.Timesheets.AddAsync(item);
                    await _context.SaveChangesAsync();
                    return res.Entity;
                }
                else
                {
                    _context.Timesheets.Update(item);
                    await _context.SaveChangesAsync();
                    return item;
                }

            }
            catch (Exception ex)
            {
                return StatusCode(500, ex);
            }
        }


        [HttpGet]
        public async Task<ActionResult<bool>> SendNotification()
        {
            try
            {
                string id = HttpContext.User.FindFirst("id").Value;
                var user = await _context.Users.AsNoTracking().FirstAsync(x => x.Id == Int32.Parse(id));
                if (user == null)
                    return NotFound();
                int sum = await _context.Timesheets
                    .Where(y => y.UserID == Int32.Parse(id) && y.Year == DateTime.Now.Year && y.Month == DateTime.Now.Month)
                    .SumAsync(z => z.Bilable);
                //await _EmailService.SendAsync(user.ManagerEmail, "Job report", Templates.Templates.ManagerLetter(
                //    user.ManagerEmail, user.Name, user.Surname, user.UserName, sum, user.Id, 
                //    DateTime.Today.Year, DateTime.Today.Month), true);
                return true;
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex);
            }
        }

        [HttpGet]
        public async Task<ActionResult<InitialData>> InitialData()
        {
            try
            {
                string id = HttpContext.User.FindFirst("id").Value;
                var user = await _context.Users.AsNoTracking().FirstAsync(x => x.Id == Int32.Parse(id));
                var today = DateTime.Now;
                var timesheets = await _context.Timesheets.AsNoTracking()
                    .Where(y => y.UserID == user.Id && y.Year == today.Year && y.Month == today.Month).ToListAsync();
                var notifications = await _context.Notifications.AsNoTracking()
                    .Where(z => z.UserID == user.Id && z.Is_Seen == false).ToListAsync();
                var editable = !(user.Last_Accept_Month == DateTime.Today.Month && user.Last_Accept_Year == DateTime.Today.Year);
                return new InitialData
                {
                    ManagerEmail = user.ManagerEmail,
                    Timesheets = timesheets,
                    Notifications = notifications,
                    Is_Editable = editable
                };
            }
            catch(Exception ex)
            {
                return StatusCode(500, ex);
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<bool>> CheckNotification(int id)
        {
            try
            {
                var notification = await _context.Notifications.AsNoTracking().FirstOrDefaultAsync(x => x.ID == id);
                if (notification == null)
                    return NotFound();
                notification.Is_Seen = true;
                _context.Notifications.Update(notification);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex);
            }
        }

        [HttpPost]
        public async Task<ActionResult<bool>> CheckNotificationGroup([FromBody] NotificationIdRange items)
        {
            try
            {
                foreach (int id in items.Id)
                {
                    var notification = await _context.Notifications.AsNoTracking().FirstOrDefaultAsync(x => x.ID == id);
                    if (notification == null)
                        return NotFound();
                    notification.Is_Seen = true;
                    _context.Notifications.Update(notification);
                }
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex);
            }
        }

        [HttpPost]
        public async Task<ActionResult<TimesheetRange>> TableData([FromBody] TableDate date)
        {
            try
            {
                string id = HttpContext.User.FindFirst("id").Value;
                var user = await _context.Users.AsNoTracking().FirstAsync(x => x.Id == Int32.Parse(id));
                var editable = date.Year == DateTime.Today.Year && date.Month == DateTime.Today.Month
                    && !(user.Last_Accept_Month == DateTime.Today.Month && user.Last_Accept_Year == DateTime.Today.Year);
                var result = await _context.Timesheets.AsNoTracking()
                        .Where(y => y.UserID == Int32.Parse(id) && y.Year == date.Year && y.Month == date.Month).ToListAsync();
                return new TimesheetRange
                {
                    items = result,
                    Is_Editable = editable
                };
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex);
            }            
        }

        [HttpGet("{email}")]
        public async Task<ActionResult<bool>> ChangeManagerEmail(string email)
        {
            try
            {
                string id = HttpContext.User.FindFirst("id").Value;
                var user = await _context.Users.AsNoTracking().FirstAsync(x => x.Id == Int32.Parse(id));
                if (user == null)
                    return NotFound();
                user.ManagerEmail = email;
                _context.Users.Update(user);
                await _context.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex);
            }
        }
    }
}
