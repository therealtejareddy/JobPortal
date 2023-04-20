using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using JobPortal.Data;
using JobPortal.Models;
using System.Dynamic;

namespace JobPortal.Controllers
{
    public class AdminController : Controller
    {
        private readonly ApplicationDBContext _context;

        public AdminController(ApplicationDBContext context)
        {
            _context = context;
        }

        // GET: JobModels
        public async Task<IActionResult> Index()
        {
            var jobsData = await _context.Jobs.ToListAsync();
            var categoriesData = await _context.Categories.ToListAsync();
            dynamic myModel = new ExpandoObject();
            myModel.Jobs = jobsData;
            myModel.Categories = categoriesData;
              return _context.Jobs != null ? 
                          View(myModel) :
                          Problem("Entity set 'ApplicationDBContext.Jobs'  is null.");
        }

        public async Task<IActionResult> CategoryIndex()
        {
            return _context.Categories != null ?
                        View(await _context.Categories.ToListAsync()) :
                        Problem("Entity set 'ApplicationDBContext.Jobs'  is null.");
        }

        // GET: JobModels/Details/5
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

        // GET: JobModels/Create
        public IActionResult CreateJob()
        {
            List<SelectListItem> categories = new List<SelectListItem>();
            _context.Categories.ToList().ForEach(item =>
            {
                categories.Add(new SelectListItem { Text = item.CategoryName, Value = item.Id.ToString() });
            });
            ViewData["categories"] = categories;
            return View();
        }

        // POST: JobModels/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateJob([Bind("JobId,JobTitle,JobDescription,JobCompany,JobSalary,JobMajorSkill")] JobModel jobModel)
        {
                int categoryId = Convert.ToInt32(Request.Form["Category"]);
                Console.WriteLine(categoryId);
            Console.WriteLine(Request.Form["caregories"]);
                jobModel.Category = categoryId;
                jobModel.User = Request.Cookies["user"];
                _context.Add(jobModel);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
        }

        // GET: JobModels/CreateCategory
        public IActionResult CreateCategory()
        {
            return View();
        }

        // POST: JobModels/CreateCategory
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateCategory([Bind("CategoryName")] CategoryModel categoryModel)
        {
            if (ModelState.IsValid)
            {
                _context.Add(categoryModel);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(CategoryIndex));
            }
            return View(categoryModel);
        }

        // GET: JobModels/Edit/5
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

        // POST: JobModels/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("JobId,JobTitle,JobDescription,JobCompany,JobSalary,JobMajorSkill")] JobModel jobModel)
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

        // GET: JobModels/Delete/5
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

        // POST: JobModels/Delete/5
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

        public async Task<IActionResult> DeleteCategory(int? id)
        {
            if (id == null || _context.Categories == null)
            {
                return NotFound();
            }

            var cModel = await _context.Categories
                .FirstOrDefaultAsync(m => m.Id == id);
            if (cModel == null)
            {
                return NotFound();
            }

            return View(cModel);
        }

        // POST: JobModels/Delete/5
        [HttpPost, ActionName("DeleteCategory")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmedCategory(int id)
        {
            if (_context.Categories == null)
            {
                return Problem("Entity set 'ApplicationDBContext.Jobs'  is null.");
            }
            var cModel = await _context.Categories.FindAsync(id);
            if (cModel != null)
            {
                _context.Categories.Remove(cModel);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(CategoryIndex));
        }

        private bool JobModelExists(int id)
        {
          return (_context.Jobs?.Any(e => e.JobId == id)).GetValueOrDefault();
        }
    }
}
