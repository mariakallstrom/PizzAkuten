using System.Linq;
using Microsoft.AspNetCore.Mvc;
using PizzAkuten.Data;
using PizzAkuten.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using PizzAkuten.Services;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;
using System.Security.Claims;

namespace PizzAkuten.Controllers
{
    public class OrderController : Controller
    {

        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly OrderService _service;

        public OrderController(ApplicationDbContext context, UserManager<ApplicationUser> userManager, OrderService service)
        {
            _context = context;
            _userManager = userManager;
            _service = service;
        }
        [Authorize]
        public IActionResult Index()
        {

            return View(_context.Orders.ToList());
        }
        [AllowAnonymous]
        public IActionResult AddToCart(int dishId)
        {
            if(dishId == 0)
            {
                RedirectToAction("Index", "Home");
            }
            _service.SetOrderForCurrentSession(dishId);

            return RedirectToAction("Index", "Home");
        }

        public IActionResult RemoveFromCart(int dishId)
        {
            if (dishId == 0)
            {
                RedirectToAction("Index", "Home");
            }
            _service.RemoveItemFromSession(dishId);

            return RedirectToAction("Index", "Home");
        }

        public async Task<IActionResult> Details(int dishId)
        {
            if (dishId == 0)
            {
                return NotFound();
            }

            var dish = await _context.Dishes.Include(d => d.DishIngredients).ThenInclude(di => di.Ingredient)
                .SingleOrDefaultAsync(m => m.DishId == dishId);
            if (dish == null)
            {
                return NotFound();
            }

            return View(dish);
        }

        public async Task<IActionResult> Edit(int dishId)
        {
            if (dishId == 0)
            {
                return NotFound();
            }
            var ingredients = _context.Ingredients.ToList();
            var dish = await _context.Dishes.Include(d => d.DishIngredients).ThenInclude(di => di.Ingredient)
                .SingleOrDefaultAsync(m => m.DishId == dishId);

            var model = new EditDishViewModel();

            var extraIngredients = _context.ExtraIngredients.ToList();
            foreach (var item in dish.DishIngredients)
            {
                item.Ingredient.IsChecked = true;
            }
            model.EditDish = dish;
            model.ExtraIngredients = extraIngredients;

            if (dish == null)
            {
                return NotFound();
            }

            return View(model);
        }

        [HttpPost]
        public IActionResult Edit(EditDishViewModel model)
        {
            if (model != null)
            {
                _service.AddSpecialDishToCart(model);
                return RedirectToAction("Index", "Home");
            };
            return View();
        }

        public IActionResult ViewOrder()
        {
           var order =  _service.GetOrder();

            return View(order);
        }

        public IActionResult ConfirmOrder()
        {
            var order = _service.GetOrder();
            ClaimsPrincipal currentUser = this.User;
            if(currentUser != null)
            {
                var currentUserID = currentUser.FindFirst(ClaimTypes.NameIdentifier).Value;
                order.ApplicationuserId = currentUserID;
                var text = _service.ConfirmOrder(order);

                ViewBag.ThankYou = text;

                return RedirectToAction("ViewOrder", "Order");
            }
            return RedirectToAction("ViewOrder");
          
        }

    }
}
