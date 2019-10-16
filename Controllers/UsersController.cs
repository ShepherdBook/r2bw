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

    [Authorize(Roles = "Administrator")]
    public class UsersController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly SelectList shirtSizes;
        private readonly SelectList shirtSexes;

        public UsersController(ApplicationDbContext context)
        {
            _context = context;

            List<string> sizes = new List<string> {"", "XS", "S", "M", "L", "XL", "XXL", "XXXL"};
            shirtSizes = new SelectList(sizes);

            List<string> sexes = new List<string> {"", "Male", "Female", "Other"};
            shirtSexes = new SelectList(sexes);
        }

        // GET: Users
        public async Task<IActionResult> Index()
        {
            List<User> users = await _context.Users
                .Where(p => p.Active)
                .Include(p => p.Group)
                .OrderBy(p => p.FirstName)
                .OrderBy(p => p.LastName)
                .OrderBy(p => p.Group.Name)
                .ToListAsync();

            return View(users);
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

        [AllowAnonymous]
        public IActionResult ThankYou()
        {
            return View();
        }

        // POST: Participants/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,FirstName,LastName,Email,GroupId,Sex,Size,DateOfBirth")] User newUser)
        {
            if (ModelState.IsValid)
            {                
                newUser.Active = true;
                newUser.UserName = newUser.Email;
                newUser.WaiverSignedOn = DateTimeOffset.Now;

                _context.Add(newUser);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewData["Groups"] = new SelectList(_context.Groups.Where(g => g.Active), "Id", "Name");
            ViewData["ShirtSizes"] = this.shirtSizes;
            ViewData["ShirtSexes"] = this.shirtSexes;

            return View(newUser);
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
        public async Task<IActionResult> Edit(string id, [Bind("Id,WaiverSignedOn,FirstName,LastName,Email,GroupId,Sex,Size,DateOfBirth,Active,StatusId")] User user)
        {
            if (id != user.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(user);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ParticipantExists(user.Id))
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

            return View(user);
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
