using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Goal.Data;
using Goal.Models;
using Microsoft.AspNetCore.Identity;

namespace Goal.Controllers
{
    public class GoalsController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;

        private readonly ApplicationDbContext _context;

        public GoalsController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }
        private Task<ApplicationUser> GetCurrentUserAsync() => _userManager.GetUserAsync(HttpContext.User);

        // GET: Goals
        public async Task<IActionResult> Index()
        {
            var user = await GetCurrentUserAsync();

            var goals = await _context.Goals
                           .Where(g => g.User == user)
                           .OrderByDescending(g => g.DateCreated)
                           .ToListAsync();

            if (goals.Count > 0)
            {
                return View(goals);
            }
            return View("CreateAGoal");
        }

        // GET: Goals/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var goals = await _context.Goals
                .SingleOrDefaultAsync(m => m.Id == id);
            if (goals == null)
            {
                return NotFound();
            }

            return View(goals);
        }

        // GET: Goals/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Goals/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,BeginningState,DateCreated,Completed")] Goals goals)
        {
            goals.DateCreated = DateTime.Now;
            ModelState.Remove("User");
            goals.User = await GetCurrentUserAsync();
            if (ModelState.IsValid)
            {
   
                _context.Add(goals);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(goals);
        }

        // GET: Goals/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var goals = await _context.Goals.SingleOrDefaultAsync(m => m.Id == id);
            if (goals == null)
            {
                return NotFound();
            }
            return View(goals);
        }

        // POST: Goals/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,BeginningState,DateCreated,Completed")] Goals goals)
        {
            if (id != goals.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(goals);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!GoalsExists(goals.Id))
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
            return View(goals);
        }

        // GET: Goals/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var goals = await _context.Goals
                .SingleOrDefaultAsync(m => m.Id == id);
            if (goals == null)
            {
                return NotFound();
            }

            return View(goals);
        }

        // POST: Goals/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var goals = await _context.Goals.SingleOrDefaultAsync(m => m.Id == id);
            _context.Goals.Remove(goals);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool GoalsExists(int id)
        {
            return _context.Goals.Any(e => e.Id == id);
        }
    }
}
