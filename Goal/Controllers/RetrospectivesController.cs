using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Goal.Data;
using Goal.Models;
using Goal.Models.RetrospectiveViewModels;

namespace Goal.Controllers
{
    public class RetrospectivesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public RetrospectivesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: Retrospectives
        public async Task<IActionResult> Index(int id)
        {
            //creates new instance of the daily sprint list 

            ListRetrospectivevViewModel viewModel = new ListRetrospectivevViewModel();
            //grabs each daily sprint  for a given sprint group
            var retrospective = await (
               from r in _context.Retrospective
               from sg in _context.SprintGroup
               where r.SprintGroupId == sg.Id
               && sg.Id == id
               select r)
               .OrderByDescending(d => d.DateCreated)
               .ToListAsync();

            var sprintGroup = await _context.SprintGroup.SingleAsync(sg => sg.Id == id);

            if (retrospective.Count < 1)
            {

                return RedirectToAction("Create", new { id = id });
            }

            viewModel.Retrospective = retrospective;
            viewModel.SprintGroup = sprintGroup;

            return View(viewModel);
            //var applicationDbContext = _context.Retrospective.Include(r => r.SprintGroup);
            //return View(await applicationDbContext.ToListAsync());
        }

        // GET: Retrospectives/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var retrospective = await _context.Retrospective
                .Include(r => r.SprintGroup)
                .SingleOrDefaultAsync(m => m.Id == id);
            if (retrospective == null)
            {
                return NotFound();
            }

            return View(retrospective);
        }

        // GET: Retrospectives/Create
        public IActionResult Create(int id)
        {
            CreateRetrospectivevViewModel viewModel = new CreateRetrospectivevViewModel();


            viewModel.SprintGroupId = id;
            viewModel.Retrospective = new Retrospective();
            //ViewData["SprintGroupId"] = new SelectList(_context.SprintGroup, "Id", "Name");
            return View(viewModel);
        }

        // POST: Retrospectives/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateRetrospectivevViewModel viewModel)
        {
            viewModel.Retrospective.DateCreated = DateTime.Now;
            if (ModelState.IsValid)
            {
                viewModel.Retrospective.SprintGroupId = viewModel.SprintGroupId;
                _context.Add(viewModel.Retrospective);
                await _context.SaveChangesAsync();
                var goalSprintGroup = await (
                     from gsg in _context.GoalSprintGroup
                     where gsg.SprintGroupId == viewModel.SprintGroupId
                     select gsg)
                     .ToListAsync();


                var gs = goalSprintGroup.First();
                //return RedirectToAction("Index", "SprintGroups");
                return RedirectToAction("Index", "Retrospectives", new { id = gs.SprintGroupId });
            }
            ViewData["SprintGroupId"] = new SelectList(_context.SprintGroup, "Id", "Name", viewModel.SprintGroupId);
            return View(viewModel);
        }

        // GET: Retrospectives/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var retrospective = await _context.Retrospective.SingleOrDefaultAsync(m => m.Id == id);
            if (retrospective == null)
            {
                return NotFound();
            }
            ViewData["SprintGroupId"] = new SelectList(_context.SprintGroup, "Id", "Name", retrospective.SprintGroupId);
            return View(retrospective);
        }

        // POST: Retrospectives/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,DateCreated,Success,Challenges,EndState,SprintGroupId")] Retrospective retrospective, int SprintGroupId)
        {
            if (id != retrospective.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(retrospective);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!RetrospectiveExists(retrospective.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("Index", "Retrospectives", new { id = SprintGroupId });
                //return RedirectToAction(nameof(Index));
            }
            ViewData["SprintGroupId"] = new SelectList(_context.SprintGroup, "Id", "Name", retrospective.SprintGroupId);
            return View(retrospective);
        }

        // GET: Retrospectives/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var retrospective = await _context.Retrospective
                .Include(r => r.SprintGroup)
                .SingleOrDefaultAsync(m => m.Id == id);
            if (retrospective == null)
            {
                return NotFound();
            }
            DeleteRetrospectivevViewModel viewModel = new DeleteRetrospectivevViewModel();
            //grabs goal by dipping into Goalsprintgroup join table
            var goalSprintGroup = await (
                   from gsg in _context.GoalSprintGroup
                   where gsg.SprintGroupId == retrospective.SprintGroupId
                   select gsg)
                   .ToListAsync();

            //grabs first return
            var gs = goalSprintGroup.First();

            viewModel.Retrospective = retrospective;
            viewModel.SprintGroupId = gs.SprintGroupId;

            return View(viewModel);
        }

        // POST: Retrospectives/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id, int SprintGroupId)
        {
            var retrospective = await _context.Retrospective.SingleOrDefaultAsync(m => m.Id == id);
            _context.Retrospective.Remove(retrospective);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index", "Retrospectives", new { id = SprintGroupId });
            // return RedirectToAction(nameof(Index));
        }

        private bool RetrospectiveExists(int id)
        {
            return _context.Retrospective.Any(e => e.Id == id);
        }
    }
}
