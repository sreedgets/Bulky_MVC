﻿using Bulky.DataAccess.Repository.IRepository;
using Bulky.Models;
using Microsoft.AspNetCore.Mvc;

namespace BulkyWeb.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProductController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public ProductController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public IActionResult Index()
        {
            List<Product> products = _unitOfWork.Product.GetAll().ToList();

            return View(products);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Create(Product product)
        {
            if (ModelState.IsValid)
            {
                _unitOfWork.Product.Add(product);

                _unitOfWork.Save();

                TempData["Success"] = "Product added successfully";

                return RedirectToAction("Index");
            }

            return View();
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
