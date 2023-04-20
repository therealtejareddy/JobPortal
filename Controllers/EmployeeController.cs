using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using JobPortal.Data;
using JobPortal.Models;

namespace JobPortal.Controllers
{
    public class EmployeeController : Controller
    {
        private readonly ApplicationDBContext _context;

        public EmployeeController(ApplicationDBContext context)
        {
            _context = context;
        }

        // GET: Employee
        public async Task<IActionResult> Index()
        {
              return _context.Jobs != null ? 
                          View(await _context.Jobs.ToListAsync()) :
                          Problem("Entity set 'ApplicationDBContext.Jobs'  is null.");
        }

        // GET: Employee/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.Jobs == null)
            {
                return NotFound();
            }

            var jobModel = await _context.Jobs
                .FirstOrDefaultAsync(m => m.JobId == id);
            if (jobModel == null)
            {
                return NotFound();
            }

            return View(jobModel);
        }

        // GET: Employee/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Employee/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("JobId,JobTitle,JobDescription,JobCompany,JobSalary,JobMajorSkill")] JobModel jobModel)
        {
            int categoryId = Convert.ToInt32(Request.Form["categories"]);
            Console.WriteLine(categoryId);
            jobModel.Category = categoryId;
            jobModel.User = Request.Cookies["user"];
            _context.Add(jobModel);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
        }

        // GET: Employee/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.Jobs == null)
            {
                return NotFound();
            }

            var jobModel = await _context.Jobs.FindAsync(id);
            if (jobModel == null)
            {
                return NotFound();
            }
            return View(jobModel);
        }

        // POST: Employee/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("JobId,JobTitle,JobDescription,JobCompany,JobSalary,JobMajorSkill,Category,User")] JobModel jobModel)
        {
            if (id != jobModel.JobId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(jobModel);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!JobModelExists(jobModel.JobId))
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
            return View(jobModel);
        }

        // GET: Employee/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.Jobs == null)
            {
                return NotFound();
            }

            var jobModel = await _context.Jobs
                .FirstOrDefaultAsync(m => m.JobId == id);
            if (jobModel == null)
            {
                return NotFound();
            }

            return View(jobModel);
        }

        // POST: Employee/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.Jobs == null)
            {
                return Problem("Entity set 'ApplicationDBContext.Jobs'  is null.");
            }
            var jobModel = await _context.Jobs.FindAsync(id);
            if (jobModel != null)
            {
                _context.Jobs.Remove(jobModel);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool JobModelExists(int id)
        {
          return (_context.Jobs?.Any(e => e.JobId == id)).GetValueOrDefault();
        }
    }
}
