using Bulky.DataAccess.Data;
using Bulky.DataAccess.Repository.IRepository;
using Bulky.Models;
using Microsoft.AspNetCore.Mvc;


namespace BulkyWeb.Areas.Admin.Controllers;
[Area("Admin")]
public class CategoryController : Controller
{
    private readonly IUnitOfWork _unitOfWork;
    public CategoryController(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }
    public IActionResult Index()
    {
        List<Category> categoryList = _unitOfWork.Category.GetAll().ToList();
        return View(categoryList);
    }

    public IActionResult Create()
    {
        return View();
    }
    [HttpPost]
    public IActionResult Create(Category obj)
    {
        if (obj.Name == obj.DisplayOrder.ToString())
        {
            ModelState.AddModelError("name", "The DisplayOrder cannot exactly match the name");
        }
        if (obj.Name.ToLower() == "test")
        {
            ModelState.AddModelError("", "Test mode");
        }
        if (ModelState.IsValid)
        {
            _unitOfWork.Category.Add(obj);
            _unitOfWork.Save();
            TempData["success"] = "Category created successfully";
            return RedirectToAction("Index");
        }
        return View();
    }

    public IActionResult Edit(int? id)
    {
        if (id == null || id == 0)
            return NotFound();

        // Działa tylko na kluczu głównym
        Category? categoryFromDb = _unitOfWork.Category.Get(u => u.CategoryId == id);
        // FirstOrDefault - spróbuje znaleźć, jeśli się nie uda to zrobi null object
        //Category? categoryFromDb1 = _unitOfWork.Categories.FirstOrDefault<Category>(u=>u.CategoryId == id);
        // Przydatne gdy potrzeba więcej obliczeń
        //Category? categoryFromDb2 = _db.Categories.Where(u=>u.CategoryId == id).FirstOrDefault();

        if (categoryFromDb == null)
            return NotFound();

        return View(categoryFromDb);
    }
    [HttpPost]
    public IActionResult Edit(Category obj, int? id)
    {
        if (ModelState.IsValid)
        {
            _unitOfWork.Category.Update(obj);
            _unitOfWork.Save();
            TempData["success"] = "Category edited successfully";
            return RedirectToAction("Index");
        }
        return View(obj);
    }

    public IActionResult Delete(int? id)
    {
        if (id == null || id == 0)
            return NotFound();

        Category? categoryFromDb = _unitOfWork.Category.Get(u => u.CategoryId == id);

        if (categoryFromDb == null)
            return NotFound();

        return View();
    }

    [HttpPost, ActionName("Delete")]
    public IActionResult DeletePOST(int? id)
    {
        Category? obj = _unitOfWork.Category.Get(u => u.CategoryId == id);

        if (obj == null) return NotFound();

        _unitOfWork.Category.Remove(obj);
        _unitOfWork.Save();
        TempData["success"] = "Category deleted successfully";
        return RedirectToAction("Index");
    }
}
