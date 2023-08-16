using Bulky.DataAccess.Repository.IRepository;
using Bulky.Models;
using Bulky.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace BulkyWeb.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProductController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        private readonly IWebHostEnvironment _webHostEnvironment;

        public ProductController(IUnitOfWork unitOfWork, IWebHostEnvironment webHostEnvironment)
        {
            _unitOfWork = unitOfWork;
            _webHostEnvironment = webHostEnvironment;
        }

        public IActionResult Index()
        {
            List<Product> products = _unitOfWork.Product.GetAll().ToList();

            return View(products);
        }

        //Commenting out to combine Create and Update to Upsert.
        //public IActionResult Create()
        //{
        //    IEnumerable<SelectListItem> CategoryList = _unitOfWork.Category.GetAll().Select(c => new SelectListItem
        //    {
        //        Text = c.Name,
        //        Value = c.Id.ToString()
        //    });

        //    //Using Viewbag
        //    //ViewBag.CategoryList = CategoryList;

        //    //Using ViewData
        //    //ViewData["CategoryList"] = CategoryList;

        //    //Using View Model
        //    ProductVM productVM = new()
        //    {
        //        Product = new Product(),
        //        CategoryList = CategoryList
        //    };

        //    return View(productVM);
        //}

        public IActionResult Upsert(int? id)
        {
            IEnumerable<SelectListItem> CategoryList = _unitOfWork.Category.GetAll().Select(c => new SelectListItem
            {
                Text = c.Name,
                Value = c.Id.ToString()
            });

            //Using Viewbag
            //ViewBag.CategoryList = CategoryList;

            //Using ViewData
            //ViewData["CategoryList"] = CategoryList;

            //Using View Model
            ProductVM productVM = new() {
                CategoryList = CategoryList
            };

            if (id == null || id == 0)
            {
                //If no id then return create view.
                productVM.Product = new Product();

                return View(productVM);
            }
            else
            {
                productVM.Product = _unitOfWork.Product.GetFirstOrDefault(c => c.Id == id);
                return View(productVM);
            }
        }

        //Changing to Upsert
        //[HttpPost]
        //public IActionResult Create(ProductVM productVM, IFormFile? file)
        //{
        //    if (ModelState.IsValid)
        //    {
        //        _unitOfWork.Product.Add(productVM.Product);

        //        _unitOfWork.Save();

        //        TempData["Success"] = "Product added successfully";

        //        return RedirectToAction("Index");
        //    }

        //    productVM.CategoryList = _unitOfWork.Category.GetAll().Select(c => new SelectListItem
        //    {
        //        Text = c.Name,
        //        Value = c.Id.ToString()
        //    });

        //    return View(productVM);
        //}

        [HttpPost]
        public IActionResult Upsert(ProductVM productVM, IFormFile? file)
        {
            if (ModelState.IsValid)
            {
                string wwwRootPath = _webHostEnvironment.WebRootPath;

                if (file != null)
                {
                    string fileName = Guid.NewGuid().ToString() + Path.GetExtension(file.FileName);
                    string productPath = Path.Combine(wwwRootPath, @"images\product");

                    if (!string.IsNullOrEmpty(productVM.Product.ImageUrl))
                    {
                        //delete old image
                        var oldImagePath = Path.Combine(wwwRootPath, productVM.Product.ImageUrl.TrimStart('\\'));

                        if (System.IO.File.Exists(oldImagePath))
                        {
                            System.IO.File.Delete(oldImagePath);
                        }
                    }

                    using (var fileStream = new FileStream(Path.Combine(productPath, fileName), FileMode.Create))
                    {
                        file.CopyTo(fileStream);
                    }

                    productVM.Product.ImageUrl = @"\images\product\" + fileName;
                }

                if (productVM.Product.Id == 0)
                {
                    _unitOfWork.Product.Add(productVM.Product);
                }
                else
                {
                    _unitOfWork.Product.Update(productVM.Product);
                }
                

                _unitOfWork.Save();

                TempData["Success"] = "Product created successfully";

                return RedirectToAction("Index");
            }
            else
            {
                productVM.CategoryList = _unitOfWork.Category.GetAll().Select(c => new SelectListItem
                {
                    Text = c.Name,
                    Value = c.Id.ToString()
                });

                return View(productVM);
            }
        }

        public IActionResult Edit(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }

            Product? result = _unitOfWork.Product.GetFirstOrDefault(u => u.Id == id);

            if (result == null)
                return NotFound();

            return View(result);
        }

        [HttpPost]
        public IActionResult Edit(Product product)
        {
            if (ModelState.IsValid)
            {

                //_categoryRepo.Update(obj);
                _unitOfWork.Product.Update(product);

                // _categoryRepo.Save();
                _unitOfWork.Save();

                TempData["Success"] = "Product updated successfully";

                return RedirectToAction("Index", "Product");
            }
            return View();
        }

        public IActionResult Delete(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }

            Product? result = _unitOfWork.Product.GetFirstOrDefault(u => u.Id == id);

            if (result == null)
                return NotFound();

            return View(result);
        }

        [HttpPost, ActionName("Delete")]
        public IActionResult DeletePOST(int? id)
        {
            Product product = _unitOfWork.Product.GetFirstOrDefault(e => e.Id == id);

            if (product == null)
                return NotFound();

            _unitOfWork.Product.Remove(product);

            _unitOfWork.Save();

            TempData["Success"] = "Product deleted successfully";

            return RedirectToAction("Index", "Product");
        }
    }
}
