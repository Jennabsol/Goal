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
    public class DailySprintsController : Controller
    {
        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public DailySprintsController(ApplicationDbContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }

        private Task<ApplicationUser> GetCurrentUserAsync() => _userManager.GetUserAsync(HttpContext.User);

        //id is SprintGroupId used to add a new dailySprint to a sprint group. 
        // GET: DailySprints
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.DailySprints.Include(d => d.SprintGroup);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: DailySprints/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var dailySprints = await _context.DailySprints
                .Include(d => d.SprintGroup)
                .SingleOrDefaultAsync(m => m.Id == id);
            if (dailySprints == null)
            {
                return NotFound();
            }

            return View(dailySprints);
        }

        // GET: DailySprints/Create
        public IActionResult Create()
        {
            ViewData["SprintGroupId"] = new SelectList(_context.SprintGroup, "Id", "Name");
            return View();
        }

        // POST: DailySprints/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,DateCreated,CurrentState,Notes,SprintGroupId")] DailySprints dailySprints)
        {
            if (ModelState.IsValid)
            {
                _context.Add(dailySprints);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["SprintGroupId"] = new SelectList(_context.SprintGroup, "Id", "Name", dailySprints.SprintGroupId);
            return View(dailySprints);
        }

        // GET: DailySprints/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var dailySprints = await _context.DailySprints.SingleOrDefaultAsync(m => m.Id == id);
            if (dailySprints == null)
            {
                return NotFound();
            }
            ViewData["SprintGroupId"] = new SelectList(_context.SprintGroup, "Id", "Name", dailySprints.SprintGroupId);
            return View(dailySprints);
        }

        // POST: DailySprints/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,DateCreated,CurrentState,Notes,SprintGroupId")] DailySprints dailySprints)
        {
            if (id != dailySprints.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(dailySprints);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!DailySprintsExists(dailySprints.Id))
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
            ViewData["SprintGroupId"] = new SelectList(_context.SprintGroup, "Id", "Name", dailySprints.SprintGroupId);
            return View(dailySprints);
        }

        // GET: DailySprints/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var dailySprints = await _context.DailySprints
                .Include(d => d.SprintGroup)
                .SingleOrDefaultAsync(m => m.Id == id);
            if (dailySprints == null)
            {
                return NotFound();
            }

            return View(dailySprints);
        }

        // POST: DailySprints/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var dailySprints = await _context.DailySprints.SingleOrDefaultAsync(m => m.Id == id);
            _context.DailySprints.Remove(dailySprints);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool DailySprintsExists(int id)
        {
            return _context.DailySprints.Any(e => e.Id == id);
        }
    }
}
