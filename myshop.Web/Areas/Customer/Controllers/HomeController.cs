using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using myshop.DataAccess.Implementation;
using myshop.Entities.Models;
using myshop.Entities.Repositories;
using myshop.Entities.ViewModels;
using myshop.ViewModels;
using System.Linq.Expressions;
using System.Security.Claims;
using Utilities;
using X.PagedList.Extensions;

namespace myshop.Web.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class HomeController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly ILogger<HomeController> logger;

        public HomeController
            (
            IUnitOfWork unitOfWork,
            ILogger<HomeController> logger
            )
        {
            _unitOfWork = unitOfWork;
            this.logger = logger;
        }

        public IActionResult Index(int? page)
        {
            var PageNumber = page ?? 1;
            int PageSize = 8;

            var products = _unitOfWork.Product.GetAll().ToPagedList(PageNumber, PageSize);
            return View(products);
        }

        public IActionResult Details(int ProductId)
        {
            var product = _unitOfWork.Product.GetFirstOrDefault(i => i.Id == ProductId, IncludeWord: "Category");

            if (product == null)
            {
                return NotFound();
            }

            ShoppingCart obj = new ShoppingCart()
            {
                ProductId = ProductId,
                Product = product,
                Count = 1
            };

            return View(obj);
        }

        [Authorize]
        public IActionResult Orders() 
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);

            var orderDetails = _unitOfWork.OrderHeader.GetOrderHeadersByUseId(claim.Value);
            return View(orderDetails);
        }

        [Authorize]
        public IActionResult OrderDetails(int id) // Can be synchronous now as repo methods are sync
        {
            var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrEmpty(userId)) { return Unauthorized(); }

            // Define the predicate for filtering
            Expression<Func<OrderHeader, bool>> predicate = oh => oh.Id == id && oh.ApplicationUserId == userId;

            // Define the Include string for related entities
            // For OrderDetails and then Product within OrderDetails, use "OrderDetails.Product"
            // Also include OrderDetails itself if needed directly (often implied by nested include, but explicit doesn't hurt)
            string includeProperties = "OrderDetails,OrderDetails.Product";

            // Fetch the OrderHeader using the repository
            var orderHeader = _unitOfWork.OrderHeader.GetFirstOrDefault(predicate, includeProperties);

            if (orderHeader == null)
            {
                // Order not found or doesn't belong to user
                return NotFound();
            }

            // --- Mapping to ViewModel (using the simplified ViewModel) ---
            var viewModel = new OrderDetailViewModel
            {
                OrderId = orderHeader.Id,
                TotalPrice = orderHeader.TotalPrice,

                // Map the items list - check if OrderDetails was loaded
                Items = orderHeader.OrderDetails?.Select(od => new OrderItemVM
                {
                    ProductId = od.ProductId,
                    // Product might be null if Include didn't work as expected or data issue
                    ProductName = od.Product?.Name ?? "N/A",
                    Quantity = od.Count,
                    PricePerItem = od.Price,
                    ImageUrl = od.Product?.Img
                }).ToList() ?? new List<OrderItemVM>() // Ensure Items is never null
            };
            // --- End Mapping ---

            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Authorize]
        public IActionResult Details(ShoppingCart shoppingCart)
        {
            var claimsIdentity = (ClaimsIdentity)User.Identity;
            var claim = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
            shoppingCart.ApplicationUserId = claim.Value;

            ShoppingCart CartObj = _unitOfWork.ShoppingCart.GetFirstOrDefault(
                 u => u.ApplicationUserId == claim.Value && u.ProductId == shoppingCart.ProductId,
                 IncludeWord : "Product"
            );

            if (CartObj == null) //adding the products to the cart if product not found 
            {
                _unitOfWork.ShoppingCart.Add(shoppingCart);
                _unitOfWork.Complete();
                HttpContext.Session.SetInt32(SD.SessionKey,
                    _unitOfWork.ShoppingCart.GetAll(u => u.ApplicationUserId == claim.Value).Count()
                );
            }
            else
            {
                if(CartObj.Count < CartObj.Product.CountInStock)
                {
                    _unitOfWork.ShoppingCart.IncreaseCount(CartObj, shoppingCart.Count);
                    _unitOfWork.Complete();
                }
            }

            return RedirectToAction("Index");
        }
    }
}
