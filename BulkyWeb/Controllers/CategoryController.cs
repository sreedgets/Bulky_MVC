using BulkyWeb.Data;
using BulkyWeb.Models;
using Microsoft.AspNetCore.Mvc;

namespace BulkyWeb.Controllers
{
    public class CategoryController : Controller
    {
        private readonly ApplicationDbContext _db;

        public CategoryController(ApplicationDbContext db)
        {
            _db = db;
        }

        public IActionResult Index()
        {
            List<Category> categories = _db.Categories.ToList();

            return View(categories);
        }

        public IActionResult Create()
        {
            //You don't have to pass a new Category object here for the view to use. 
            //If you declare a model on a view and nothing is passed from the controller
            //an object will be created with empty values.
            return View();
        }

        [HttpPost]
        public IActionResult Create(Category obj)
        {
            if (obj.Name == obj.DisplayOrder.ToString())
            {
                ModelState.AddModelError("Name", "The Display Order cannot exactly match the Name.");
            }

            //ModelState.IsValid takes the obj and checks
            //that its values pass what is required by the data
            //model
            if (ModelState.IsValid)
            {
                //Add adds to a sort of list of tasks that need
                //to be done in the db but doesn't commit any
                //of them. It's good practice to gather up all
                //of the changes you need and commit them at
                //once.
                _db.Categories.Add(obj);

                //Saves changes to the db
                _db.SaveChanges();

                return RedirectToAction("Index", "Category");
            }

            return View();
        }
    }
}
