using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using WebAppBMS.Data;

namespace WebAppBMS.Controllers;

public class UsersManagementController: Controller
{
    private readonly ApplicationDbContext _db;
    private readonly UserManager<IdentityUser> _userManager;
    private readonly RoleManager<IdentityRole> _roleManager;

    public UsersManagementController(ApplicationDbContext db, UserManager<IdentityUser> userManager,
        RoleManager<IdentityRole> roleManager)
    {
        _userManager = userManager;
        _db = db;
        _roleManager = roleManager;
    }
    
    //GET
    [HttpGet]
    public async Task<IActionResult> Index()
    {
        //lay ID cua nguoi dang nhap hien tai
        var claimsIdentity = (ClaimsIdentity)User.Identity;
        var claims = claimsIdentity.FindFirst(ClaimTypes.NameIdentifier);
        
        //tranh truong hop xoa nham role cua minh
        var userList = _db.Users.Where(u => u.Id != claims.Value);

        foreach (var user in userList)
        {
            var roleTemp = await _userManager.GetRolesAsync(user);
            user.Role = roleTemp.FirstOrDefault();
        }

        return View(userList.ToList());
    }
    [HttpGet]
    public async Task<IActionResult> Delete(string id)
    {
        var user = await _userManager.FindByIdAsync(id);
        if (user == null)
        {
            return NotFound();
        }

        await _userManager.DeleteAsync(user);
        return RedirectToAction(nameof(Index));
    }


    
}