using BulkyWeb.Data;
using BulkyWeb.Models;
using Microsoft.AspNetCore.Mvc;


namespace BulkyWeb.Controllers;

public class CategoryController : Controller
{
    private readonly ApplicationDbContext _db;
    public CategoryController(ApplicationDbContext db)
    {
        _db = db;    
    }
    public IActionResult Index()
    {
        List<Category> categoryList = _db.Categories.ToList();
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
			_db.Categories.Add(obj);
			_db.SaveChanges();
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
		Category? categoryFromDb = _db.Categories.Find(id);
		// FirstOrDefault - spróbuje znaleźć, jeśli się nie uda to zrobi null object
		//Category? categoryFromDb1 = _db.Categories.FirstOrDefault<Category>(u=>u.CategoryId == id);
		// Przydatne gdy potrzeba więcej obliczeń
		//Category? categoryFromDb2 = _db.Categories.Where(u=>u.CategoryId == id).FirstOrDefault();

		if (categoryFromDb == null)
			return NotFound();

		return View();
	}
	[HttpPost]
	public IActionResult Edit(Category obj)
	{
		if (ModelState.IsValid)
		{
			_db.Categories.Update(obj);
			_db.SaveChanges();
			TempData["success"] = "Category edited successfully";
			return RedirectToAction("Index");
		}
		return View();
	}

	public IActionResult Delete(int? id)
	{
		if (id == null || id == 0)
			return NotFound();

		Category? categoryFromDb = _db.Categories.Find(id);

		if (categoryFromDb == null)
			return NotFound();

		return View();
	}

	[HttpPost, ActionName("Delete")]
	public IActionResult DeletePOST(int? id)
	{
		Category? obj = _db.Categories.Find(id);

		if (obj == null) return NotFound();

		_db.Categories.Remove(obj);
		_db.SaveChanges();
		TempData["success"] = "Category deleted successfully";
		return RedirectToAction("Index");
	}
}
