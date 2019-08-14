using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using r2bw.Data;

namespace r2bw.Controllers
{

    [Authorize]
    public class ParticipantsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly SelectList shirtSizes;
        private readonly SelectList shirtSexes;

        public ParticipantsController(ApplicationDbContext context)
        {
            _context = context;

            List<string> sizes = new List<string> {"", "XS", "S", "M", "L", "XL", "XXL", "XXXL"};
            shirtSizes = new SelectList(sizes);

            List<string> sexes = new List<string> {"", "Male", "Female", "Other"};
            shirtSexes = new SelectList(sexes);
        }

        // GET: Participants
        public async Task<IActionResult> Index()
        {
            return View(await _context.Users
                .Where(p => p.Active)
                .Include(p => p.Group)
                .OrderBy(p => p.FirstName)
                .OrderBy(p => p.LastName)
                .OrderBy(p => p.Group.Name)
                .Where(p => p.StatusId == (int)ParticipantStatusValue.Active)
                .ToListAsync());
        }

        // GET: Pending sign ups
        public async Task<IActionResult> Pending()
        {
            return View(await _context.Users
                .Where(p => p.Active)
                .Include(p => p.Group)
                .OrderBy(p => p.FirstName)
                .OrderBy(p => p.LastName)
                .OrderBy(p => p.Group.Name)
                .Where(p => p.StatusId == (int)ParticipantStatusValue.Pending)
                .ToListAsync());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Approve([FromForm] string participantId)
        {
            var participantRecord = _context.Users.Find(participantId);

            if (ModelState.IsValid)
            {
                try
                {
                    if (participantRecord != null)
                    {
                        participantRecord.StatusId = (int)ParticipantStatusValue.Active;
                        participantRecord.Active = true;

                        _context.Update(participantRecord);
                        await _context.SaveChangesAsync();
                    }
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ParticipantExists(participantId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
            }
            return RedirectToAction(nameof(Pending));
        }

        // GET: Participants/Details/5
        public async Task<IActionResult> Details(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                return NotFound();
            }

            var participant = await _context.Users.Where(p => p.Active).Include(p => p.Group)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (participant == null)
            {
                return NotFound();
            }

            ViewData["Groups"] = new SelectList(_context.Groups.Where(g => g.Active), "Id", "Name");
            ViewData["ShirtSizes"] = this.shirtSizes;
            ViewData["ShirtSexes"] = this.shirtSexes;

            return View(participant);
        }

        // GET: Participants/Create
        public IActionResult Create()
        {
            ViewData["Groups"] = new SelectList(_context.Groups.Where(g => g.Active), "Id", "Name");
            ViewData["ShirtSizes"] = this.shirtSizes;
            ViewData["ShirtSexes"] = this.shirtSexes;
            
            return View();
        }

        // GET: Participants/Register
        [AllowAnonymous]
        public IActionResult Register()
        {
            ViewData["Groups"] = new SelectList(_context.Groups.Where(g => g.Active), "Id", "Name");
            ViewData["ShirtSizes"] = this.shirtSizes;
            ViewData["ShirtSexes"] = this.shirtSexes;
            
            return View();
        }

        [AllowAnonymous]
        public IActionResult ThankYou()
        {
            return View();
        }

        // POST: Participants/Register
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [AllowAnonymous]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register([Bind("Id,FirstName,LastName,Email,GroupId,Sex,Size,DateOfBirth")] User participant)
        {
            if (ModelState.IsValid)
            {
                participant.StatusId = (int)ParticipantStatusValue.Pending;
                participant.WaiverSignedOn = DateTimeOffset.Now;
                participant.Active = true;
                _context.Add(participant);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(ThankYou));
            }

            ViewData["Groups"] = new SelectList(_context.Groups.Where(g => g.Active), "Id", "Name");
            ViewData["ShirtSizes"] = this.shirtSizes;
            ViewData["ShirtSexes"] = this.shirtSexes;

            return RedirectToAction(nameof(ThankYou));
        }

        // POST: Participants/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,FirstName,LastName,Email,GroupId,Sex,Size,DateOfBirth")] User participant)
        {
            if (ModelState.IsValid)
            {
                participant.StatusId = (int)ParticipantStatusValue.Active;
                participant.Active = true;
                _context.Add(participant);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewData["Groups"] = new SelectList(_context.Groups.Where(g => g.Active), "Id", "Name");
            ViewData["ShirtSizes"] = this.shirtSizes;
            ViewData["ShirtSexes"] = this.shirtSexes;

            return View(participant);
        }

        // GET: Participants/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                return NotFound();
            }

            var participant = await _context.Users.FindAsync(id);
            if (participant == null)
            {
                return NotFound();
            }

            ViewData["Groups"] = new SelectList(_context.Groups.Where(g => g.Active), "Id", "Name");
            ViewData["ShirtSizes"] = this.shirtSizes;
            ViewData["ShirtSexes"] = this.shirtSexes;

            return View(participant);
        }

        // POST: Participants/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("Id,WaiverSignedOn,FirstName,LastName,Email,GroupId,Sex,Size,DateOfBirth,Active,StatusId")] User participant)
        {
            if (id != participant.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(participant);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ParticipantExists(participant.Id))
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
            ViewData["ShirtSizes"] = this.shirtSizes;
            ViewData["ShirtSexes"] = this.shirtSexes;

            return View(participant);
        }

        // GET: Participants/Waiver/5
        public async Task<IActionResult> Waiver(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var participant = await _context.Users.FindAsync(id);
            if (participant == null)
            {
                return NotFound();
            }

            return View(participant);
        }

        // POST: Participants/Waiver/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Waiver(string id)
        {
            var participantRecord = _context.Users.Find(id);

            if (ModelState.IsValid)
            {
                try
                {
                    if (participantRecord != null && participantRecord.WaiverSignedOn == null)
                    {
                        participantRecord.WaiverSignedOn = DateTime.Now;
                        participantRecord.Active = true;
                        
                        _context.Update(participantRecord);
                        await _context.SaveChangesAsync();
                    }
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ParticipantExists(id))
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
            ViewData["ShirtSizes"] = this.shirtSizes;
            ViewData["ShirtSexes"] = this.shirtSexes;

            return View(participantRecord);
        }

        // GET: Participants/Delete/5
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var participant = await _context.Users
                .Where(p => p.Active)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (participant == null)
            {
                return NotFound();
            }

            ViewData["Groups"] = new SelectList(_context.Groups.Where(g => g.Active), "Id", "Name");
            ViewData["ShirtSizes"] = this.shirtSizes;
            ViewData["ShirtSexes"] = this.shirtSexes;

            return View(participant);
        }

        // POST: Participants/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            var participant = await _context.Users.FindAsync(id);
            participant.Active = false;

            var attendance = _context.Attendance.Where(a => a.UserId == participant.Id);
            await attendance.ForEachAsync(a => a.Active = false);
            _context.UpdateRange(attendance);
            
            await _context.SaveChangesAsync();

            return RedirectToAction(nameof(Index));
        }

        private bool ParticipantExists(string id)
        {
            return _context.Users.Any(e => e.Id == id);
        }
    }
}
