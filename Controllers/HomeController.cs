using JobPortal.Data;
using JobPortal.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace JobPortal.Controllers
{
    public class HomeController : Controller
    {
      

        private readonly ApplicationDBContext _context;

        public HomeController(ApplicationDBContext context)
        {
            _context = context;
        }

        public IActionResult Index()
        {
            // get all jobs
            //var allJobs = _context.Jobs.ToList();
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [HttpPost]
        public IActionResult SearchJob()
        {
            string searchQuery = Request.Form["job-search"];
            var searchedJobs = _context.Jobs.ToList().FindAll(job => { 
            if(job.JobTitle.StartsWith(searchQuery) || job.JobCompany.StartsWith(searchQuery) || job.JobDescription.StartsWith(searchQuery))
                {
                    return true;
                }
                else
                {
                    return false;
                }
            });
 
            return View(searchedJobs);
        }

    }
}