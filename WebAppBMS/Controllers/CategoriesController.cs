using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using WebAppBMS.Constant;
using WebAppBMS.Data;
using WebAppBMS.Models;

namespace WebAppBMS.Controllers;

[Authorize(Roles = SD.StoreOwner_Role + "," + SD.Admin_Role)]
public class CategoriesController : Controller
{
    private readonly ApplicationDbContext _db;

    public CategoriesController(ApplicationDbContext db)
    {
        _db = db;
    }

    // GET
    [Authorize(Roles = SD.Admin_Role)]
    public IActionResult Index(string searchString)
    {
        var listCategories = _db.Categories.ToList();
        if (!String.IsNullOrEmpty(searchString))
        {
            listCategories = listCategories.Where(c => c.Name.Contains(searchString)).ToList();
        }

        return View(listCategories);
    }
    
    [Authorize(Roles = SD.Admin_Role)]
    public IActionResult IndexWaitToApprove(string searchString)
    {
        var listCategories = _db.Categories.Where(_=>_.Status == SD.Category_Status_Added).ToList();
        if (!String.IsNullOrEmpty(searchString))
        {
            listCategories = listCategories.Where(c => c.Name.Contains(searchString)).ToList();
        }

        return View(listCategories);
    }

    [HttpGet]
    public IActionResult Upsert(int? id)
    {
        if (id == null)
        {
            return View(new Category());
        }

        var findCategory = _db.Categories.Find(id);

        return View(findCategory);
    }

    [HttpPost]
    public IActionResult Upsert(Category category)
    {
        if (category.Name != String.Empty)
        {
            if (category.Id == 0)
            {
                category.Status = SD.Category_Status_Added;
                _db.Categories.Add(category);
                _db.SaveChanges();
                return RedirectToAction(nameof(Index));
            }

            category.Status = SD.Category_Status_Added;
            _db.Categories.Update(category);
            _db.SaveChanges();
            return RedirectToAction(nameof(Index));
        }

        return View(category);
    }

    public IActionResult Delete(int id)
    {
        var deleteId = _db.Categories.Find(id);
        _db.Categories.Remove(deleteId);
        _db.SaveChanges();
        return RedirectToAction(nameof(Index));
    }
    
    [Authorize(Roles = SD.Admin_Role)]
    public IActionResult Approved(int id)
    {
        var category = _db.Categories.Find(id);
        category.Status = SD.Category_Status_Approved;
        _db.Categories.Update(category);
        _db.SaveChanges();
        return RedirectToAction(nameof(IndexWaitToApprove));
    }
}