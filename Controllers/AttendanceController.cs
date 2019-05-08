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
    public class AttendanceController : Controller
    {
        private readonly ApplicationDbContext _context;

        public AttendanceController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Attendance
        public async Task<IActionResult> Index()
        {
            var events = _context.Events
                .Include(e => e.Attendance)
                .Include(e => e.Group)
                .Where(e => e.Active)
                .OrderByDescending(e => e.Timestamp);

            return View(await events.ToListAsync());
        }

        // GET: Attendance/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var attendance = await _context.Attendance
                .Include(a => a.Event)
                .ThenInclude(e => e.Group)
                .Include(a => a.Participant)
                .Where(a => a.Active)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (attendance == null)
            {
                return NotFound();
            }

            return View(attendance);
        }

        // GET: Attendance/Create
        public IActionResult Create()
        {
            var events = _context.Events.Where(e => e.Active).Include(e => e.Group);
            var participants = _context.Participants.Where(p => p.Active);

            ViewData["Events"] = new SelectList(events, "Id", "DisplayName");
            ViewData["Participants"] = new SelectList(participants, "Id", "Name");

            return View();
        }

        // POST: Attendance/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ParticipantId,EventId")] Attendance attendance)
        {
            if (ModelState.IsValid)
            {
                var recordtoAdd = new Attendance(
                    await _context.Participants.FindAsync(attendance.ParticipantId), 
                    await _context.Events.FindAsync(attendance.EventId)
                );

                recordtoAdd.Active = true;

                await _context.AddAsync(recordtoAdd);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["Events"] = new SelectList(_context.Events, "Id", "Timestamp");
            ViewData["Participants"] = new SelectList(
                _context.Participants.Where(p => p.Active).Where(p => p.StatusId == (int)ParticipantStatusValue.Active)
                , "Id", "Name");
            return View(attendance);
        }

        // GET: Attendance/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var attendance = await _context.Attendance.FindAsync(id);

            if (attendance == null)
            {
                return NotFound();
            }

            ViewData["Events"] = new SelectList(_context.Events.Where(e => e.Active), "Id", "Timestamp");

            ViewData["Participants"] = new SelectList(
                _context.Participants.Where(p => p.Active).Where(p => p.StatusId == (int)ParticipantStatusValue.Active)
                , "Id", "Name");

            return View(attendance);
        }

        // POST: Attendance/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,ParticipantId,EventId")] Attendance attendance)
        {
            if (id != attendance.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(attendance);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!AttendanceExists(attendance.Id))
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

            ViewData["Events"] = new SelectList(_context.Events.Where(e => e.Active), "Id", "Timestamp");

            ViewData["Participants"] = new SelectList(
                _context.Participants.Where(p => p.Active).Where(p => p.StatusId == (int)ParticipantStatusValue.Active)
                , "Id", "Name");

            return View(attendance);
        }

        // GET: Attendance/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var attendance = await _context.Attendance
                .Include(a => a.Event)
                .Include(a => a.Participant)
                .Where(a => a.Active)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (attendance == null)
            {
                return NotFound();
            }

            return View(attendance);
        }

        // POST: Attendance/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var attendance = await _context.Attendance.FindAsync(id);

            attendance.Active = false;

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool AttendanceExists(int id)
        {
            return _context.Attendance.Any(e => e.Id == id);
        }
    }
}
