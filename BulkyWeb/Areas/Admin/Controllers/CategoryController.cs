using Bulky.DataAccess.Data;
using Bulky.DataAccess.Repository.IRepository;
using Bulky.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace BulkyWeb.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CategoryController : Controller
    {
        //private readonly ApplicationDbContext _db;
        //private readonly ICategoryRepository _categoryRepo;
        private readonly IUnitOfWork _unitOfWork;

        public CategoryController(IUnitOfWork unitOfWork)
        {
            //Commenting out to apply Repository Pattern to use ApplicationDbContext in 
            //CategoryRepository instead of the CategoryController.
            //_db = db;

            //Commenting out to apply unit of work. Passing in IUnitOfWork which creates
            //its own CategoryRepository instead of doing that here.
            //_categoryRepo = db;

            _unitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {
            //List<Category> categories = _db.Categories.ToList();
            //List<Category> categories = _categoryRepo.GetAll().ToList();
            List<Category> categories = _unitOfWork.Category.GetAll().ToList();

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
                //Adds to a sort of list of tasks that need
                //to be done in the db but doesn't commit any
                //of them. It's good practice to gather up all
                //of the changes you need and commit them at
                //once.

                //_db.Categories.Add(obj);
                //_categoryRepo.Add(obj);
                _unitOfWork.Category.Add(obj);

                //Saves changes to the db

                //_db.SaveChanges();
                //_categoryRepo.Save();
                _unitOfWork.Save();

                //Passing key-value pair to the client side through TempData.
                TempData["Success"] = "Category created successfully";

                return RedirectToAction("Index", "Category");
            }

            return View();
        }

        //No Post attribute defaults to HttpGet.
        //Set up the controller to receive the Category ID
        //when the request is sent.
        public IActionResult Edit(int? id)
        {
            //Check to see if the request is valid.
            if (id == null || id == 0)
            {
                //Return the NotFound() page. Alternatively,
                //you can return the View for an error page
                //you have created.
                return NotFound();
            }

            //Method 1 for retrieving one entry from the DB.
            //Find() only works with the Primary Key
            //Category? result = _db.Categories.Find(id);

            //Method 2 for retrieving one entry from the DB.
            //Linq operation
            //Category result2 = _db.Categories.FirstOrDefault(u => u.Id == id);

            //Method 3 for retrieving one entry from the DB.
            //Linq operation
            //Category result3 = _db.Categories.Where(u => u.Id == id).FirstOrDefault();

            //Category? result = _categoryRepo.GetFirstOrDefault(u => u.Id == id);

            Category? result = _unitOfWork.Category.GetFirstOrDefault(u => u.Id == id);

            if (result == null)
                return NotFound();

            return View(result);
        }

        [HttpPost]
        public IActionResult Edit(Category obj)
        {
            if (ModelState.IsValid)
            {

                //_categoryRepo.Update(obj);
                _unitOfWork.Category.Update(obj);

                // _categoryRepo.Save();
                _unitOfWork.Save();

                TempData["Success"] = "Category updated successfully";

                return RedirectToAction("Index", "Category");
            }
            return View();
        }

        public IActionResult Delete(int? id)
        {
            //Check to see if the request is valid.
            if (id == null || id == 0)
            {
                //Return the NotFound() page. Alternatively,
                //you can return the View for an error page
                //you have created.
                return NotFound();
            }

            //Method 1 for retrieving one entry from the DB.
            //Find() only works with the Primary Key
            //Category result = _db.Categories.Find(id);

            //Method 2 for retrieving one entry from the DB.
            //Linq operation
            //Category result2 = _db.Categories.FirstOrDefault(u => u.Id == id);

            //Method 3 for retrieving one entry from the DB.
            //Linq operation
            //Category result3 = _db.Categories.Where(u => u.Id == id).FirstOrDefault();

            //Applying Unit of Work
            //Category? result = _categoryRepo.GetFirstOrDefault(u => u.Id == id);

            Category? result = _unitOfWork.Category.GetFirstOrDefault(u => u.Id == id);

            if (result == null)
                return NotFound();

            return View(result);
        }

        [HttpPost, ActionName("Delete")]
        public IActionResult DeletePost(int? id)
        {
            //Category? obj = _db.Categories.Find(cat.Id);
            //Category? obj = _categoryRepo.GetFirstOrDefault(u => u.Id == id);

            Category? obj = _unitOfWork.Category.GetFirstOrDefault(u => u.Id == id);

            if (obj == null)
                return NotFound();

            //_categoryRepo.Remove(obj);
            _unitOfWork.Category.Remove(obj);

            //_categoryRepo.Save();
            _unitOfWork.Save();

            TempData["Success"] = "Category deleted successfully";

            return RedirectToAction("Index", "Category");
        }

        //[HttpPost, ActionName("Delete")]
        //public IActionResult DeletePOST(int? id)
        //{
        //    Category? obj = _db.Categories.Find(id);

        //    if (obj == null)
        //        return NotFound();

        //    _db.Categories.Remove(obj);
        //    _db.SaveChanges();

        //    TempData["Success"] = "Category deleted successfully";

        //    return RedirectToAction("Index", "Category");
        //}
    }
}
