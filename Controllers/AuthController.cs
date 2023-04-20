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
    public class AuthController : Controller
    {
        private readonly ApplicationDBContext _context;

        public AuthController(ApplicationDBContext context)
        {
            _context = context;
        }

        // GET: Auth
        public async Task<IActionResult> Index()
        {
              return _context.Users != null ? 
                          View(await _context.Users.ToListAsync()) :
                          Problem("Entity set 'ApplicationDBContext.Users'  is null.");
        }


        // GET: Auth/Create
        public IActionResult Register()
        {
            return View();
        }

        // POST: Auth/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register([Bind("Id,UserName,Email,Password,Role")] UserModel userModel)
        {
            if (ModelState.IsValid)
            {
                _context.Add(userModel);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(userModel);
        }
        public IActionResult Login()
        {
                var user = _context.Users.Where(user => user.Id==Request.Cookies["user"]).FirstOrDefault();
                if (user != null)
                {
                    if (user.Role == "Admin")
                    {
                        return RedirectToAction("Index", "Admin");
                    }
                else if (user.Role == "User")
                {
                    return RedirectToAction("Index", "Home");
                }
                else if (user.Role == "Employee")
                {
                    return RedirectToAction("Index", "Employee");
                }
            }
            
            return View();
        }

        // POST: Auth/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login([Bind("UserName,Password")] UserModel userModel)
        {
            var validUser = _context.Users.Where(user => user.Password == userModel.Password && user.UserName == userModel.UserName).FirstOrDefault();
            if (validUser != null)
            {
                // TODO : Redirect to Home
                Response.Cookies.Append("user",validUser.Id);
                if (validUser.Role == "Admin")
                {
                    return RedirectToAction("Index","Admin");
                }else if (validUser.Role == "User")
                {
                    return RedirectToAction("Index", "Home");
                }
                if (validUser.Role == "Employee")
                {
                    return RedirectToAction("Index", "Employee");
                }
            }

            return RedirectToAction(nameof(Register));
        }

    }
}
