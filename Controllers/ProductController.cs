using atelier3.Models;
using atelier3.Models.Repositories;
using atelier3.ViewModels;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Hosting;
using Microsoft.Net.Http.Headers;
using System;
using System.IO;

namespace atelier3.Controllers
{
    public class ProductController : Controller
    {
        private readonly IWebHostEnvironment hostingEnvironment;
        private readonly IProducRepository pro;

        public ProductController(IProducRepository productRepository, IWebHostEnvironment environment)
        {
            this.pro = productRepository;
            this.hostingEnvironment = environment;
        }

        // GET: ProductController
        public IActionResult Index()
        {
            var model = pro.GetAllProduct(); // Method name correction
            return View(model);
        }

        // GET: ProductController/Details/5
        public IActionResult Details(int id)
        {
            var model = pro.GetProduct(id);
            return View(model);
        }

        // GET: ProductController/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: ProductController/Create
        [ValidateAntiForgeryToken]
        [HttpPost]
        public IActionResult Create(ProductCreateVM modelToCreate)
        {
            try
            {
                var files = HttpContext.Request.Form.Files;
                foreach (var Image in files)
                {
                    if (Image != null && Image.Length > 0)
                    {
                        var file = Image;
                        var uploads = Path.Combine(hostingEnvironment.WebRootPath, "images");

                        if (file.Length > 0)
                        {
                            var fileName = ContentDispositionHeaderValue.Parse(file.ContentDisposition).FileName.Trim();
                            System.Console.WriteLine(fileName);

                            using (var fileStream = new FileStream(Path.Combine(uploads, file.FileName), FileMode.Create))
                            {
                                file.CopyTo(fileStream);
                                modelToCreate.PhotoPath = file.FileName;
                            }
                        }
                    }
                }

                Product newP = new Product
                {
                    // Convert the string "Women" or "Men" to enum
                    Category = (Categories)Enum.Parse(typeof(Categories), modelToCreate.Category, true),
                    Price = modelToCreate.Price,
                    // Convert the string "Small" or "Meduim" or "Large" to enum
                    ProductSize = (Size)Enum.Parse(typeof(Size), modelToCreate.ProductSize, true),
                    PhotoPath = modelToCreate.PhotoPath
                };

                pro.Add(newP);

                // Assuming the product creation is successful, you can return a success response
                return Json(new { success = true, message = "Product created successfully" });
            }
            catch (Exception ex)
            {
                // Log the exception or handle it accordingly
                return Json(new { success = false, message = "Error creating product", error = ex.Message });
            }
        }



        // GET: ProductController/Edit/5
        public IActionResult Edit(int id)
        {
            Product p = pro.GetProduct(id);
            if (p == null)
            {
                return NotFound();
            }
            //// Split the string based on the underscore
            //string[] parts = p.PhotoPath.Split('_');

            //// Take the last part of the array
            //string PhotoName = parts.LastOrDefault();

            EditViewModel pEditViewModel = new EditViewModel
            {
                Id = p.ProductID,
                Category = p.Category,
                ProductSize = p.ProductSize,
                Price = p.Price,
                ExistingPhotoPath = p.PhotoPath
            };
            return View(pEditViewModel);
        }

        // POST: ProductController/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(EditViewModel model)
        {
            if (ModelState.IsValid)
            {
                Product e = pro.GetProduct(model.Id);
                if (e == null)
                {
                    return NotFound();
                }

                e.Category = model.Category;
                e.Price = model.Price;
                e.ProductSize = model.ProductSize;

                if (model.Photo != null)
                {
                    if (model.ExistingPhotoPath != null)
                    {
                        string filePath = Path.Combine(hostingEnvironment.WebRootPath, "images", model.ExistingPhotoPath);
                        System.IO.File.Delete(filePath);
                    }

                    e.PhotoPath = ProcessUploadedFile(model);
                }

                pro.Update(e);
                return RedirectToAction(nameof(Index));
            }
            return View(model);
        }

        // GET: ProductController/Delete/5
        public IActionResult Delete(int id)
        {
            var model = pro.GetProduct(id);
            if (model == null)
            {
                return NotFound();
            }
            return View(model);
        }

        // POST: ProductController/Delete/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Delete(int id, Product p)
        {
            try
            {
                pro.Delete(id);
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }

        [NonAction]
        private string ProcessUploadedFile(EditViewModel model)
        {
            string uniqueFileName = null;
            if (model.Photo != null)
            {
                string uploadsFolder = Path.Combine(hostingEnvironment.WebRootPath, "images");
                uniqueFileName = Guid.NewGuid().ToString() + "_" + model.Photo.FileName;
                string filePath = Path.Combine(uploadsFolder, uniqueFileName);
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    model.Photo.CopyTo(fileStream);
                }
            }
            return uniqueFileName;
        }
    }
}
