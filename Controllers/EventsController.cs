using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using r2bw.Data;
using r2bw.Models;

namespace r2bw.Controllers
{

    [Authorize]
    public class EventsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public EventsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Events
        public async Task<IActionResult> Index()
        {
            return View(await _context.Meetings
                .Include(e => e.Group)
                .Include(e => e.Attendance)
                .Where(e => e.Active)
                .OrderByDescending(e => e.Timestamp)
                .ToListAsync());
        }

        // GET: Events/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var meeting = await _context.Meetings
                .Include(e => e.Group)
                .Where(e => e.Active)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (meeting == null)
            {
                return NotFound();
            }

            ViewData["Groups"] = new SelectList(_context.Groups.Where(g => g.Active), "Id", "Name");
            return View(meeting);
        }

        // GET: Events/Create
        public IActionResult Create()
        {
            ViewData["Groups"] = new SelectList(_context.Groups.Where(g => g.Active), "Id", "Name");

            return View();
        }

        // POST: Events/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Timestamp,GroupId,Name")] Meeting meeting)
        {
            if (ModelState.IsValid)
            {
                meeting.Active = true;
                _context.Add(meeting);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["Groups"] = new SelectList(_context.Groups.Where(g => g.Active), "Id", "Name");
            return View(meeting);
        }

        // GET: Events/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var @event = await _context.Meetings.FindAsync(id);
            if (@event == null)
            {
                return NotFound();
            }

            ViewData["Groups"] = new SelectList(_context.Groups.Where(g => g.Active), "Id", "Name");

            return View(@event);
        }

        [HttpGet]
        public async Task<IActionResult> Attendance(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var thisMeeting = await _context.Meetings.FindAsync(id);
            if (thisMeeting == null)
            {
                return NotFound();
            }

            var model = new MeetingAttendanceModel();

            thisMeeting.Attendance = _context.Attendance.Where(a => a.Active).Where(a => a.MeetingId == thisMeeting.Id).ToList();

            model.Meeting = thisMeeting;

            if (thisMeeting.Attendance.Count() == 0)
            {
                model.Attendees = _context.Users
                    .Where(p => p.Active)
                    .Where(p => p.GroupId == thisMeeting.GroupId)
                    .Where(p => p.StatusId == (int)ParticipantStatusValue.Active)
                    .Select(p => p.Id)
                    .ToArray();
            }
            else 
            {
                model.Attendees = thisMeeting.Attendance.Select(a => a.UserId).ToArray();
            }

            model.AllParticipants = _context.Users
                .Where(p => p.Active)
                .Include(p => p.Group)
                .Where(p => p.StatusId == (int)ParticipantStatusValue.Active)
                .OrderBy(p => p.FirstName)
                .OrderBy(p => p.LastName)
                .OrderBy(p => p.Group.Name)
                .ToList();

            ViewData["AllUsers"] = new SelectList(model.AllParticipants, "Id", "Name");

            var present = _context.Attendance.Where(a => a.Active).Where(a => a.MeetingId == thisMeeting.Id).Select(a => a.UserId).ToList();

            model.Present = _context.Users
                .Where(p => p.Active)
                .Where(p => p.StatusId == (int)ParticipantStatusValue.Active)
                .Where(p => present.Contains(p.Id))
                .Include(p => p.Group)
                .OrderBy(p => p.FirstName)
                .OrderBy(p => p.LastName)
                .OrderBy(p => p.Group.Name)
                .ToList();

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Attendance(int id, [Bind("Meeting,Attendees")] MeetingAttendanceModel model)
        {
            if (id != model.Meeting.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var existing = _context.Attendance.Where(a => a.Active).Where(a => model.Attendees.Contains(a.UserId) && a.MeetingId == model.Meeting.Id).ToList();
                    var received = model.Attendees
                        .Select(pId => new Attendance(
                            _context.Users.Find(pId), 
                            _context.Meetings.Find(model.Meeting.Id)))
                        .ToList();

                    var toAdd = received.Except(existing).ToList();

                    await _context.AddRangeAsync(toAdd);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EventExists(model.Meeting.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
            }

            return RedirectToAction(nameof(Index));
        }

        // POST: Events/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Timestamp,GroupId,Name,ManualHeadcount,Active")] Meeting meeting)
        {
            if (id != meeting.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(meeting);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EventExists(meeting.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }

            ViewData["Groups"] = new SelectList(_context.Groups.Where(g => g.Active), "Id", "Name");
            return View(meeting);
        }

        // GET: Events/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var @event = await _context.Meetings
                .Where(e => e.Active)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (@event == null)
            {
                return NotFound();
            }

            return View(@event);
        }

        // POST: Events/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var meeting = await _context.Meetings.FindAsync(id);
            meeting.Active = false;
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool EventExists(int id)
        {
            return _context.Meetings.Any(e => e.Id == id);
        }
    }
}
