using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using myshop.Entities.Models;
using myshop.Entities.Repositories;

namespace myshop.Controllers
{
    public class ErrorController : Controller
    {
        private readonly IUnitOfWork unitOfWork;
        private readonly ILogger<ErrorController> logger;

        public ErrorController
            (
                IUnitOfWork unitOfWork,
                ILogger<ErrorController> logger
            )
        {
            this.unitOfWork = unitOfWork;
            this.logger = logger;
        }

        public IActionResult Index(string errorId)
        {
            ViewData["ErrorId"] = errorId;
            return View("Error");
        }
    }
}
