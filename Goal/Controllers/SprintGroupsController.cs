using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Goal.Data;
using Goal.Models;
using Goal.Models.SprintGroupViewModels;

namespace Goal.Controllers
{
    public class SprintGroupsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public SprintGroupsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: SprintGroups
        public async Task<IActionResult> Index(int id)
        {
            //return View(await _context.SprintGroup.ToListAsync());
            //creates new instance of the DocumentGroupsList
            ListSprintGroupViewModel viewModel = new ListSprintGroupViewModel();


            //grabs each sprint group for a given goal id
            var sprintGroup = await (
                from sg in _context.SprintGroup
                from gsp in _context.GoalSprintGroup
                where sg.Id == gsp.SprintGroupId
                && gsp.GoalsId == id
                select sg)
                .OrderByDescending(s => s.DateCreated)
                .ToListAsync();

            //grabs an instance of the goal
            var goals = await _context.Goals.SingleAsync(g => g.Id == id);

            //creates a new list of DailySprints 
            List<DailySprints> dailySprints = new List<DailySprints>();

            if (sprintGroup == null)
            {
                return NotFound();
            }

            //if there are sprint groups present
            if (sprintGroup.Count > 0)
            {
                //loop through them
                foreach (var s in sprintGroup)
                {
                    //grab any sprints associated with a sprint group and add it to a list
                    var sgSprintGroups = await _context.DailySprints
                                    .Where(d => d.SprintGroupId == s.Id)
                                    .OrderByDescending(d => d.DateCreated)
                                    .ToListAsync();
                    //go through that list and push to our overall DailySprints list
                    foreach (DailySprints sprint in dailySprints)
                    {
                        dailySprints.Add(sprint);
                    }
                }
                //builds the viewModel with a project, list of document groups, and list of documents and returns index view with that viewModel
                viewModel.SprintGroup = sprintGroup;
                viewModel.Goal = goals;
                viewModel.DailySprints = dailySprints;
                return View(viewModel);
            }

            //if there are no document groups for a given project ID, redirect to create a new document group 
            EmptyGoalsViewModel emptyGoal = new EmptyGoalsViewModel();
            emptyGoal.GoalsId = goals.Id;
            return View("EmptyProject", emptyGoal);


            //return View(await _context.SprintGroup.ToListAsync());

        }

        // GET: SprintGroups/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var sprintGroup = await _context.SprintGroup
                .SingleOrDefaultAsync(m => m.Id == id);
            if (sprintGroup == null)
            {
                return NotFound();
            }

            return View(sprintGroup);
        }

        // GET: SprintGroups/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: SprintGroups/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,Name,DateCreated,Completed")] SprintGroup sprintGroup)
        {
            if (ModelState.IsValid)
            {
                _context.Add(sprintGroup);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(sprintGroup);
        }

        // GET: SprintGroups/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var sprintGroup = await _context.SprintGroup.SingleOrDefaultAsync(m => m.Id == id);
            if (sprintGroup == null)
            {
                return NotFound();
            }
            return View(sprintGroup);
        }

        // POST: SprintGroups/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,Name,DateCreated,Completed")] SprintGroup sprintGroup)
        {
            if (id != sprintGroup.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(sprintGroup);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!SprintGroupExists(sprintGroup.Id))
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
            return View(sprintGroup);
        }

        // GET: SprintGroups/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var sprintGroup = await _context.SprintGroup
                .SingleOrDefaultAsync(m => m.Id == id);
            if (sprintGroup == null)
            {
                return NotFound();
            }

            return View(sprintGroup);
        }

        // POST: SprintGroups/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var sprintGroup = await _context.SprintGroup.SingleOrDefaultAsync(m => m.Id == id);
            _context.SprintGroup.Remove(sprintGroup);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool SprintGroupExists(int id)
        {
            return _context.SprintGroup.Any(e => e.Id == id);
        }
    }
}
