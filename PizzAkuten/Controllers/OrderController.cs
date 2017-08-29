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
using Microsoft.AspNetCore.Http;

namespace PizzAkuten.Controllers
{
    public class OrderController : Controller
    {

        private readonly ApplicationDbContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly OrderService _service;
        private readonly IEmailSender _emailservice;

        public OrderController(ApplicationDbContext context, UserManager<ApplicationUser> userManager, OrderService service, SignInManager<ApplicationUser> signInManager, IEmailSender emailservice)
        {
            _context = context;
            _userManager = userManager;
            _service = service;
            _signInManager = signInManager;
            _emailservice = emailservice;
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
        public async Task<IActionResult> ConfirmOrder()
        {
            var order = _service.GetOrder();
            if(_signInManager.IsSignedIn(User))
            {
                var currentUser = User;
                order.ApplicationUser = await _userManager.FindByNameAsync(User.Identity.Name);

                _service.SaveOrderToDataBase(order);
                _emailservice.SendOrderConfirmToUser(order);

                return RedirectToAction("Create", "Payment");
            }
          
            var savedOrder = _service.SaveOrderToDataBase(order);
            _emailservice.SendOrderConfirmToUser(order);
            return View(savedOrder);

        }
 

    }
}
