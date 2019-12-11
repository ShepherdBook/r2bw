using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using CsvHelper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using r2bw.Data;
using r2bw.Services;

namespace r2bw.Controllers
{

    [Authorize]
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
                .OrderByDescending(p => p.WaiverSignedOn.Date)
                .ThenBy(p => p.LastName)
                .ThenBy(p => p.FirstName)
                .ToListAsync();

            return View(users);
        }

        // GET: Participants/Details/5
        [Authorize(Roles = "Administrator")]
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
        [Authorize(Roles = "Administrator")]
        public IActionResult Create()
        {
            ViewData["Groups"] = new SelectList(_context.Groups.Where(g => g.Active), "Id", "Name");
            ViewData["ShirtSizes"] = this.shirtSizes;
            ViewData["ShirtSexes"] = this.shirtSexes;
            
            return View();
        }

        // GET: Participants/Edit/5
        [Authorize(Roles = "Administrator")]
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
        [Authorize(Roles = "Administrator")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(string id, [Bind("Id,FirstName,LastName,Sex,Size,ShoeSize,DateOfBirth,Street1,Street2,City,State,Zip")] User user)
        {
            if (id != user.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    var toUpdate = await _context.Users.FindAsync(id);

                    toUpdate.FirstName = user.FirstName;
                    toUpdate.LastName = user.LastName;
                    toUpdate.DateOfBirth = user.DateOfBirth;
                    toUpdate.Sex = user.Sex;
                    toUpdate.Size = user.Size;
                    toUpdate.ShoeSize = user.ShoeSize;
                    toUpdate.Street1 = user.Street1;
                    toUpdate.Street2 = user.Street2;
                    toUpdate.City = user.City;
                    toUpdate.State = user.State;
                    toUpdate.Zip = user.Zip;

                    _context.Update(toUpdate);
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

        // GET: Participants/Delete/5
        [Authorize(Roles = "Administrator")]
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
        [Authorize(Roles = "Administrator")]
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

        [Authorize(Roles = "Administrator")]
        [HttpGet]
        public async Task<IActionResult> Export()
        {
            List<User> users = await _context.Users.ToListAsync();

            var stream = new MemoryStream();
            var writeFile = new StreamWriter(stream);
            var csv = new CsvWriter(writeFile);

            csv.Configuration.RegisterClassMap<UserCsvMap>();
            csv.WriteRecords(users);

            stream.Position = 0; //reset stream
            return File(stream, "application/octet-stream", "Users.csv");
        }

        private bool ParticipantExists(string id)
        {
            return _context.Users.Any(e => e.Id == id);
        }
    }
}
