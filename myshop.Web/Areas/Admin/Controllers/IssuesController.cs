using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using myshop.DataAccess;
using myshop.Entities.Models;

namespace myshop.Web.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class IssuesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public IssuesController(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> Index()
        {
            return View(await _context.Issues.ToListAsync());
        }
    }
}
