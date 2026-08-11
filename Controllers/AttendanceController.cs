using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Data;
using System.Threading.Tasks;
using System.Linq;

namespace AttendanceMonitoringSystem.Controllers
{
    public class AttendanceController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AttendanceController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> SmsLogs()
        {
            var logs = await _context.SmsLogs
                .Include(s => s.Student)
                .OrderByDescending(s => s.SentTimestamp)
                .ToListAsync();
 
            DataTable dataTable = new DataTable();
            dataTable.Columns.Add("Sms_id", typeof(int));
            dataTable.Columns.Add("Student_id", typeof(int));
            dataTable.Columns.Add("Contact_number", typeof(string));
            dataTable.Columns.Add("Message", typeof(string));
            dataTable.Columns.Add("Date_sent", typeof(DateTime));
 
            foreach (var log in logs)
            {
                dataTable.Rows.Add(log.Id, log.StudentId, log.ReceiverNumber, log.MessageContent, log.SentTimestamp);
            }
            return View(dataTable);
        }
    }
}
