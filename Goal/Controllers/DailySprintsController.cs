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
using Goal.Models.DailySprintViewModels;

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
        public async Task<IActionResult> Index(int id)
        {

            //creates new instance of the daily sprint list 

            ListDailySprintViewModel viewModel = new ListDailySprintViewModel();
            //grabs each daily sprint  for a given sprint group
            var dailySprint = await (
               from d in _context.DailySprints
               from sg in _context.SprintGroup
               where d.SprintGroupId == sg.Id
               && sg.Id == id
               select d)
               .OrderByDescending(d => d.DateCreated)
               .ToListAsync();

            var sprintGroup = await _context.SprintGroup.SingleAsync(sg => sg.Id == id);

            if (dailySprint.Count < 1)
            {

                return RedirectToAction("Create", new { id = id });
            }

            viewModel.DailySprints = dailySprint;
            viewModel.SprintGroup = sprintGroup;

            return View(viewModel);
            //var applicationDbContext = _context.DailySprints.Include(d => d.SprintGroup);
            //return View(await applicationDbContext.ToListAsync());
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
        // id is SprintGroupId
        // GET: DailySprints/Create
        public IActionResult Create(int id)
        {
            CreateDailySprintViewModel viewModel = new CreateDailySprintViewModel();
           

            viewModel.SprintGroupId = id;
            viewModel.DailySprints = new DailySprints();

            return View(viewModel);
            //ViewData["SprintGroupId"] = new SelectList(_context.SprintGroup, "Id", "Name");
            //return View();
        }

        // POST: DailySprints/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateDailySprintViewModel viewModel)
        {
            viewModel.DailySprints.DateCreated = DateTime.Now;
            if (ModelState.IsValid)
            {
               

                //viewModel.DailySprints = new DailySprints();


                //sets date created to current date
                
                //passes the sprint group Id to the daily sprint
                viewModel.DailySprints.SprintGroupId = viewModel.SprintGroupId;
        
                //adds new dailysprint to db
                _context.Add(viewModel.DailySprints);
                await _context.SaveChangesAsync();

                //grabs each document group for a given project id
                var goalSprintGroup = await (
                    from gsg in _context.GoalSprintGroup
                    where gsg.SprintGroupId == viewModel.SprintGroupId
                    select gsg)
                    .ToListAsync();


                var gs = goalSprintGroup.First();
                //return RedirectToAction("Index", "SprintGroups");
                return RedirectToAction("Index", "DailySprints", new { id = gs.SprintGroupId });
            }
            ViewData["SprintGroupId"] = new SelectList(_context.SprintGroup, "Id", "Name", viewModel.DailySprints.SprintGroupId);
            return View(viewModel);
            //if (ModelState.IsValid)
            //{
            //    _context.Add(dailySprints);
            //    await _context.SaveChangesAsync();
            //    return RedirectToAction(nameof(Index));
            //}
            //ViewData["SprintGroupId"] = new SelectList(_context.SprintGroup, "Id", "Name", dailySprints.SprintGroupId);
            //return View(dailySprints);
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
            //creates a new instance of DeleteDailySprintViewModel model, that contains a sprintGroup and goal ID
            DeleteDailySprintViewModel viewModel = new DeleteDailySprintViewModel();

            //grabs goal by dipping into Goalsprintgroup join table
            var goalSprintGroup = await (
                   from gsg in _context.GoalSprintGroup
                   where gsg.SprintGroupId == dailySprints.SprintGroupId
                   select gsg)
                   .ToListAsync();

            //grabs first return
            var gs = goalSprintGroup.First();

            viewModel.DailySprints = dailySprints;
            viewModel.SprintGroupId = gs.SprintGroupId;

            return View(viewModel);


            //return View(dailySprints);

        }

        // POST: DailySprints/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id, int SprintGroupId)
        {
            var dailySprints = await _context.DailySprints.SingleOrDefaultAsync(m => m.Id == id);
            _context.DailySprints.Remove(dailySprints);
            await _context.SaveChangesAsync();

            

            return RedirectToAction("Index", "SprintGroups", new { id = SprintGroupId });
            //return RedirectToAction(nameof(Index));
        }

        private bool DailySprintsExists(int id)
        {
            return _context.DailySprints.Any(e => e.Id == id);
        }
    }
}
