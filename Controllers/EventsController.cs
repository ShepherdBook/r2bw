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
            return View(await _context.Events
                .Include(e => e.Group)
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

            var @event = await _context.Events
                .Include(e => e.Group)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (@event == null)
            {
                return NotFound();
            }

            ViewData["Groups"] = new SelectList(_context.Groups, "Id", "Name");
            return View(@event);
        }

        // GET: Events/Create
        public IActionResult Create()
        {
            ViewData["Groups"] = new SelectList(_context.Groups, "Id", "Name");

            return View();
        }

        // POST: Events/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Timestamp,GroupId,Name")] Event @event)
        {
            if (ModelState.IsValid)
            {
                _context.Add(@event);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["Groups"] = new SelectList(_context.Groups, "Id", "Name");
            return View(@event);
        }

        // GET: Events/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var @event = await _context.Events.FindAsync(id);
            if (@event == null)
            {
                return NotFound();
            }

            ViewData["Groups"] = new SelectList(_context.Groups, "Id", "Name");

            return View(@event);
        }

        [HttpGet]
        public async Task<IActionResult> Attendance(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var thisEvent = await _context.Events.FindAsync(id);
            if (thisEvent == null)
            {
                return NotFound();
            }

            var model = new EventAttendanceModel();

            thisEvent.Attendance = _context.Attendance.Where(a => a.EventId == thisEvent.Id).ToList();

            model.Event = thisEvent;

            if (thisEvent.Attendance.Count() == 0)
            {
                model.Attendees = _context.Participants
                    .Where(p => p.GroupId == thisEvent.GroupId)
                    .Select(p => p.Id)
                    .ToArray();
            }
            else 
            {
                model.Attendees = thisEvent.Attendance.Select(a => a.ParticipantId).ToArray();
            }

            model.AllParticipants = _context.Participants.ToList();

            ViewData["AllParticipants"] = new SelectList(model.AllParticipants, "Id", "Name");

            var present = _context.Attendance.Where(a => a.EventId == thisEvent.Id).Select(a => a.ParticipantId).ToList();

            model.Present = _context.Participants
                .Where(p => present.Contains(p.Id))
                .Include(p => p.Group)
                .OrderBy(p => p.Group.Name)
                .OrderBy(p => p.LastName)
                .OrderBy(p => p.FirstName)
                .ToList();

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Attendance(int id, [Bind("Event,Attendees")] EventAttendanceModel model)
        {
            if (id != model.Event.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var existing = _context.Attendance.Where(a => model.Attendees.Contains(a.ParticipantId) && a.EventId == model.Event.Id).ToList();
                    var received = model.Attendees
                        .Select(pId => new Attendance(
                            _context.Participants.Find(pId), 
                            _context.Events.Find(model.Event.Id)))
                        .ToList();

                    var toAdd = received.Except(existing).ToList();

                    await _context.AddRangeAsync(toAdd);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EventExists(model.Event.Id))
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
        public async Task<IActionResult> Edit(int id, [Bind("Id,Timestamp,GroupId,Name")] Event @event)
        {
            if (id != @event.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(@event);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!EventExists(@event.Id))
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

            ViewData["Groups"] = new SelectList(_context.Groups, "Id", "Name");
            return View(@event);
        }

        // GET: Events/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var @event = await _context.Events
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
            var @event = await _context.Events.FindAsync(id);
            _context.Events.Remove(@event);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool EventExists(int id)
        {
            return _context.Events.Any(e => e.Id == id);
        }
    }
}
